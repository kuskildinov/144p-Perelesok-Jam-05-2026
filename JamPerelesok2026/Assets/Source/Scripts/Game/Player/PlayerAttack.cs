using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private PlayerInputHandler _inputHandler;
  
    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        SubscribeToEvents();
    }

    public void TryAttack()
    {
        if (!_player.IsActive)
            return;

        Debug.Log("Attack");
    }

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
