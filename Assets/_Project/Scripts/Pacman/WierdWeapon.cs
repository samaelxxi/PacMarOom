using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WierdWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] float _cooldown = 0.5f;
    [SerializeField] WierdProjectile _projectilePrefab;
    [SerializeField] Transform _shootPoint;


    float _cooldownTimer = 0f;

    public void TryShoot()
    {
        Debug.Log("TryShoot");
        if (_cooldownTimer > 0)
            return;

        _cooldownTimer = _cooldown;

        Debug.Log("Shoot");
        WierdProjectile projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Setup(_shootPoint.forward);
    }

    void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }
}
