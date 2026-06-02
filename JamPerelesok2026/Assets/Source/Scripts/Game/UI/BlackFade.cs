using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _fadeImage; // Черное изображение на весь экран

    [Header("Settings")]
    [SerializeField] private float _fadeInDuration = 1f; // Длительность появления (из черного в прозрачный)
    [SerializeField] private float _fadeOutDuration = 1f; // Длительность затухания (в черный)
    [SerializeField] private float _holdDuration = 0.5f; // Задержка между fade in и fade out (опционально)
    [SerializeField] private bool _fadeAtStart = true; // Затухать ли в начале
    [SerializeField] private bool _fadeAtEnd = true; // Затухать ли в конце

    private Tween _currentFadeTween;
    private Action _onFadeCompleteCallback;

    private void Start()
    {
        if (_fadeAtStart)
        {
            FadeIn();
        }
    }

    /// <summary>
    /// Появление из черного
    /// </summary>
    public void FadeIn(float customDuration = -1, Action onComplete = null)
    {
        KillCurrentTween();

        float duration = customDuration > 0 ? customDuration : _fadeInDuration;
        _onFadeCompleteCallback = onComplete;

        // Убеждаемся, что изображение полностью черное
        SetImageAlpha(1f);

        // Анимация затухания до прозрачного
        _currentFadeTween = _fadeImage.DOFade(0f, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _onFadeCompleteCallback?.Invoke();
                _onFadeCompleteCallback = null;
            });
    }

    /// <summary>
    /// Затухание в черный
    /// </summary>
    public void FadeOut(float customDuration = -1, Action onComplete = null)
    {
        KillCurrentTween();

        float duration = customDuration > 0 ? customDuration : _fadeOutDuration;
        _onFadeCompleteCallback = onComplete;

        // Анимация затухания до черного
        _currentFadeTween = _fadeImage.DOFade(1f, duration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                _onFadeCompleteCallback?.Invoke();
                _onFadeCompleteCallback = null;
            });
    }

    /// <summary>
    /// Полный цикл: затухание в черный и затем появление
    /// </summary>
    public void FadeOutIn(float fadeOutDur = -1, float fadeInDur = -1, Action onComplete = null)
    {
        float outDur = fadeOutDur > 0 ? fadeOutDur : _fadeOutDuration;
        float inDur = fadeInDur > 0 ? fadeInDur : _fadeInDuration;

        FadeOut(outDur, () =>
        {
            FadeIn(inDur, onComplete);
        });
    }

    /// <summary>
    /// Полный цикл с задержкой между затуханием и появлением
    /// </summary>
    public void FadeOutInWithDelay(float fadeOutDur = -1, float delay = -1, float fadeInDur = -1, Action onComplete = null)
    {
        float outDur = fadeOutDur > 0 ? fadeOutDur : _fadeOutDuration;
        float inDur = fadeInDur > 0 ? fadeInDur : _fadeInDuration;
        float hold = delay > 0 ? delay : _holdDuration;

        FadeOut(outDur, () =>
        {
            DOVirtual.DelayedCall(hold, () =>
            {
                FadeIn(inDur, onComplete);
            });
        });
    }

    /// <summary>
    /// Мгновенная установка прозрачности
    /// </summary>
    private void SetImageAlpha(float alpha)
    {
        if (_fadeImage != null)
        {
            Color color = _fadeImage.color;
            color.a = Mathf.Clamp01(alpha);
            _fadeImage.color = color;
        }
    }

    /// <summary>
    /// Мгновенное включение черного экрана
    /// </summary>
    public void SetBlackImmediate()
    {
        KillCurrentTween();
        SetImageAlpha(1f);
    }

    /// <summary>
    /// Мгновенное выключение черного экрана
    /// </summary>
    public void SetClearImmediate()
    {
        KillCurrentTween();
        SetImageAlpha(0f);
    }

    private void KillCurrentTween()
    {
        if (_currentFadeTween != null && _currentFadeTween.IsActive())
        {
            _currentFadeTween.Kill();
            _currentFadeTween = null;
        }
    }

    private void OnDestroy()
    {
        KillCurrentTween();
    }
}
