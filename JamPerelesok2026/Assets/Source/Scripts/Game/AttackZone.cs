using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public AttackZoneType Type;
}

public enum AttackZoneType
{
    Player,
    Enemy
}
