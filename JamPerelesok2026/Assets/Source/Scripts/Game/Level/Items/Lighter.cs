using UnityEngine;

public class Lighter : Item
{
    [SerializeField] private GameObject _pointLightObject;
    [SerializeField] private Light _pointLight;
    [SerializeField] private LayerMask _enemyLayer;

    public override void Initialize()
    {
        base.Initialize();
        
        _pointLight.range = GlobalVars.LightRadius;
    }

    private void Update()
    {
        EnemyDetectionHandler();
    }

    #region >>> TAKE

    public override void TryTake()
    {
        base.TryTake();

        ToggleLights(false);
    }

    public override void TryDrop(Transform playerTransform)
    {
        base.TryDrop(playerTransform);

        ToggleLights(true);
    }
    #endregion
    #region >>> LIGHT

    private void ToggleLights(bool value)
    {
        _pointLightObject.SetActive(value);
    }

    #endregion
    #region >>> DETECTIONS

    private void EnemyDetectionHandler()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GlobalVars.LightRadius);

        foreach (var hitCollider in hitColliders)
        {
            // Проверяем, является ли объект врагом
            if (hitCollider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (enemy.IsActive || !enemy.IsAlive)
                    return;

                enemy.Activate(this);
            }
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (IsTaked)
            return;

        Gizmos.DrawWireSphere(transform.position, GlobalVars.LightRadius);
    }

}
