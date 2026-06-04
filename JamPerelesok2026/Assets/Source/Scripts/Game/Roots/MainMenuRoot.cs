using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuRoot : CompositeRoot
{
    [SerializeField] private GameObject _confirmExitPanel;
    [SerializeField] private BlackFade _blackFade;

    private MainMenuPanel _mainMenuPanel;

    public override void Compose()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        InitializeMainMenuPanel();
    }

    #region >>> MAIN MENU PANEL

    private void InitializeMainMenuPanel()
    {
        _mainMenuPanel = FindAnyObjectByType<MainMenuPanel>();

        if (_mainMenuPanel == null)
        {
            Debug.LogError("Error: Cant find MainMenuPanel on scene!");
            return;
        }
        _mainMenuPanel.Initialzie(this);
    }

    public bool CheckHasSavedData()
    {
        if (PlayerPrefs.HasKey(GlobalVars.CurrentStartedSceneSaveKey))
        {
            GlobalVars.CurrentStartedSceneIndex = PlayerPrefs.GetInt(GlobalVars.CurrentStartedSceneSaveKey);
            return true;
        }
        return false;
    }

    public void OnNewGameButtonClicked()
    {
        LoadScene(GlobalVars.Level_0_Name);
    }

    public void OnContinueGameButtonClicked()
    {
        int currentSavedLevel = GlobalVars.CurrentStartedSceneIndex;
        if (currentSavedLevel == 0)
            OnNewGameButtonClicked();
        else if(currentSavedLevel == 1)
            LoadScene(GlobalVars.Level_1_Name);
        else if (currentSavedLevel == 2)
            LoadScene(GlobalVars.Level_2_Name);
        else if (currentSavedLevel == 3)
            LoadScene(GlobalVars.Level_3_Name);
        else if (currentSavedLevel == 4)
            LoadScene(GlobalVars.Level_4_Name);
    }

    public void OnExitGameButtonClicked()
    {
        _confirmExitPanel.gameObject.SetActive(true);
    }

    #endregion

    private void LoadScene(string name)
    {
        _blackFade.FadeOut(-1, () =>
        {
            SceneManager.LoadScene(name);
        });      
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
