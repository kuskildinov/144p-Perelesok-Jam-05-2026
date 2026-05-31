using UnityEngine;

[RequireComponent(typeof(LightAura))]
public class Lighter : Item
{
    [SerializeField] private GameObject _pointLightObject;
    [SerializeField] private Light _pointLight;
    [SerializeField] private LayerMask _enemyLayer;

    private LightAura _aura;

    public override void Initialize(ItemsRoot root)
    {
        base.Initialize(root);

        _aura = GetComponent<LightAura>();
        _pointLight.range = GlobalVars.LightRadius;
    }

    #region >>> TAKE

    public override void TryTake()
    {
        base.TryTake();
       
        ToggleLights(false);
    }

    public override void TryDrop(Transform playerTransform)
    {
        base.TryDrop(playerTransform);
    
        ToggleLights(true);
    }
    #endregion
    #region >>> LIGHT

    private void ToggleLights(bool value)
    {
        _pointLightObject.SetActive(value);
        _aura.gameObject.SetActive(value);
    }

    #endregion
   
}
