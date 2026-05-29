using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovment _playerMovment;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private PlayerInteractions _playerInteractions;
    [SerializeField] private PlayerAttack _playerAttack;

    private CharacterController _characterController;
    private bool _isActive = true;
    private PlayerRoot _root;
    private PlayerInputHandler _inputHandler;
    private LookDirection _currentLookDirection;

    public bool IsActive => _isActive;
    public CharacterController Controller => _characterController;
    public LookDirection CurrentLookDirection => _currentLookDirection;

    public event Action<LookDirection> LookDirectionChanged;

    public void Initialzie(PlayerRoot root)
    {
        _root = root;
        _inputHandler = _root.InputHandler;
        _characterController = GetComponent<CharacterController>();
        
        _playerMovment.Initialize(this, _inputHandler);
        _playerVisual.Initialize(this, _inputHandler);
        _playerInteractions.Initialize(this, _inputHandler);
        _playerAttack.Initialize(this, _inputHandler);

        SubscribeToEvents();
    }

    #region >>> ITEMS

    public Item GetCurrentTakedItem()
    {
        return _playerInteractions.CurrentTakedItem;
    }

    public void OnItemTaked(ItemType type)
    {
        _playerVisual.OnItemTaked(type);
    }

    public void OnItemDropped(ItemType type)
    {
        _playerVisual.OnItemDropped(type);
    }

    public void OnCurrentItemUsed()
    {
        _playerInteractions.OnCurrentItemUsed();
    }

    #endregion
    #region >>> LOOK DIRECTION

    public void UpdateLookDirection(Vector2 input)
    {      
        if (input == Vector2.zero)
            return;

        // Определяем главную ось
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Горизонталь
            _currentLookDirection =
                input.x > 0
                    ? LookDirection.Right
                    : LookDirection.Left;
        }
        else
        {
            // Вертикаль
            _currentLookDirection =
                input.y > 0
                    ? LookDirection.Top
                    : LookDirection.Down;
        }

        LookDirectionChanged?.Invoke(_currentLookDirection);
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
        UpdateLookDirection(moveInput);
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}

public enum LookDirection
{
    Top,
    Down,
    Left,
    Right,
}
