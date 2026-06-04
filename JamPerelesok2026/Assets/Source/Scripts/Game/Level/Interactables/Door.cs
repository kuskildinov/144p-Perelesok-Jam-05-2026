using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : Interactable
{
    private const string AnimatorOpenParam = "Open";

    [SerializeField] private Collider _doorCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _openSound;

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
        if(player.HasKey)
        {
            player.UseKey();
            Open();
        }        
    }
    #endregion
    #region >>> SOUNDS

    private void PlayOpenSound()
    {
        _source.PlayOneShot(_openSound);
    }

    #endregion

    private void Open()
    {
        _isOpen = true;
        _doorCollider.enabled = false;
        PlayOpenSound();
        HideIndicator();
        PlayOpenAnimation();
    }

    private void PlayOpenAnimation()
    {
        _animator.SetBool(AnimatorOpenParam, true);
    }
}
