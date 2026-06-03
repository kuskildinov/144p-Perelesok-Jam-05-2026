using UnityEngine;

public class ExitHole : Interactable
{
    [SerializeField] private DialogPhrase _phrase;

    public override void TryInteract(Player player, Item item)
    {
        if (!_isActive)
            return;

        if (player.CurrentTakedItem == null || player.CurrentTakedItem.Type != ItemType.Light)
        {
            _root.TryShowCommentDialog(_phrase);
        }
        else
        {
            _root.TryLeaveLevel();
        }
    }
}
