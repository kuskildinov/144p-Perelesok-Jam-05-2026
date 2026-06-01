using UnityEngine;

public class LevelRoot : CompositeRoot
{
    private LevelBlocksHandler _blocksHandler;   
    private PlayerInputHandler _inputHandler;

    public override void Compose()
    {
        _inputHandler = FindAnyObjectByType<PlayerInputHandler>();

        InitializeLevelBlocksHandler();
        
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

    public void OnPlayerModeChanged(PlayerMode newMode)
    {
        _blocksHandler.Reset();
    }
}
