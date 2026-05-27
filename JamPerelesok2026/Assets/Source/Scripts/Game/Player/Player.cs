using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovment _playerMovment;

    private bool _isActive = true;
    private PlayerRoot _root;
    private Rigidbody2D _rigidbody;

    public bool IsActive => _isActive;
    public Rigidbody2D Rigidbody => _rigidbody;

   public void Initialzie(PlayerRoot root)
    {
        _root = root;
        _rigidbody = GetComponent<Rigidbody2D>();

        _playerMovment.Initialize(this, _root.InputHandler);
    }


}
