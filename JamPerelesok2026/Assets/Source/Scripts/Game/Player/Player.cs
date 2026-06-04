using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovment _playerMovment;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerInteractions _playerInteractions;
    [SerializeField] private PlayerAttack _playerAttack;
    [Header("Sounds")]
    [SerializeField] private AudioSource _commonSource;
    [SerializeField] private AudioSource _walkSource;
    [SerializeField] private AudioSource _darkSource;
    [Header("SoundsClips")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _takeDamageSound;
    [SerializeField] private AudioClip _dropItemSound;
    [SerializeField] private AudioClip _takeItemSound;
    [SerializeField] private AudioClip _changeModeSound;
    [SerializeField] private AudioClip _deadSound;
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
    private Coroutine _inDarknessCoroutine;

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

        ToggleActivation(true);

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

        PlayChangeModeSound();
        _playerVisual.Reset();
    }

    #endregion
    #region >>> ACTIVATION

    public void ToggleActivation(bool value)
    {
        if (_root.CurrentPlayerMode == PlayerMode.Player)
            _isActive = false;
        else
            _isActive = value;

        ToggleWalkSound(false);
    }

    #endregion
    #region >>> ITEMS

    public Item GetCurrentTakedItem()
    {
        return _playerInteractions.CurrentTakedItem;
    }

    public void OnItemTaked(ItemType type)
    {
        PlayTakeItemSound();
        _playerVisual.OnItemTaked(type);
    }

    public void OnItemDropped(ItemType type)
    {
        PlayDropItemSound();
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

        // Определяем главную ось
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Горизонталь
            _currentLookDirection =
                input.x > 0
                    ? LookDirection.Right
                    : LookDirection.Left;
        }
        else
        {
            // Вертикаль
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

        PlayTakeDamageSound();
        _playerMovment.KnockBack(damageSource);
        _playerHealth.OnTakeDamage();
        _playerVisual.OnTakeDamage();
        _root.OnPlayerTakeDamage();
    }

    public void OnPlayerDead()
    {
        _playerInteractions.DropItem();

        PlayDeadSound();

         _isAlive = false;
        _isActive = false;      
        _playerVisual.PlayDeadAnimation();
        _root.OnPlayerDead();
    }

    #endregion
    #region >>> IN DARKNESS BEHAVIOUR

    private void OnEnterDarkness()
    {
        if (!_isActive)
            return;

        ToggleDarkSounds(true);
        _darkEffect.Play();
        _inDarkness = true;
    }

    private void OnExitDarkness()
    {
        if (!_isActive)
            return;

        ToggleDarkSounds(false);
        _darkEffect.Stop();
        _inDarkness = false;
        _darknessTimer = 0f;
    }

    #endregion
    #region >>> SOUNDS

    public void ToggleWalkSound(bool value)
    {       
        if (!_walkSource.isPlaying && value)
            _walkSource.Play();
        else if(_walkSource.isPlaying && !value)
            _walkSource.Pause();
    }

    public void ToggleDarkSounds(bool value)
    {
        if (_inDarknessCoroutine != null)
            StopCoroutine(_inDarknessCoroutine);

        if (value && !_darkSource.isPlaying)
            _darkSource.Play();

        _inDarknessCoroutine = StartCoroutine(FadeVolume(value ? 1f : 0f, 1f));
    }

    private IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = _darkSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _darkSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        _darkSource.volume = targetVolume;

        if (!_darkSource.isPlaying && targetVolume == 0f)
            _darkSource.Pause();

        _inDarknessCoroutine = null;
    }

    public void PlayAttackSound()
    {
        _commonSource.PlayOneShot(_attackSound);
    }

    public void PlayTakeDamageSound()
    {
        _commonSource.PlayOneShot(_takeDamageSound);
    }
    
    public void PlayDropItemSound()
    {
        _commonSource.PlayOneShot(_dropItemSound);
    }

    public void PlayTakeItemSound()
    {
        _commonSource.PlayOneShot(_takeItemSound);        
    }

    public void PlayChangeModeSound()
    {
        _commonSource.PlayOneShot(_changeModeSound);
    }

    public void PlayDeadSound()
    {
        _commonSource.PlayOneShot(_deadSound);
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
