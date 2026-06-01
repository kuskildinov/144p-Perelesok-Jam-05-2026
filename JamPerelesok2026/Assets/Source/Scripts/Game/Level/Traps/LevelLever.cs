using UnityEngine;

public class LevelLever : Interactable
{
    [SerializeField] private Vector2 _rotationValues;
    [SerializeField] private Transform _handle;

    private LevelTrapsHandler _trapsHandler;
    
    public void Initialize(LevelTrapsHandler trapsHandler)
    {
        _trapsHandler = trapsHandler;

        UpdateHandlePosition();
    }

    public override void TryInteract(Player player, Item item)
    {
        _trapsHandler.ToggleTraps();
        _isActive = !_isActive;

        UpdateHandlePosition();
    }

    private void UpdateHandlePosition()
    {
        if (_isActive)
        {
            _handle.localEulerAngles = new Vector3(_rotationValues.x, 0, 0);
        }
        else
        {
            _handle.localEulerAngles = new Vector3(_rotationValues.y, 0, 0 );
        }
    }
}
