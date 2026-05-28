using System.Collections.Generic;
using UnityEngine;

public class Lighter : Item
{
    [SerializeField] private List<GameObject> _lights;
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
        foreach (GameObject light in _lights)
        {
            light.gameObject.SetActive(value);
        }
    }

    #endregion

}
