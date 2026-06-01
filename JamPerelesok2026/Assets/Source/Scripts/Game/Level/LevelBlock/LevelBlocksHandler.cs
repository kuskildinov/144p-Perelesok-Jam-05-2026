using UnityEngine;

public class LevelBlocksHandler : MonoBehaviour
{
    private LevelRoot _root;
    private LevelBlock _currentActiveBlock;
    private PlayerInputHandler _inputHandler;

    private bool _isRotating;

    public void Initialize(LevelRoot root, PlayerInputHandler inputHandler)
    {
        _root = root;
        _inputHandler = inputHandler;

        InitializeLevelBlocks();

        SubscribeToEvents();
    }

    private void InitializeLevelBlocks()
    {
        LevelBlock[] blocks = FindObjectsByType<LevelBlock>();

        foreach (LevelBlock block in blocks)
        {
            block.Initialize(this);
        }
    }

    public void SetCurrentBlock(LevelBlock block)
    {
        if (_currentActiveBlock != null)
            _currentActiveBlock.ToggleOutline(false,false);

        _currentActiveBlock = block;
        _currentActiveBlock.ToggleOutline(false,true);
    }

    private void TryRotateCurrentBlock(bool onLeft)
    {
        if (_currentActiveBlock == null || _isRotating)
            return;

        _isRotating = true;

        _currentActiveBlock.RotateBlock(onLeft, () =>
        {
            _isRotating = false;
        });
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.RightRotateInput += OnRightRotateInputChanged;
        _inputHandler.LeftRotateInput += OnLeftRotateInputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.RightRotateInput -= OnRightRotateInputChanged;
        _inputHandler.LeftRotateInput -= OnLeftRotateInputChanged;
    }

    private void OnRightRotateInputChanged()
    {
        TryRotateCurrentBlock(false);
    }

    private void OnLeftRotateInputChanged()
    {
        TryRotateCurrentBlock(true);
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }

    public void Reset()
    {
        if (_currentActiveBlock != null)
        {
            _currentActiveBlock.ToggleOutline(true,false);
            _currentActiveBlock.ToggleOutline(false, false);
        }
           
        _currentActiveBlock = null;
    }
}
