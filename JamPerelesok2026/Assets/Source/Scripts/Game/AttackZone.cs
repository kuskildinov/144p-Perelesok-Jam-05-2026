using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public AttackZoneType Type;
    public Transform DamagerCenter;
}

public enum AttackZoneType
{
    Player,
    Enemy,
    Trap
}
