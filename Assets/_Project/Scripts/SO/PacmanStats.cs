using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PacmanStats", menuName = "Pacman/Stats")]
public class PacmanStats : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; } = 3;
    [field: SerializeField] public float InvulnerableTime { get; private set; } = 2;
    [field: SerializeField] public float Speed { get; private set; } = 2;
    [field: SerializeField] public float MouseSensitivity { get; private set; } = 2;
    [field: SerializeField] public float Gravity { get; private set; } = 1;
    [field: SerializeField] public float JumpForce { get; private set; } = 5;
    [field: SerializeField] public float JumpWindow { get; private set; } = 0.1f;
    [field: SerializeField] public int JumpDamage { get; private set; } = 1;


    [field: Header("Wierd Weapon"), 
    SerializeField]         public int WierdDamage { get; private set; } = 1;
    [field: SerializeField] public float WierdShootCooldown { get; private set; } = 0.5f;
    [field: SerializeField] public float WierdProjectileSpeed { get; private set; } = 10;
    [field: SerializeField] public int WierdAmmo { get; private set; } = 50;

    [field: Header("Pig Catcher Weapon"),
    SerializeField]         public float PigCatchCooldown { get; private set; } = 1f;
    [field: SerializeField] public float CatchDuration { get; private set; } = 0.5f;
}
