using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackDuration;
    [Header("Attack Zones")]
    [SerializeField] private AttackZone _topAttackZone;
    [SerializeField] private AttackZone _bottomAttackZone;
    [SerializeField] private AttackZone _leftAttackZone;
    [SerializeField] private AttackZone _rightAttackZone;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private AttackZone _curentAttackZone;
    private Coroutine _attackRoutine;
  
    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        SubscribeToEvents();
    }

    #region >>> ATTACK
    public void TryAttack()
    {
        if (!_player.IsActive)
            return;

        LookDirection currentDirection = _player.CurrentLookDirection;
        ActivateAttackZoneByDirection(currentDirection);
    }

    private void ActivateAttackZoneByDirection(LookDirection direction)
    {        
        if(_curentAttackZone != null)
        {
            _curentAttackZone.gameObject.SetActive(false);
            _curentAttackZone = null;
        }

        switch(direction)
        {
            case LookDirection.Top:
                {
                    _curentAttackZone = _topAttackZone;
                    break;
                }
            case LookDirection.Down:
                {
                    _curentAttackZone = _bottomAttackZone;
                    break;
                }
            case LookDirection.Right:
                {
                    _curentAttackZone = _rightAttackZone;
                    break;
                }
            case LookDirection.Left:
                {
                    _curentAttackZone = _leftAttackZone;
                    break;
                }
        }

        if (_attackRoutine != null)
            StopCoroutine(ActivateAttackZoneRoutine(_curentAttackZone));

        _attackRoutine = StartCoroutine(ActivateAttackZoneRoutine(_curentAttackZone));
    }

    private IEnumerator ActivateAttackZoneRoutine(AttackZone zone)
    {
        _player.ToggleActivation(false);
        zone.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(_attackDuration);
        zone.gameObject.SetActive(false);
        _player.ToggleActivation(true);
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.AttackInput += OnAttackInputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.AttackInput -= OnAttackInputChanged;
    }

    private void OnAttackInputChanged()
    {
        Item currentTakedItem = _player.GetCurrentTakedItem();

        if (currentTakedItem == null || currentTakedItem.Type != ItemType.Sword)
            return;

        TryAttack();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}
