using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject _interactionIndicator;

    protected bool _isActive = true;

    public bool IsActive => _isActive;

    #region >>> PLAYER INTERACTION
    public virtual void OnPlayerEnter()
   {
        ShowIndeicator();
    }

    public virtual void OnPlayerExit()
    {
        HideIndicator();
    }
    #endregion
    #region >>> INTERACT
    public virtual void TryInteract(Player player, Item item)
    {

    }
    #endregion
    #region >>> INDICATOR

    protected void ShowIndeicator()
    {
        _interactionIndicator.gameObject.SetActive(true);
    }

    protected void HideIndicator()
    {       
        _interactionIndicator.gameObject.SetActive(false);
    }

    #endregion
}
