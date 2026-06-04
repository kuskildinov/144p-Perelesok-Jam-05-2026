using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private string _nextLevelScene;
    [SerializeField] private List<DialogPhrase> _startDialogs;  
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _pausePanel;

    private BlackFade _blackFade;

    private LevelBlocksHandler _blocksHandler;
    private LevelTrapsHandler _trapsHandler;
    private LevelInteractablesHandler _interactablesHandler;
    private PlayerRoot _playeRoot;
    private PlayerInputHandler _inputHandler;
    private StartDialogPanel _startDialogPanel;
    private CommentsDialogPanel _commentsDialogPanel;

    private bool _isGamePaused = false;

    public override void Compose()
    {
        _playeRoot = FindAnyObjectByType<PlayerRoot>();
        _inputHandler = _playeRoot.InputHandler;
        _blackFade = FindAnyObjectByType<BlackFade>();
        Time.timeScale = 1f;
        _isGamePaused = false;

        InitializeLevelBlocksHandler();
        InitializeLevelTrapsHandler();
        InitializeLevelInteractablesHandler();
        InitializeStartDialogPanel();
        InitializeCommentsDialogPanel();
        SaveLevelIndex();

        TryShowStartPhrase();
        SubscribeToEvents();
    }

    private void StartGame()
    {
        ResumeGame();

        _blackFade.FadeIn();
    }

    public void PauseGame()
    {
        _isGamePaused = true;
        OpenPausePanel();
        Time.timeScale = 0f;
        _playeRoot.SetCursorActive();
    }
    
    public void ResumeGame()
    {
        _isGamePaused = false;
        ClosePausePanel();
        Time.timeScale = 1f;
        _playeRoot.ToggleActivation(true);
        _playeRoot.SetCursorNotActive();
    }

    private void SaveLevelIndex()
    {
        GlobalVars.CurrentStartedSceneIndex = _levelIndex;
        PlayerPrefs.SetInt(GlobalVars.CurrentStartedSceneSaveKey, _levelIndex);
    }
       
    #region >>> PAUSE PANEL

    private void OpenPausePanel()
    {
        _pausePanel.gameObject.SetActive(true);
    }

    private void ClosePausePanel()
    {
        _pausePanel.gameObject.SetActive(false);
    }

    private void OnPauseButtonClicked()
    {
        if (_isGamePaused)
            ResumeGame();
        else
            PauseGame();
    }

    #endregion
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

    public void TryShowCommentDialog(DialogPhrase phrase)
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
    #region >>> INTERACTABLES

    private void InitializeLevelInteractablesHandler()
    {
        _interactablesHandler = FindAnyObjectByType<LevelInteractablesHandler>();
        if (_interactablesHandler == null)
        {
            Debug.LogError("Error: Cant find LevelInteractablesHandler on scene!");
            return;
        }

        _interactablesHandler.Initialize(this);
    }

    #endregion
    #region >>> LEAVE LEVEL

    public void TryLeaveLevel()
    {
        LoadScene(_nextLevelScene);
    }

    public void TryReloadLevel()
    {      
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadScene(string name)
    {
        Time.timeScale = 1f;
        _blackFade.FadeOut(-1, () =>
        {
            SceneManager.LoadScene(name);
        });
    }

    public void LoadMainMenuScene()
    {
        LoadScene(GlobalVars.MainMenuSceneName);
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.PauseInput += OnPauseButtonClicked;
    }

    private void UnSubscribeToEvents()
    {
        _inputHandler.PauseInput -= OnPauseButtonClicked;
    }

    #endregion

    public void OnPlayerModeChanged(PlayerMode newMode)
    {
        _blocksHandler.Reset();
    }

    public void OnPlayerDead()
    {
        StartCoroutine(PlayerDeadRoutine());
    }    

    private IEnumerator PlayerDeadRoutine()
    {
        yield return new WaitForSecondsRealtime(3f);

        _playeRoot.SetCursorActive();
        _losePanel.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}
