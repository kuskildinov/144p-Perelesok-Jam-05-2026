using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : Interactable
{
    private const string AnimatorOpenParam = "Open";

    [SerializeField] private Collider _doorCollider;
    [SerializeField] private Animator _animator;

    private bool _isOpen = false;

    #region >>> PLAYER INTERACTION
    public override void OnPlayerEnter()
    {
        if (_isOpen)
            return;

        base.OnPlayerEnter();
    }

    public override void OnPlayerExit()
    {
        if (_isOpen)
            return;

        base.OnPlayerExit();
    }
    #endregion
    #region >>> INTERACT
    public override void TryInteract(Player player, Item item)
    {       
        if (item == null || item.Type != ItemType.Key)
            return;

        player.OnCurrentItemUsed();     

        Open();
    }
    #endregion

    private void Open()
    {
        _isOpen = true;
        _doorCollider.enabled = false;

        HideIndicator();
        PlayOpenAnimation();
    }

    private void PlayOpenAnimation()
    {
        _animator.SetBool(AnimatorOpenParam, true);
    }
}
