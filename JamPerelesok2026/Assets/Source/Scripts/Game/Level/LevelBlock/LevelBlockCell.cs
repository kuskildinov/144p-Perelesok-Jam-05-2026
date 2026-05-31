using UnityEngine;

public class LevelBlockCell : MonoBehaviour
{
    [SerializeField] private GameObject _outline;

    public void ToggleOutline(bool value)
    {
        _outline.gameObject.SetActive(value);
    }
}
