using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private GameObject _doorVisual;

    public override void TryInteract(Player player, Item item)
    {       
        if (item == null || item.Type != ItemType.Key)
            return;

        player.OnCurrentItemUsed();     

        Open();
    }

    private void Open()
    {       
        HideIndicator();
        _isActive = false;

        //Анимация открытия
        _doorVisual.gameObject.SetActive(false);
    }
}
