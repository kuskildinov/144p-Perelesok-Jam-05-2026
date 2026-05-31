using System.Collections;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;
    [Header("Reaction Settings")]
    [SerializeField] private float _knockbackForce = 8f;
    [SerializeField] private float _knockbackDuration = 0.25f;
    [SerializeField] private float _knockbackDrag = 10f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Vector3 _moveInput;
    private Vector3 _movement;

    private Vector3 _externalVelocity;
    private bool _isKnockedBack;

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
        Vector3 move = _movement * _moveSpeed;

        if (_isKnockedBack)
            move = Vector3.zero;

        // плавное затухание knockback
        _externalVelocity = Vector3.Lerp(
            _externalVelocity,
            Vector3.zero,
            _knockbackDrag * Time.deltaTime
        );

        Vector3 finalMove = move + _externalVelocity;

        _player.Controller.Move(finalMove * Time.deltaTime);
    }

    #endregion
    #region >>> KNOCK BACK

    public void KnockBack(Transform damageSource)
    {
        Vector3 direction = (transform.position - damageSource.position).normalized;
        direction.y = 0f;

        StartCoroutine(KnockbackRoutine(direction));
    }

    private IEnumerator KnockbackRoutine(Vector3 direction)
    {
        _isKnockedBack = true;

        _externalVelocity = direction * _knockbackForce;

        yield return new WaitForSeconds(_knockbackDuration);

        _isKnockedBack = false;
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
