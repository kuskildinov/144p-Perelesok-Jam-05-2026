using System;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Item _currentTakedItem;
    private Item _currentDetectedItem;
    private Interactable _currentInteractable;

    public Item CurrentTakedItem => _currentTakedItem;

    public event Action<ItemType> OnItemTaked;   

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        SubscribeToEvents();
    }

    public void TryInteract()
    {
        if (!_player.IsActive)
            return;

        if(_currentInteractable != null)
        {
            _currentInteractable.TryInteract(_player, _currentTakedItem);
            return;
        }
        
        if(_currentDetectedItem == null)           //если не нужно ничего подбирать
        {
            if (_currentTakedItem != null)
                DropItem();
        }
        else                                       // если можно что то подобрать
        {
            if (_currentTakedItem != null)
                DropItem();
            
            TakeNewItem(_currentDetectedItem);
        }       
    }

    #region >>> ITEMS

    private void TakeNewItem(Item item)
    {
        if (_currentTakedItem != null)
            DropItem();

        _currentTakedItem = item;
        _currentDetectedItem.TryTake();
        _player.OnItemTaked(_currentDetectedItem.Type);
        _currentDetectedItem = null;
    }

    public void DropItem()
    {
        _currentTakedItem.TryDrop(this.transform);
        _player.OnItemDropped(_currentTakedItem.Type);
        _currentTakedItem = null;
    }

    public void OnCurrentItemUsed()
    {
        if (_currentTakedItem == null)
            return;

        _player.OnItemDropped(_currentTakedItem.Type);
        Destroy(_currentTakedItem.gameObject);

        _currentTakedItem = null;
        _currentInteractable = null;
    }
    #endregion   
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.InteractInput += OnInteractInputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.InteractInput -= OnInteractInputChanged;
    }

    private void OnInteractInputChanged()
    {
        TryInteract();
    }
 
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Item>(out Item item))
        {
            if (item.IsTaked)
                return;

            item.ShowTargetIndicator();
            _currentDetectedItem = item;
        }

        if(other.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (!interactable.IsActive)
                return;

            interactable.OnPlayerEnter();
            _currentInteractable = interactable;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Item>(out Item item))
        {
            if (item.IsTaked)
                return;

            item.HideTargetIndicator();
            _currentDetectedItem = null;
        }

        if (other.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (!interactable.IsActive)
                return;

            interactable.OnPlayerExit();
            _currentInteractable = null;
        }
    }
}
