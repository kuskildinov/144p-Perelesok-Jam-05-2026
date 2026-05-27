using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Vector2 _moveInput;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody => _player.Rigidbody;

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
        _rigidbody.linearVelocity = _movement * _moveSpeed;
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
        _movement = _moveInput.normalized;
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}
