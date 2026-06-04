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
    [Header("Sounds")]
    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _girlSound_1;
    [SerializeField] private AudioClip _girlSound_2;

    [SerializeField] private AudioClip _deathSound_1;
    [SerializeField] private AudioClip _deathSound_2;

    [SerializeField] private AudioClip _otherSound_1;
    [SerializeField] private AudioClip _otherSound_2;

    private string _fullText;
    private string _currentText = "";
    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private DialogType _currentType;

    public bool IsTyping => _isTyping;
    public string FullText => _fullText;

    public event Action TypingStart;
    public event Action TypingComplete;
       
    public void ChangeTextAndTyping(DialogPhrase dialog)
    {
        _fullText = dialog.Phrase;
        _text.color = dialog.Color;
        _currentType = dialog.Type;

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

    private void PlaySoundByType()
    {
        int rand = UnityEngine.Random.Range(0,2);
        AudioClip currentClip = _girlSound_1;
        if (_currentType == DialogType.Girl)
        {
           if(rand == 0)
            {
                currentClip = _girlSound_1;
            }
           else
            {
                currentClip = _girlSound_2;
            }
        }
        else if(_currentType == DialogType.Death)
        {
            if (rand == 0)
            {
                currentClip = _deathSound_1;
            }
            else
            {
                currentClip = _deathSound_2;
            }
        }
        else
        {
            if (rand == 0)
            {
                currentClip = _otherSound_1;
            }
            else
            {
                currentClip = _otherSound_2;
            }
        }
        _source.PlayOneShot(currentClip);
    }

    private IEnumerator TypeText()
    {
        _isTyping = true;

        for (int i = 0; i <= _fullText.Length; i++)
        {
            PlaySoundByType();
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
