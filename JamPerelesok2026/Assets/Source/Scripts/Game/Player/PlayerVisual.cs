using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private GameObject _keyView;
    [SerializeField] private GameObject _swordView;
    [SerializeField] private GameObject _lightView;

    private Player _player;
    private PlayerInputHandler _inputHandler;  
    private Vector3 _movement;
    private GameObject _currentShowedView;
   

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;      

        SubscribeToEvents();
    }

    #region >>> ITEM VIEW

    public void OnItemTaked(ItemType type)
    {
        ToggleItemViewByType(type, true);
    }

    public void OnItemDropped(ItemType type)
    {
        ToggleItemViewByType(type, false);
    }

    private void ToggleItemViewByType(ItemType type, bool value)
    {
        if (_currentShowedView != null)
        {
            _currentShowedView.gameObject.SetActive(false);
            _currentShowedView = null;
        }

        switch(type)
        {
            case ItemType.Light:
                {
                    _currentShowedView = _lightView;
                    break;
                }
            case ItemType.Key:
                {
                    _currentShowedView = _keyView;
                    break;
                }
            case ItemType.Sword:
                {
                    _currentShowedView = _swordView;
                    break;
                }
        }

        _currentShowedView.gameObject.SetActive(value);
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.MoveInput += OnMoveInputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.MoveInput -= OnMoveInputChanged;
    }

    private void OnMoveInputChanged(Vector2 moveInput)
    {       
        _movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}
