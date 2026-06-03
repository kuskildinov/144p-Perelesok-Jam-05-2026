using UnityEngine;

public class NarativeItem : Interactable
{
    [SerializeField] private DialogPhrase _dialogPhrase;

    public override void TryInteract(Player player, Item item)
    {
        if (!_isActive)
            return;
               
        _root.TryShowCommentDialog(_dialogPhrase);
    }
}
