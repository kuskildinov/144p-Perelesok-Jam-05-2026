using System.Collections;
using UnityEngine;

public class BrackableBlockCell : LevelBlockCell
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float _destroyTime = 5f;
    [SerializeField] private AudioSource _mainSource;
    [SerializeField] private AudioClip _brakeSound;

    private bool _isActive;

    public override void Start()
    {
        base.Start();
        _isActive = true;
        _particles.Stop();
    }

    private void DestroyWall()
    {
        if (!_isActive)
            return;

        PlayBreakSound();
        _isActive = false;
        _meshRenderer.gameObject.SetActive(false);
        _particles.Play();
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSecondsRealtime(_destroyTime);

        this.gameObject.SetActive(false);
    }

    private void PlayBreakSound()
    {
        _mainSource.PlayOneShot(_brakeSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Нас увидел {other}");

        if (other.gameObject.TryGetComponent<AttackZone>(out AttackZone attackZone))
        {           
            if(attackZone.Type == AttackZoneType.Player && _isActive)
            {
                DestroyWall();
            }
        }
    }
}
