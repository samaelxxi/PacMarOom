using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InvaderStats", menuName = "InvaderStats")]
public class InvaderStats : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public float MoveCooldown { get; private set; }
    [field: SerializeField] public float MoveDist { get; private set; }
    [field: SerializeField] public float ForwardMoveCooldown { get; private set; }
    [field: SerializeField] public float ForwardMoveDist { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float AttackPreparationTime { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public float AttackCooldown { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float SightRange { get; private set; }
    [field: SerializeField] public float ForgetRange { get; private set; }

}
