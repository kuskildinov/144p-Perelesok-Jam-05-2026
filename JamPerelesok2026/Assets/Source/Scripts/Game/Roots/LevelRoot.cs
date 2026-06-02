using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private string _nextLevelScene;
    [SerializeField] private List<DialogPhrase> _startDialogs;

    private BlackFade _blackFade;

    private LevelBlocksHandler _blocksHandler;
    private LevelTrapsHandler _trapsHandler;
    private PlayerRoot _playeRoot;
    private PlayerInputHandler _inputHandler;
    private StartDialogPanel _startDialogPanel;
    private CommentsDialogPanel _commentsDialogPanel;

    public override void Compose()
    {
        _playeRoot = FindAnyObjectByType<PlayerRoot>();
        _inputHandler = _playeRoot.InputHandler;
        _blackFade = FindAnyObjectByType<BlackFade>();

        InitializeLevelBlocksHandler();
        InitializeLevelTrapsHandler();
        InitializeStartDialogPanel();
        InitializeCommentsDialogPanel();

        TryShowStartPhrase();
    }

    private void StartGame()
    {
        ResumeGame();

        _blackFade.FadeIn();
        _playeRoot.ToggleActivation(true);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    #region >>> LEVEL START PHRASE

    private void InitializeStartDialogPanel()
    {
        _startDialogPanel = FindAnyObjectByType<StartDialogPanel>();
        if (_startDialogPanel == null)
        {
            Debug.LogError("Error: Cant find StartDialogPanel on scene!");
            return;
        }

        _startDialogPanel.Initialize(this, _inputHandler);
    }

    public void TryShowStartPhrase()
    {
        if (_startDialogs == null || _startDialogs.Count <= 0)
        {
            _startDialogPanel.Close();
            StartGame();
            return;
        }

        _startDialogPanel.Open();
        _startDialogPanel.ShowTextsFromDialog(_startDialogs);
    }

    public void OnPhrasesComplete()
    {
        _startDialogPanel.Close();
        StartGame();
    }

    #endregion
    #region >>> LEVEL COMMENTS DIALOGS

    private void InitializeCommentsDialogPanel()
    {
        _commentsDialogPanel = FindAnyObjectByType<CommentsDialogPanel>();
        if (_commentsDialogPanel == null)
        {
            Debug.LogError("Error: Cant find CommentsDialogPanel on scene!");
            return;
        }

        _commentsDialogPanel.Initialize(this, _inputHandler);
    }

    public void ShowCommentDialog(DialogPhrase phrase)
    {
        _commentsDialogPanel.Open(phrase);
    }

    public void OnCommentDialogComplete()
    {
        _commentsDialogPanel.Close();
    }

    #endregion
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
    #region >>> LEAVE LEVEL

    public void TryLeaveLevel()
    {
        if (_playeRoot.Player.CurrentTakedItem.Type == ItemType.Light)
        {
            StartCoroutine(ChangeSceneRoutine());
        }
        else
        {

        }
    }

    private  IEnumerator ChangeSceneRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);

        _blackFade.FadeOut(-1,() =>
        {
            SceneManager.LoadScene(_nextLevelScene);
        });       
    }

    #endregion

    public void OnPlayerModeChanged(PlayerMode newMode)
    {
        _blocksHandler.Reset();
    }
}
