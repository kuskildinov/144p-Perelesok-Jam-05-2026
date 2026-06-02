using System.Collections.Generic;
using UnityEngine;

public class LevelInteractablesHandler : MonoBehaviour
{
    private LevelRoot _root;
    private List<Interactable> _currentinteractables = new List<Interactable>();

    public void Initialize(LevelRoot root)
    {
        _root = root;

        InitializeLevelInteractables();
    }

    private void InitializeLevelInteractables()
    {
        Interactable[] interactable = FindObjectsByType<Interactable>();

        foreach (Interactable interact in interactable)
        {
            interact.Initialize(_root);
            _currentinteractables.Add(interact);
        }
    }
}
