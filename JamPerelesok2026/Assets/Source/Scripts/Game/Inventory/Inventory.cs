using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> _currentItems;

    public void Initialize()
    {
        _currentItems = new List<Item>();
    }
}
