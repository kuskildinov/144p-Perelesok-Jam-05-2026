using UnityEngine;

public class LightSource : MonoBehaviour
{
    [Min(0.1f)]
    public float Radius = 5f;

    [Range(0f, 1f)]
    public float Intensity = 1f;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
#endif
}