using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private GameObject _visual;
    [Header("Indicator")]
    [SerializeField] private GameObject _targetIndeicator;

    private bool _isTaked = false;
  
    public ItemType Type => _type;
    public bool IsTaked => _isTaked;

    public void Initialize()
    {
        HideTargetIndicator();                
    }

    #region >>> TARGET INDEICATOR

    public void ShowTargetIndicator()
    {
        if (_isTaked)
            return;

        _targetIndeicator.gameObject.SetActive(true);      
    }

    public void HideTargetIndicator()
    {
        if (_isTaked)
            return;

        _targetIndeicator.gameObject.SetActive(false);      
    }

    #endregion
    #region >>> TAKE

    public virtual void TryTake()
    {
        _isTaked = true;
        _visual.gameObject.SetActive(false);
    }

    public virtual void TryDrop(Transform playerTransform)
    {
        _isTaked = false;
        transform.position = playerTransform.position;
        _visual.gameObject.SetActive(true);
    }

    #endregion
    #region >>> ACTIVATION



    #endregion

}

public enum ItemType
{
    Light,
    Sword,
    Key,
}
