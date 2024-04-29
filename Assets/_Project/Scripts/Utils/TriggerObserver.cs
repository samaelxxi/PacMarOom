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

    bool _onlyOnce = false;
    bool _triggered = false;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void SetOnlyOnce(bool onlyOnce)
    {
        _onlyOnce = onlyOnce;
    }

    public void Reset()
    {
        _triggered = false;
    }

    public void SetTestPredicate(Func<Collider, bool> testPredicate)
    {
        _testPredicate = testPredicate;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_triggered && _onlyOnce)
            return;

        if (_testPredicate is null || _testPredicate(other))
        {
            OnTriggerEnterEvent?.Invoke(other);
            _triggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_testPredicate is null || _testPredicate(other))
            OnTriggerExitEvent?.Invoke(other);
    }
}
