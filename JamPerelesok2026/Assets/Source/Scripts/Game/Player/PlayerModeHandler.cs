using UnityEngine;

public class PlayerModeHandler : MonoBehaviour
{    
    private PlayerRoot _root;
    private PlayerInputHandler _inputHandler;
    private PlayerMode _currentPlayerMode;

    public PlayerMode CurrentPlayerMode => _currentPlayerMode;
   
    public void Initialize(PlayerRoot root)
    {
        _root = root;
        _inputHandler = _root.InputHandler;
        _currentPlayerMode = PlayerMode.Character;

        SubscribeToEvents();
    }

    private void TryChangeState()
    {
        if (!_root.Player.IsAlive)
            return;

        if (_currentPlayerMode == PlayerMode.Character)
            _currentPlayerMode = PlayerMode.Player;
        else if (_currentPlayerMode == PlayerMode.Player)
            _currentPlayerMode = PlayerMode.Character;

        _root.OnPlayerModeChanged(_currentPlayerMode);
    }

    public void EmergancyChangeState()
    {
        TryChangeState();
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.ChangeStateInput += OnChangeStateinputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.ChangeStateInput -= OnChangeStateinputChanged;
    }

    private void OnChangeStateinputChanged()
    {
        TryChangeState();
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}

public enum PlayerMode
{
    Character,
    Player
}
