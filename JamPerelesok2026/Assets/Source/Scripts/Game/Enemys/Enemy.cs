using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private const string AnimatorWalkParam = "Walk";
    private const string AnimatorAttackTrigger = "Attack";
    private const string AnimatorDeadParam = "Dead";

    [SerializeField] private SpriteRenderer _renderer;
    [Header("Movement")]
    [SerializeField] private float _speed = 3.5f;

    [Header("Detection")]
    [SerializeField] private CharacterIllumination _illumanation;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private float _eyeHeight = 1.5f;
    [SerializeField] private float _loseTargetDelay = 2f;
    [SerializeField] private LayerMask _visionMask;

    [Header("Attack")]
    [SerializeField] private float _attackDistance = 0.1f;
    [SerializeField] private float _attackDuration = 1f;
    [SerializeField] private float _timeBeforeAttack = 3f;
    [SerializeField] private AttackZone _topAttackZone;
    [SerializeField] private AttackZone _bottomAttackZone;
    [SerializeField] private AttackZone _leftAttackZone;
    [SerializeField] private AttackZone _rightAttackZone;

    [Header("Dead")]
    [SerializeField] private float _deadDestroyDeley = 2f;

    private EnemysRoot _root;
    private NavMeshAgent _agent;
    private Player _player;
    private Transform _target;
    private Animator _animator;

    private Coroutine _loseTargetCoroutine;
    private Coroutine _attackCoroutine;

    protected bool _isActive;
    protected bool _isAlive;
    private bool _isAttacking;

    private AttackZone _curentAttackZone;
    private Coroutine _attackRoutine;

    protected EnemyState _currentState;
    protected LightAura _currentDetectedLight;

    public bool IsActive => _isActive;
    public bool IsAlive => _isAlive;
    public NavMeshAgent Agent => _agent;

    public void Initialzie(EnemysRoot root)
    {
        _root = root;

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        if(_agent != null && _agent.enabled)
        {
            _agent.speed = _speed;
            _agent.isStopped = true;
        }        

        _player = _root.TryGetPlayer();

        _isActive = false;
        _isAlive = true;
        
        _currentState = EnemyState.Idle;

        SubscribeToEvents();
    }

    private void Update()
    {
        if (!_isAlive)
            return;
        
        if(_isActive)
            CheckAttackRange();
        WalkHandler();
        RotateByPlayerPosition();
    }

    #region >>> ACTIVATION

    public void Activate(LightAura light)
    {
        if (!_isAlive)
            return;

        if (!_illumanation.IsInDarkness)
            _isActive = true;
        else
            _isActive = false;

        if(light != null)
            _currentDetectedLight = light;
    }

    public void Deactivate()
    {
        _isActive = false;
        SetIdleState();
    }

    #endregion   
    #region >>> STATES

    private void SetIdleState()
    {
        if (_currentState == EnemyState.Idle)
            return;

        _currentState = EnemyState.Idle;

        if (_agent != null && _agent.enabled)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        PlayIdleAnimation();
    }

    private void LosePlayer()
    {
        if (_loseTargetCoroutine != null)
            return;

        _loseTargetCoroutine = StartCoroutine(LosePlayerRoutine());
    }

    private void SetWalkState()
    {
        if (_isAttacking)
            return;

        if (_loseTargetCoroutine != null)
        {
            StopCoroutine(_loseTargetCoroutine);
            _loseTargetCoroutine = null;
        }

        _currentState = EnemyState.Walk;

        if (_agent != null && _agent.enabled)
        {
            _target = _player.transform;
            _agent.isStopped = false;
        }

        PlayWalkAnimation();
    }

    private void SetAttackState()
    {
        Debug.Log("ATTACK");
        _currentState = EnemyState.Attack;

        PlayAttackAnimation();
    }

    private void OnDead()
    {
        _isAlive = false;
        _currentState = EnemyState.Dead;

        if (_agent != null && _agent.enabled)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        PlayDeadAnimation();
        StartCoroutine(DeadDestroyRoutine());
    }

    private IEnumerator LosePlayerRoutine()
    {
        yield return new WaitForSeconds(_loseTargetDelay);

        SetIdleState();

        _loseTargetCoroutine = null;
    }

    private IEnumerator DeadDestroyRoutine()
    {
        yield return new WaitForSecondsRealtime(_deadDestroyDeley);

        this.gameObject.SetActive(false);
    }

    #endregion
    #region >>> MOVEMENT

    private void WalkHandler()
    {
        if (_currentState != EnemyState.Walk || _isAttacking)
            return;

        if (_target == null)
            return;

        if (_agent != null && _agent.enabled)
        {
            if (!_agent.hasPath ||
            Vector3.Distance(_agent.destination, _target.position) > 0.5f)
            {
                _agent.SetDestination(_target.position);
            }
        }
    }

    #endregion
    #region >>> ROTATION

    private void RotateByPlayerPosition()
    {
        if (!_isAlive || _isAttacking)
            return;

        LookDirection dir = GetDirectionToPlayer();
        if (dir == LookDirection.Left)
            ToggleRotation(false);
        else if (dir == LookDirection.Right)
            ToggleRotation(true);
    }

    private void ToggleRotation(bool isLeft)
    {
        _renderer.flipX = isLeft;
    }

    #endregion
    #region >>> ATTACK

    private void CheckAttackRange()
    {
        if (!_isActive || _isAttacking)
            return;

        float distance = Vector3.Distance(
            transform.position,
            _player.transform.position);

        if (distance <= _attackDistance)
        {            
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
        else
        {           
            SetWalkState();
        }
    }

    private LookDirection GetDirectionToPlayer()
    {
        Vector3 dir = _player.transform.position - transform.position;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            return dir.x > 0
                ? LookDirection.Right
                : LookDirection.Left;
        }

        return dir.z > 0
            ? LookDirection.Top
            : LookDirection.Down;
    }
   
    private IEnumerator AttackRoutine()
    {
        SetIdleState();
        _isAttacking = true;
        if (_agent != null && _agent.enabled)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(_timeBeforeAttack);
        if (!_isAlive)
            yield break;

        SetAttackState();
        LookDirection dir = GetDirectionToPlayer();
        ActivateAttackZoneByDirection(dir);
               
        yield return new WaitForSeconds(_attackDuration);
              
        if (_curentAttackZone != null)
            _curentAttackZone.gameObject.SetActive(false);

        _isAttacking = false;        
    }

    private void ActivateAttackZoneByDirection(LookDirection direction)
    {
        if (_curentAttackZone != null)
        {
            _curentAttackZone.gameObject.SetActive(false);
            _curentAttackZone = null;
        }

        switch (direction)
        {
            case LookDirection.Top:
                {
                    _curentAttackZone = _topAttackZone;
                    break;
                }
            case LookDirection.Down:
                {
                    _curentAttackZone = _bottomAttackZone;
                    break;
                }
            case LookDirection.Right:
                {
                    _curentAttackZone = _rightAttackZone;
                    break;
                }
            case LookDirection.Left:
                {
                    _curentAttackZone = _leftAttackZone;
                    break;
                }
        }

        if (_attackRoutine != null)
            StopCoroutine(ActivateAttackZoneRoutine(_curentAttackZone));

        _attackRoutine = StartCoroutine(ActivateAttackZoneRoutine(_curentAttackZone));
    }

    private IEnumerator ActivateAttackZoneRoutine(AttackZone zone)
    {       
        zone.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(_attackDuration);
        zone.gameObject.SetActive(false);      
    }

    #endregion
    #region >>> VISUAL

    private void PlayIdleAnimation()
    {
        _animator.SetBool(AnimatorWalkParam, false);
    }

    private void PlayWalkAnimation()
    {
        _animator.SetBool(AnimatorWalkParam, true);
    }

    private void PlayAttackAnimation()
    {
        _animator.SetTrigger(AnimatorAttackTrigger);
    }

    private void PlayDeadAnimation()
    {
        _animator.SetBool(AnimatorDeadParam, true);
    }

    #endregion
    #region >>> IN DARKNESS BEHAVIOUR

    private void OnEnterDarkness()
    {
        Debug.Log("Enter Darkness");
        _isActive = false;
        LosePlayer();
    }

    private void OnExitDarkness()
    {
        Debug.Log("Exit Darkness");
        _isActive = true;
        SetWalkState();
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _illumanation.EnteredDarkness += OnEnterDarkness;
        _illumanation.ExitedDarkness += OnExitDarkness;
    }

    private void UnsubscribeToEvents()
    {
        _illumanation.EnteredDarkness -= OnEnterDarkness;
        _illumanation.ExitedDarkness -= OnExitDarkness;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<LevelBlock>(out LevelBlock block))
        {
            block.AddEnemy(this);          
        }

        if(other.gameObject.TryGetComponent<AttackZone>(out AttackZone zone))
        {
            if(zone.Type == AttackZoneType.Player)
            {
                OnDead();
            }

            if (zone.Type == AttackZoneType.Trap)
            {
                if (zone.TryGetComponent<TrapBlock>(out TrapBlock trap))
                {
                    if (trap.IsActive)
                    {
                        zone.Activate();
                        OnDead();
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<LevelBlock>(out LevelBlock block))
        {
            block.RemoveEnemy(this);
            Debug.Log("Exit from Block");
        }
    }
   
    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,_attackDistance);
    }
}

public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Dead
}