using UnityEngine;

public class LeveLightArea : MonoBehaviour
{
    private Transform _currentTarget;
    private Player _player;
    private Lighter _lighter;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _lighter = FindAnyObjectByType<Lighter>();
    }

    private void Update()
    {
        if (_player == null || _lighter == null)
            return;

        if (_player.CurrentTakedItem != null && _player.CurrentTakedItem.Type == ItemType.Light)
        {
            _currentTarget = _player.transform;
        }
        else
        {
            _currentTarget = _lighter.transform;
        }

        SetPosition(_currentTarget);
    }

    private void SetPosition(Transform newTransform)
    {
        Vector3 newPosition = new Vector3(newTransform.position.x, transform.position.y, newTransform.position.z);

        transform.position = newPosition;
    }
}
