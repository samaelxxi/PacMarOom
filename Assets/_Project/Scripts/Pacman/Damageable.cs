using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] UnityEvent<int> _onDamageTaken;

    public void TakeDamage(int damage)
    {
        _onDamageTaken.Invoke(damage);
    }
}
