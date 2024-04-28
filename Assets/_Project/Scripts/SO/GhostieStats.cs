using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostieStats", menuName = "Ghostie/Stats")]
public class GhostieStats : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; } = 2;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 180;
    [field: SerializeField] public float ChaseSpeed { get; private set; } = 3;
    [field: SerializeField] public int Damage { get; private set; } = 1;
    [field: SerializeField] public int Health { get; private set; } = 3;
    [field: SerializeField] public float SightRange { get; private set; } = 8;
    [field: SerializeField] public float ForgetRange { get; private set; } = 14;
    [field: SerializeField] public float AttackDelay { get; private set; } = 0.2f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2;
    [field: SerializeField] public float AttackCooldown { get; private set; } = 1;
    [field: SerializeField] public float AttackDuration { get; private set; } = 0.5f;
}
