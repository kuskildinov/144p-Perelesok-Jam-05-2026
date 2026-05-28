using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Transform _spriteTransform;
    private Player _player;
    private PlayerInputHandler _inputHandler;  
    private Vector3 _movement;
    private Camera _targetCamera;

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;
        _targetCamera = Camera.main;

        SubscribeToEvents();
    }

    private void LateUpdate()
    {
        RotateToCamHandler();
    }

    #region >>> LOOK AT CAM

    private void RotateToCamHandler()
    {
        if (_targetCamera == null) return;

        Vector3 directionToCamera = _targetCamera.transform.position - _spriteTransform.position;
        
        //directionToCamera.z = 0;
        directionToCamera.x = 0;
        //directionToCamera.y = 0;

        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            _spriteTransform.rotation = targetRotation;
        }
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
