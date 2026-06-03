using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitGameButton;

    private MainMenuRoot _root;

    public void Initialzie(MainMenuRoot root)
    {
        _root = root;

        UpdateContinueButton();
        SubscribeToEvents();
    }

    private void UpdateContinueButton()
    {
        if (_root.CheckHasSavedData())
            _continueButton.interactable = true;
        else
            _continueButton.interactable = false;
    }

    #region >>> ENENTS

    private void SubscribeToEvents()
    {
        _continueButton.onClick.AddListener(OnContinueGameButtonClicked);
        _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        _exitGameButton.onClick.AddListener(OnExitGameButtonClicked);
    }

    private void UnSubscribeToEvents()
    {
        _continueButton.onClick.RemoveAllListeners();
        _newGameButton.onClick.RemoveAllListeners();
        _exitGameButton.onClick.RemoveAllListeners();
    }

    private void OnNewGameButtonClicked()
    {
        _root.OnNewGameButtonClicked();
    }

    private void OnContinueGameButtonClicked()
    {
        _root.OnContinueGameButtonClicked();
    }

    private void OnExitGameButtonClicked()
    {
        _root.OnExitGameButtonClicked();
    }

    #endregion

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}
