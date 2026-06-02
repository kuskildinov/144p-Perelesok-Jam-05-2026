using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    private Player _player;
    private PlayerModeHandler _playerModeHandler;
    private PlayerInputHandler _inputHandler;
    private PlayerCameraHandler _cameraHandler;
    private PlayerUI _playerUI;
    private LevelRoot _levelRoot;

    public Player Player => _player;
    public PlayerInputHandler InputHandler => _inputHandler;
    public PlayerMode CurrentPlayerMode => _playerModeHandler.CurrentPlayerMode;

    public override void Compose()
    {
        InitializeInput();
        InitializePlayer();
        InitializePlayerModeHandler();
        InitializePlayerCameraHandler();
        InitializePlayerUI();

        _levelRoot = FindAnyObjectByType<LevelRoot>();

        ChangeMouseVisibility(PlayerMode.Character);
    }

    #region >>> INPUT
    private void InitializeInput()
    {
        _inputHandler = FindAnyObjectByType<PlayerInputHandler>();
        if(_inputHandler == null)
        {
            Debug.LogError("Error: Cant find InputHandler on scene!");
            return;
        }
    }
    #endregion
    #region >>> PLAYER

    private void InitializePlayer()
    {
        _player = FindAnyObjectByType<Player>();
        if (_player == null)
        {
            Debug.LogError("Error: Cant find Player on scene!");
            return;
        }
        _player.Initialzie(this);
        ToggleActivation(false);
    }

    public void OnPlayerTakeDamage()
    {
        EmergancyChangeState();
        _playerUI.UpdateHeartCount();
    }

    public void ToggleActivation(bool value)
    {
        _player.ToggleActivation(value);
    }

    #endregion
    #region >>> PLAYER UI

    private void InitializePlayerUI()
    {
        _playerUI = FindAnyObjectByType<PlayerUI>();
        if (_playerUI == null)
        {
            Debug.LogError("Error: Cant find PlayerUI on scene!");
            return;
        }
        _playerUI.Initialize(this);
    }

    #endregion
    #region >>> MODE HANDLER

    public void InitializePlayerModeHandler()
    {
        _playerModeHandler = FindAnyObjectByType<PlayerModeHandler>();
        if (_playerModeHandler == null)
        {
            Debug.LogError("Error: Cant find PlayerModeHandler on scene!");
            return;
        }
        _playerModeHandler.Initialize(this);
    }

    public void OnPlayerModeChanged(PlayerMode newMode)
    {
        _player.OnPlayerModeChanged(newMode);
        _cameraHandler.OnPlayerModeChanged(newMode);
        _levelRoot.OnPlayerModeChanged(newMode);

        ChangeMouseVisibility(newMode);
    }

    public void EmergancyChangeState()
    {
        if(_playerModeHandler.CurrentPlayerMode == PlayerMode.Player)
            _playerModeHandler.EmergancyChangeState();
    }

    #endregion
    #region >>> CAMERA HANDLER

    public void InitializePlayerCameraHandler()
    {
        _cameraHandler = FindAnyObjectByType<PlayerCameraHandler>();
        if (_cameraHandler == null)
        {
            Debug.LogError("Error: Cant find PlayerModeHandler on scene!");
            return;
        }
        _cameraHandler.Initialize();
    }

    #endregion
    #region >>> MOUSE

    private void ChangeMouseVisibility(PlayerMode newMode)
    {
        if(newMode == PlayerMode.Character)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (newMode == PlayerMode.Player)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    #endregion
}
