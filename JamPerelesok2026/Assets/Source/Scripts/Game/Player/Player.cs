using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovment _playerMovment;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private PlayerInteractions _playerInteractions;

    private CharacterController _characterController;
    private bool _isActive = true;
    private PlayerRoot _root;
   
    public bool IsActive => _isActive;
    public CharacterController Controller => _characterController;

   public void Initialzie(PlayerRoot root)
    {
        _root = root;
        _characterController = GetComponent<CharacterController>();
        
        _playerMovment.Initialize(this, _root.InputHandler);
        _playerVisual.Initialize(this, _root.InputHandler);
        _playerInteractions.Initialize(this, _root.InputHandler);
    }

    #region >>> ITEMS

    public void OnItemTaked(ItemType type)
    {
        _playerVisual.OnItemTaked(type);
    }

    public void OnItemDropped(ItemType type)
    {
        _playerVisual.OnItemDropped(type);
    }

    #endregion
}
