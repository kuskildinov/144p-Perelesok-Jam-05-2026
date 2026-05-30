using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    private EnemysRoot _root;
    private NavMeshAgent _agent;
    protected bool _isActive = false;
    protected bool _isAlive = true;
    protected EnemyState _currentState;
    protected Lighter _currentDetectedLighter;
    private Player _player;

    public bool IsActive => _isActive;
    public bool IsAlive => _isAlive;

    public void Initialzie(EnemysRoot root)
    {
        _agent = GetComponent<NavMeshAgent>();

        _root = root;
        _agent.speed = _speed;
        _isActive = false;
        _isAlive = true;
        _currentState = EnemyState.Idle;
        _player = _root.TryGetPlayer();
    }
  
    private void Update()
    {
        if (!_isAlive)
            return;

        WalkHandler();
    }

    #region >>> ACTIVATION

    public void Activate(Lighter lighter)
    {
        _isActive = true;
        _currentDetectedLighter = lighter;
    }

    public void Deactivate()
    {
        _isActive = false;      
    }

    #endregion
    #region >>> WALK

    private void WalkHandler()
    {
        if (_currentState != EnemyState.Walk)
            return;

        if (_target != null)
        {
            _agent.speed = _speed;
            _agent.SetDestination(_target.position);
        }
        else
        {
            _agent.speed = 0;
        }
    }

    #endregion
}

public enum EnemyState
{
    Idle,
    Walk,
    Attack,
}
