using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WierdWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] float _cooldown = 0.5f;
    [SerializeField] WierdProjectile _projectilePrefab;


    float _cooldownTimer = 0f;

    public void TryShoot()
    {
        if (_cooldownTimer > 0)
            return;

        _cooldownTimer = _cooldown;

        WierdProjectile projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        projectile.Setup(transform.forward);
    }

    void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }
}
