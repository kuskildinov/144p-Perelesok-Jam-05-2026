using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string AnimatorWalkParam = "Walk";

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _lighterOnHand;
    [SerializeField] private Light _light;

    private Player _player;
    private PlayerInputHandler _inputHandler;  
    private Vector3 _movement;
    private GameObject _currentShowedView;

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;       
        _light.range = GlobalVars.LightRadius;
        SubscribeToEvents();
    }

    private void Update()
    {
        if (!_player.IsAlive)
            return;

        WalkAnimationHandler();
    }

    #region >>> ANIMATIONS

    private void WalkAnimationHandler()
    {
        if(_movement.sqrMagnitude > 0)
        {
            _animator.SetBool(AnimatorWalkParam, true);
        }
        else
        {
            _animator.SetBool(AnimatorWalkParam, false);
        }
    }

    #endregion
    #region >>> ITEM VIEW

    public void OnItemTaked(ItemType type)
    {
       if(type == ItemType.Light)
        {
            _lighterOnHand.gameObject.SetActive(true);
        }
    }

    public void OnItemDropped(ItemType type)
    {
        if (type == ItemType.Light)
        {
            _lighterOnHand.gameObject.SetActive(false);
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
