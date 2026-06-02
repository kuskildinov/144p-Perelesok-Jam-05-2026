using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{   
    [SerializeField] private TMP_Text _text;  
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private float _timeDeleyAfterTyping = 2f;
    [SerializeField] private bool skipOnClick = true;

    private string _fullText;
    private string _currentText = "";
    private Coroutine _typingCoroutine;
    private bool _isTyping = false;

    public bool IsTyping => _isTyping;
    public string FullText => _fullText;

    public event Action TypingStart;
    public event Action TypingComplete;
       
    public void ChangeTextAndTyping(DialogPhrase dialog)
    {
        _fullText = dialog.Phrase;
        _text.color = dialog.Color;      
        StartTyping();
    }

    private void StartTyping()
    {
        if (_isTyping)
            StopTyping();

        ClearText();
        TypingStart?.Invoke();
        _typingCoroutine = StartCoroutine(TypeText());
    }

    public void StopTyping()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _isTyping = false;
        ClearText();
    }

    public void SkipTyping()
    {
        if (!_isTyping) return;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        ShowFullText();
        _isTyping = false;
        
        TypingComplete?.Invoke();
    }

    public void ShowFullText()
    {
        _currentText = _fullText;
        UpdateTextDisplay(_currentText);
    }

    private void ClearText()
    {
        _currentText = "";
        UpdateTextDisplay(_currentText);
    }

    private IEnumerator TypeText()
    {
        _isTyping = true;

        for (int i = 0; i <= _fullText.Length; i++)
        {
            _currentText = _fullText.Substring(0, i);
            UpdateTextDisplay(_currentText);

            yield return new WaitForSeconds(_typingSpeed);
        }

        yield return new WaitForSecondsRealtime(_timeDeleyAfterTyping);
        _isTyping = false;
        TypingComplete?.Invoke();
    }

    private void UpdateTextDisplay(string text)
    {      
        _text.text = text;
    }
}
