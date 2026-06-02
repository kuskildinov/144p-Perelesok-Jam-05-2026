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

        // Если есть Interactable для взаимодействия, только активируем его и выходим
        if(_currentInteractable != null)
        {
            _currentInteractable.TryInteract(_player, _currentTakedItem);
            return;
        }
        
        // Если мы не находимся у предмета
        if(_currentDetectedItem == null)
        {
            // Если в руке есть предмет - бросаем и выходим
            if (_currentTakedItem != null)
            {
                DropItem();
            }
            return;
        }
        // Если рядом есть предмет для подбирания
        else
        {
            // Если в руке есть предмет - бросаем и берем предмет, который рядом
            TakeNewItem(_currentDetectedItem);
            _currentDetectedItem.TryTake();
            _currentDetectedItem = null;
        }       
    }

    #region >>> ITEMS

    private void TakeNewItem(Item item)
    {
        if (_currentTakedItem != null)
            DropItem();

        _currentTakedItem = item;       
        _player.OnItemTaked(_currentDetectedItem.Type);
    }

    public void DropItem()
    {
        _player.OnItemDropped(_currentTakedItem.Type);
        _currentTakedItem.TryDrop(this.transform);
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
        if (other.gameObject.TryGetComponent<AttackZone>(out AttackZone attackZone))
        {
            if (!_player.IsCanTakeDamage)
                return;

            if (attackZone.Type == AttackZoneType.Enemy)
            {
                attackZone.Activate();
                _player.TakeDamage(attackZone.DamagerCenter);
            }

            if (attackZone.Type == AttackZoneType.Trap)
            {  
                if(attackZone.TryGetComponent<TrapBlock>(out TrapBlock trap))
                {
                    if(trap.IsActive)
                    {
                        attackZone.Activate();
                        _player.TakeDamage(attackZone.DamagerCenter);
                    }
                }
               
            }
        }

        if (other.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.OnPlayerEnter();
            _currentInteractable = interactable;
        }

        if (other.gameObject.TryGetComponent<LevelBlock>(out LevelBlock levelBlock))
        {
            levelBlock.CanMove = false;
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
            interactable.OnPlayerExit();
            _currentInteractable = null;
        }

        if (other.gameObject.TryGetComponent<LevelBlock>(out LevelBlock levelBlock))
        {
            levelBlock.CanMove = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Item>(out Item item))
        {
            if (item.IsTaked)
                return;

            item.ShowTargetIndicator();
            _currentDetectedItem = item;
        }
    }
}
