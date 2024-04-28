using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PacmanStats", menuName = "Pacman/Stats")]
public class PacmanStats : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; } = 3;
    [field: SerializeField] public float Speed { get; private set; } = 2;
    [field: SerializeField] public float MouseSensitivity { get; private set; } = 2;
    [field: SerializeField] public float Gravity { get; private set; } = 1;
    [field: SerializeField] public float JumpForce { get; private set; } = 5;

    [field: Header("Wierd Weapon"), 
    SerializeField]         public int WierdDamage { get; private set; } = 1;
    [field: SerializeField] public float WierdShootCooldown { get; private set; } = 0.5f;
    [field: SerializeField] public float WierdProjectileSpeed { get; private set; } = 10;
}