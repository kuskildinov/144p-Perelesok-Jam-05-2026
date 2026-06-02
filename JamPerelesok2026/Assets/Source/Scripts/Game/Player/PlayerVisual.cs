using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string AnimatorWalkParam = "Walk";
    private const string AnimatorItemParam = "Item";

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;
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

    private void ToggleRotation(bool isLeft)
    {
        _renderer.flipX = isLeft;
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
        _animator.SetInteger(AnimatorItemParam, 0);
        if (type == ItemType.Light)
        {
            _lighterOnHand.gameObject.SetActive(true);
            _animator.SetInteger(AnimatorItemParam, 1);
        }
        else if (type == ItemType.Sword)
        {
            _animator.SetInteger(AnimatorItemParam, 2);
        }
        else if(type == ItemType.Key)
        {
            _animator.SetInteger(AnimatorItemParam, 3);
        }
    }

    public void OnItemDropped(ItemType type)
    {
        _animator.SetInteger(AnimatorItemParam, 0);
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
        _player.LookDirectionChanged += OnLookDirectionChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.MoveInput -= OnMoveInputChanged;
        _player.LookDirectionChanged -= OnLookDirectionChanged;
    }

    private void OnMoveInputChanged(Vector2 moveInput)
    {       
        _movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
    }

    private void OnLookDirectionChanged(LookDirection direction)
    {
        if (direction == LookDirection.Left)
            ToggleRotation(true);
        else if (direction == LookDirection.Right)
            ToggleRotation(false);
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }

    public void Reset()
    {
        _animator.SetBool(AnimatorWalkParam, false);
    }
}
