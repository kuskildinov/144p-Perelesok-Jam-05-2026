using System.Collections;
using UnityEngine;

public class BrackableBlockCell : LevelBlockCell
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float _destroyTime = 5f;

    private bool _isActive;

    public override void Start()
    {
        base.Start();
        _isActive = true;
        _particles.Stop();
    }

    private void DestroyWall()
    {
        _meshRenderer.gameObject.SetActive(false);
        _particles.Play();

        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSecondsRealtime(_destroyTime);

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<AttackZone>(out AttackZone attackZone))
        {
            if(attackZone.Type == AttackZoneType.Player && _isActive)
            {
                DestroyWall();
            }
        }
    }
}
