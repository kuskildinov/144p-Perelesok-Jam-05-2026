using UnityEngine;

public class CallItem : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PhoneItem _phone;

    public void PlayCallSound()
    {
        _audioSource.Play();
    }

    public void StopCallSound()
    {
        _audioSource.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_phone.IsActive)
            return;

        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            PlayCallSound();
        }
    }

    private void OnTriggerExit(Collider other)
    {       
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            StopCallSound();
        }
    }
}
