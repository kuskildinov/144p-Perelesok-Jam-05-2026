using System.Collections.Generic;
using UnityEngine;

public class LevelTrapsHandler : MonoBehaviour
{
    private LevelRoot _root;
    private List<TrapBlock> _traps = new List<TrapBlock>();
    private List<LevelLever> _levers = new List<LevelLever>();

    public void Initialize(LevelRoot root)
    {
        _root = root;

        InitializeLevelTraps();
        InitializeLevelLevers();
    }

    private void InitializeLevelTraps()
    {
        TrapBlock[] traps = FindObjectsByType<TrapBlock>();

        foreach (TrapBlock trap in traps)
        {
            trap.Initialize(this);
            _traps.Add(trap);
        }
    }

    private void InitializeLevelLevers()
    {
        LevelLever[] levers = FindObjectsByType<LevelLever>();

        foreach (LevelLever lever in levers)
        {
            lever.Initialize(this);
            _levers.Add(lever);
        }
    }

    public void ToggleTraps()
    {
        foreach (TrapBlock trap in _traps)
        {
            trap.Toggle();
        }
    }
}
