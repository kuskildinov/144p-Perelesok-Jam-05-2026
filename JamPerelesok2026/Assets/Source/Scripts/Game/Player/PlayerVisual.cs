using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string AnimatorWalkParam = "Walk";
    private const string AnimatorItemParam = "Item";
    private const string AnimatorAttackTrigger = "Attack";
    private const string AnimatorTakeDamageTrigger = "TakeDamage";
    private const string AnimatorDeadParam = "Dead";

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _lighterOnHand;
    [SerializeField] private Light _light;

    private Player _player;
    private PlayerInputHandler _inputHandler;  
    private Vector3 _movement;

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

        if(!_player.IsActive)
        {
            PlayIdleAnimation();
        }

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
            PlayWalkAnimation();
        }
        else
        {
            PlayIdleAnimation();
        }
    }

    private void PlayWalkAnimation()
    {
        _animator.SetBool(AnimatorWalkParam, true);
    }

    private void PlayIdleAnimation()
    {
        _animator.SetBool(AnimatorWalkParam, false);
    }
    private void PlayAttackAnimation()
    {
        _animator.SetTrigger(AnimatorAttackTrigger);
    }

    private void PlayTakeDamageAnimation()
    {
        _animator.SetTrigger(AnimatorTakeDamageTrigger);
    }

    public void PlayDeadAnimation()
    {
        _animator.SetBool(AnimatorDeadParam, true);
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
        _inputHandler.AttackInput += OnAttackInputChanged;
        _player.LookDirectionChanged += OnLookDirectionChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.MoveInput -= OnMoveInputChanged;
        _inputHandler.AttackInput -= OnAttackInputChanged;
        _player.LookDirectionChanged -= OnLookDirectionChanged;
    }

    private void OnMoveInputChanged(Vector2 moveInput)
    {       
        _movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
    }

    private void OnLookDirectionChanged(LookDirection direction)
    {
        if (!_player.IsAlive)
            return;

        if (direction == LookDirection.Left)
            ToggleRotation(true);
        else if (direction == LookDirection.Right)
            ToggleRotation(false);
    }

    private void OnAttackInputChanged()
    {
        if (_player == null)
            return;

        if(_player.CurrentTakedItem != null && _player.CurrentTakedItem.Type == ItemType.Sword)
            PlayAttackAnimation();
    }

    public void OnTakeDamage()
    {        
        PlayTakeDamageAnimation();
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
