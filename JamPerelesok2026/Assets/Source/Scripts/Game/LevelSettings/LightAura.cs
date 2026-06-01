using UnityEngine;

public class LightAura : MonoBehaviour
{
    private void Update()
    {
        EnemyDetectionHandler();
    }


    #region >>> DETECTIONS

    private void EnemyDetectionHandler()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GlobalVars.LightRadius);

        foreach (var hitCollider in hitColliders)
        {
            // Проверяем, является ли объект врагом
            if (hitCollider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (!enemy.IsAlive)
                    return;

                enemy.Activate(this);
            }
        }
    }

    #endregion

    private void OnDrawGizmos()
    {        
        Gizmos.DrawWireSphere(transform.position, GlobalVars.LightRadius);
    }

}
