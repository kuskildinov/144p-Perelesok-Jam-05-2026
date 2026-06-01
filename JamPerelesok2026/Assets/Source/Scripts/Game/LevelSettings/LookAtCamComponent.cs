using UnityEngine;

public class LookAtCamComponent : MonoBehaviour
{
    [SerializeField] private Transform _spriteTransform;
    private Camera _targetCamera;

    private void Start()
    {
        _targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        RotateToCamHandler();
    }

    #region >>> LOOK AT CAM

    private void RotateToCamHandler()
    {
        if (_targetCamera == null) return;

        Vector3 directionToCamera = _targetCamera.transform.position - _spriteTransform.position;
        
        //directionToCamera.z = 0;
        directionToCamera.x = 0;
        //directionToCamera.y = 0;

        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            _spriteTransform.rotation = targetRotation;
        }
    }
    #endregion
}
