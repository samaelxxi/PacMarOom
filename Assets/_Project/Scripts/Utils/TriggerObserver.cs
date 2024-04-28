using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerObserver : MonoBehaviour
{
    public event System.Action<Collider> OnTriggerEnterEvent;
    public event System.Action<Collider> OnTriggerExitEvent;

    Collider _collider;

    Func<Collider, bool> _testPredicate;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void SetTestPredicate(Func<Collider, bool> testPredicate)
    {
        _testPredicate = testPredicate;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_testPredicate is null || _testPredicate(other))
            OnTriggerEnterEvent?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (_testPredicate is null || _testPredicate(other))
            OnTriggerExitEvent?.Invoke(other);
    }
}
