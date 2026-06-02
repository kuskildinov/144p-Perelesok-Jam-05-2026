using DG.Tweening;
using UnityEngine;

public class TrapBlock : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;

    [SerializeField] private Transform _spikes;
    [SerializeField] private Vector3 _spikesYPositions;
    [SerializeField] private float _upDuration = 0.3f;
    [SerializeField] private float _downDuration = 1.5f;
    [SerializeField] private float _delayBeforeDown = 0.5f;
    [SerializeField] private AttackZone _attackZone;

    private LevelTrapsHandler _trapshandler;
    private Tween _currentTween;

    public bool IsActive => _isActive;

    public void Initialize(LevelTrapsHandler trapsHandler)
    {
        _trapshandler = trapsHandler;

        SetStartState();
        SubscribeToEvents();
    }

    public void Toggle()
    {
        if (_isActive)
            Deactivate();
        else
            Activate();
    }

    private void SetStartState()
    {
        if (_isActive)
            Activate();
        else
            Deactivate();
    }

    private void SpikeOnce()
    {
        if (!_isActive)
            return;

        _currentTween?.Kill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_spikes.DOLocalMoveY(_spikesYPositions.x, _upDuration).SetEase(Ease.OutQuad));
        sequence.AppendInterval(_delayBeforeDown);
        sequence.Append(_spikes.DOLocalMoveY(_spikesYPositions.y, _downDuration).SetEase(Ease.InQuad));

        sequence.OnComplete(() =>
        {
          
        });

        sequence.Play();
        _currentTween = sequence;
    }   

    private void Activate()
    {
        _isActive = true;

        _spikes.localPosition = new Vector3(_spikes.localPosition.x,_spikesYPositions.y, _spikes.localPosition.z);
    }

    private void Deactivate()
    {
        _isActive = false;

        _spikes.localPosition = new Vector3(_spikes.localPosition.x, _spikesYPositions.z, _spikes.localPosition.z);
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _attackZone.OnActivated += OnPlayerEnter;
    }

    private void UnsubscribeFromEvents()
    {
        _attackZone.OnActivated -= OnPlayerEnter;
    }

    private void OnPlayerEnter()
    {       
        SpikeOnce();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

}
