using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] UnityEvent _onDamageTaken;

    public void TakeDamage()
    {
        _onDamageTaken.Invoke();
    }
}
