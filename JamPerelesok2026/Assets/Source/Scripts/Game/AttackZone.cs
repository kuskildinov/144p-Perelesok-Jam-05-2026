using System;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public AttackZoneType Type;
    public Transform DamagerCenter;
    public event Action OnActivated;

    public void Activate()
    {
        OnActivated?.Invoke();
    }
}

public enum AttackZoneType
{
    Player,
    Enemy,
    Trap
}
