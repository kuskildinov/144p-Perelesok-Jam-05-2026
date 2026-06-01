using UnityEngine;

public class LevelRoot : CompositeRoot
{
    private LevelBlocksHandler _blocksHandler;
    private LevelTrapsHandler _trapsHandler;
    private PlayerInputHandler _inputHandler;

    public override void Compose()
    {
        _inputHandler = FindAnyObjectByType<PlayerInputHandler>();

        InitializeLevelBlocksHandler();
        InitializeLevelTrapsHandler();

    }

    #region >>> LEVEL BLOCKS

    private void InitializeLevelBlocksHandler()
    {
        _blocksHandler = FindAnyObjectByType<LevelBlocksHandler>();
        if (_blocksHandler == null)
        {
            Debug.LogError("Error: Cant find LevelBlocksHandler on scene!");
            return;
        }

        _blocksHandler.Initialize(this, _inputHandler);
    }

    #endregion
    #region >>> LEVEL TRAPS

    private void InitializeLevelTrapsHandler()
    {
        _trapsHandler = FindAnyObjectByType<LevelTrapsHandler>();
        if (_trapsHandler == null)
        {
            Debug.LogError("Error: Cant find LevelTrapsHandler on scene!");
            return;
        }

        _trapsHandler.Initialize(this);
    }

    #endregion

    public void OnPlayerModeChanged(PlayerMode newMode)
    {
        _blocksHandler.Reset();
    }
}
