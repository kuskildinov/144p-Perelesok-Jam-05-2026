using UnityEngine;

public class CommentsDialogPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextWriter _textWriter;

    private LevelRoot _root;

    public void Initialize(LevelRoot root, PlayerInputHandler inputHandler)
    {
        _root = root;
        SubnscribeToEvents();
    }

    #region >>> OPEN CLOSE

    public void Open(DialogPhrase phrase)
    {
        _panel.gameObject.SetActive(true);

        _textWriter.ChangeTextAndTyping(phrase);
    }

    public void Close()
    {
        _panel.gameObject.SetActive(false);
    }

    #endregion

    #region >>> EVENTS

    private void SubnscribeToEvents()
    {
        _textWriter.TypingComplete += OnTypingComplete;
    }

    private void UnsubscriteToEvents()
    {
        _textWriter.TypingComplete += OnTypingComplete;
    }

    private void OnTypingComplete()
    {
        _root.OnCommentDialogComplete();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscriteToEvents();
    }
}
