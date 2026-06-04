using UnityEngine;

public class Key : Interactable
{
    #region >>> INTERACT
    public override void TryInteract(Player player, Item item)
    {
        player.TakeKey();
        Destroy(this.gameObject);
    }
    #endregion
}
