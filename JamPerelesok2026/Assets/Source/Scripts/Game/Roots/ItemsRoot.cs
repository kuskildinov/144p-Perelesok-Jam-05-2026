using UnityEngine;

public class ItemsRoot : CompositeRoot
{
    private PlayerRoot _playerRoot;

    public override void Compose()
    {
        _playerRoot = FindAnyObjectByType<PlayerRoot>();

        InitializeItems();
    }

    private void InitializeItems()
    {
        Item[] items = FindObjectsByType<Item>();

        foreach (Item item in items)
        {
            item.Initialize(this);
        }
    }
}
