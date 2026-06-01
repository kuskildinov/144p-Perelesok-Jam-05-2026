using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterIllumination : MonoBehaviour
{
    [Header("Update")]
    [SerializeField] private float _updateRate = 0.1f;

    [Header("Light")]
    [SerializeField] private float _ambientLight = 0.15f;

    [SerializeField] private float _smoothSpeed = 8f;

    [Header("Raycast")]
    [SerializeField] private LayerMask _wallMask;

    [SerializeField] private float _enemyHeight = 0.5f;

    [SerializeField] private float _lightHeight = 0.2f;

    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _block;

    private float _timer;

    private float _currentIllumination;
    private float _targetIllumination;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _block = new MaterialPropertyBlock();

        _currentIllumination = _ambientLight;
        _targetIllumination = _ambientLight;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _updateRate)
        {
            _timer = 0f;
            CalculateIllumination();
        }

        _currentIllumination = Mathf.Lerp(
            _currentIllumination,
            _targetIllumination,
            Time.deltaTime * _smoothSpeed);

        _renderer.GetPropertyBlock(_block);

        _block.SetFloat(
            "_Illumination",
            _currentIllumination);

        _renderer.SetPropertyBlock(_block);
    }

    private void CalculateIllumination()
    {
        float illumination = _ambientLight;

        LightSource[] lights =
            FindObjectsByType<LightSource>(
                FindObjectsSortMode.None);

        Vector3 enemyPos =
            transform.position +
            Vector3.up * _enemyHeight;

        foreach (var light in lights)
        {
            Vector3 lightPos =
                light.transform.position +
                Vector3.up * _lightHeight;

            Vector3 dir = enemyPos - lightPos;

            float distance = dir.magnitude;

            if (distance > light.Radius)
                continue;

            bool blocked = Physics.Raycast(
                lightPos,
                dir.normalized,
                distance,
                _wallMask,
                QueryTriggerInteraction.Ignore);

            if (blocked)
                continue;

            float attenuation =
                1f - distance / light.Radius;

            illumination +=
                attenuation * light.Intensity;
        }

        _targetIllumination =
            Mathf.Clamp01(illumination);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 enemyPos =
            transform.position +
            Vector3.up * _enemyHeight;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(enemyPos, 0.05f);
    }
#endif
}