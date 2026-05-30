using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private GameObject _visual;
    [Header("Indicator")]
    [SerializeField] private GameObject _targetIndeicator;

    private Collider _collider;
    private bool _isTaked = false;
  
    public ItemType Type => _type;
    public bool IsTaked => _isTaked;

    public  virtual void Initialize()
    {
        _collider = GetComponent<Collider>();
        HideTargetIndicator();                
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        HideTargetIndicator();
    }

    #region >>> TARGET INDEICATOR

    public void ShowTargetIndicator()
    {
        if (_isTaked)
            return;

        if (_targetIndeicator != null)
            _targetIndeicator.gameObject.SetActive(true);      
    }

    public void HideTargetIndicator()
    {
        if (_isTaked)
            return;

        if(_targetIndeicator != null)
            _targetIndeicator.gameObject.SetActive(false);      
    }

    #endregion
    #region >>> TAKE

    public virtual void TryTake()
    {
        _isTaked = true;
        _collider.enabled = false;
        _visual.gameObject.SetActive(false);
    }

    public virtual void TryDrop(Transform playerTransform)
    {
        _isTaked = false;
        _collider.enabled = true;
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
