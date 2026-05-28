using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovment _playerMovment;

    private CharacterController _characterController;
    private bool _isActive = true;
    private PlayerRoot _root;
   
    public bool IsActive => _isActive;
    public CharacterController Controller => _characterController;

   public void Initialzie(PlayerRoot root)
    {
        _root = root;
        _characterController = GetComponent<CharacterController>();

        SetStartPosition();

        _playerMovment.Initialize(this, _root.InputHandler);
    }

    #region >>> LAYER POSITION

    private void SetStartPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GlobalVars.PlayerPositionZ);
    }

    #endregion
}
