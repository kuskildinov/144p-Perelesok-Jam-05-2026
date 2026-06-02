using System;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextWriter _textWriter;

    private LevelRoot _root;
    private List<DialogPhrase> _dialogs = new List<DialogPhrase>();
    private int _currentDialogIndex = 0;

    public void Initialize(LevelRoot root, PlayerInputHandler inputHandler)
    {
        _root = root;
        SubnscribeToEvents();
    }

    #region >>> OPEN CLOSE

    public void Open()
    {
        _panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        _panel.gameObject.SetActive(false);
    }

    #endregion

    public void ShowTextsFromDialog(List<DialogPhrase> dialogs)
    {
        _dialogs = dialogs;

        StartTypingPhrase(_dialogs[_currentDialogIndex]);
    }

    private void StartTypingPhrase(DialogPhrase phrase)
    {
        _textWriter.ChangeTextAndTyping(phrase);
    }

    private void TryShowNextPhrase()
    {
        _currentDialogIndex++;
        if(_currentDialogIndex >= _dialogs.Count)
        {           
            _root.OnPhrasesComplete();
            return;
        }

        StartTypingPhrase(_dialogs[_currentDialogIndex]);
    }

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
        TryShowNextPhrase();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscriteToEvents();
    }
}

[Serializable]
public struct DialogPhrase
{
    [TextArea] public string Phrase;
    public Color Color;
}

