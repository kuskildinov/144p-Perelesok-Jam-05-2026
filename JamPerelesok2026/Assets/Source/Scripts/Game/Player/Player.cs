using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovment _playerMovment;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerInteractions _playerInteractions;
    [SerializeField] private PlayerAttack _playerAttack;
    [Header("Darkness effects")]
    [SerializeField] private CharacterIllumination _illumanation;
    [SerializeField] private ParticleSystem _darkEffect;
    [SerializeField] private float _timeToDamageInDarkness;
   
    private CharacterController _characterController;
    private Rigidbody _rb;
    private bool _isActive = true;
    private bool _isAlive = true;  
    private bool _isCanTakeDamage = true;
    private bool _inDarkness = false;
    private PlayerRoot _root;
    private PlayerInputHandler _inputHandler;
    private LookDirection _currentLookDirection;
    private float _darknessTimer;

    public bool IsActive => _isActive;
    public bool IsAlive => _isAlive;
    public bool IsCanTakeDamage { get => _isCanTakeDamage; set => _isCanTakeDamage = value; }
    public CharacterController Controller => _characterController;
    public LookDirection CurrentLookDirection => _currentLookDirection;
    public Item CurrentTakedItem => _playerInteractions.CurrentTakedItem;
    public int CurrentHealth => _playerHealth.CurrentHealth;

    public event Action<LookDirection> LookDirectionChanged;

    public void Initialzie(PlayerRoot root)
    {
        _root = root;
        _inputHandler = _root.InputHandler;
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
       
        _playerMovment.Initialize(this, _inputHandler);
        _playerVisual.Initialize(this, _inputHandler);
        _playerInteractions.Initialize(this, _inputHandler);
        _playerAttack.Initialize(this, _inputHandler);
        _playerHealth.Initialize(this);
        _darkEffect.Stop();

        SubscribeToEvents();
    }

    private void Update()
    {
        if (!_isAlive)
            return;

        if(_inDarkness)
        {
            _darknessTimer += Time.deltaTime;

            if(_darknessTimer >= _timeToDamageInDarkness)
            {
                TakeDamage(transform);
                _darknessTimer = 0f;
            }
        }
    }

    #region >>> MODE

    public void OnPlayerModeChanged(PlayerMode mode)
    {
        if(mode == PlayerMode.Character)
        {
            ToggleActivation(true);
        }
        else if(mode == PlayerMode.Player)
        {
            ToggleActivation(false);
        }
    }

    #endregion
    #region >>> ACTIVATION

    public void ToggleActivation(bool value)
    {
        _isActive = value;
    }

    #endregion
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

        // Îïðåäåëÿåì ãëàâíóþ îñü
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Ãîðèçîíòàëü
            _currentLookDirection =
                input.x > 0
                    ? LookDirection.Right
                    : LookDirection.Left;
        }
        else
        {
            // Âåðòèêàëü
            _currentLookDirection =
                input.y > 0
                    ? LookDirection.Top
                    : LookDirection.Down;
        }

        LookDirectionChanged?.Invoke(_currentLookDirection);
    }

    #endregion
    #region >>> TAKE DAMAGE

    public void TakeDamage(Transform damageSource)
    {
        if (!_isAlive)
            return;

        _playerMovment.KnockBack(damageSource);
        _playerHealth.OnDamageTaked();
        _root.OnPlayerTakeDamage();
    }

    public void OnPlayerDead()
    {
        _isAlive = false;
    }

    #endregion
    #region >>> IN DARKNESS BEHAVIOUR

    private void OnEnterDarkness()
    {
        Debug.Log("Â ÒÅÌÍÎÒÅ");
        _darkEffect.Play();
        _inDarkness = true;
    }

    private void OnExitDarkness()
    {
        Debug.Log("ÂÛØÅË ÈÇ ÒÅÌÍÎÒÛÅ!");
        _darkEffect.Stop();
        _inDarkness = false;
        _darknessTimer = 0f;
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.MoveInput += OnMoveInputChanged;
        _illumanation.EnteredDarkness += OnEnterDarkness;
        _illumanation.ExitedDarkness += OnExitDarkness;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.MoveInput -= OnMoveInputChanged;
        _illumanation.EnteredDarkness -= OnEnterDarkness;
        _illumanation.ExitedDarkness -= OnExitDarkness;
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
