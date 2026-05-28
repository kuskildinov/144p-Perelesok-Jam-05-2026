using System.Collections.Generic;
using UnityEngine;

public class InventoryRoot : CompositeRoot
{
    [SerializeField] private Inventory _inventory;

    public override void Compose()
    {
        _inventory.Initialize();
    }
}
