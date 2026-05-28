using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Vector3 _moveInput;
    private Vector3 _movement;
    
    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        SubscribeToEvents();
    }

    private void FixedUpdate()
    {
        if (!_player.IsActive)
            return;

        MoveHandler();
    }

    #region >>> MOVE

    private void MoveHandler()
    {        
        _player.Controller.Move(_movement * _moveSpeed * Time.deltaTime);
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
        _moveInput = moveInput;
        _movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;      
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}
