using UnityEngine;

public class PhoneItem : NarativeItem
{
    [SerializeField] private AudioSource _audioSource;

    #region >>> PLAYER INTERACTION
    public override void OnPlayerEnter()
    {
        base.OnPlayerEnter();

        _audioSource.Play();
    }

    public override void OnPlayerExit()
    {
        base.OnPlayerExit();

        _audioSource.Stop();

    }
    #endregion
}
