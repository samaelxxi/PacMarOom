using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InvaderStats", menuName = "InvaderStats")]
public class InvaderStats : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
}
