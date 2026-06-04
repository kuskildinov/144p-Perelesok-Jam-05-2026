using UnityEngine;

public class PhoneItem : NarativeItem
{
    [SerializeField] private CallItem _callItem;

    public override void TryInteract(Player player, Item item)
    {
        base.TryInteract(player, item);

        Destroy(_callItem.gameObject);
    }

}
