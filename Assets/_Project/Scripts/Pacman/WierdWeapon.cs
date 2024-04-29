using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WierdWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] WierdProjectile _projectilePrefab;
    [SerializeField] Transform _shootPoint;

    PacmanStats _stats;


    float _cooldownTimer = 0f;

    public void Equip()
    {
    }

    public void Setup(PacmanStats stats)
    {
        _stats = stats;
    }

    public void TryShoot()
    {
        if (_cooldownTimer > 0)
            return;

        _cooldownTimer = _stats.WierdShootCooldown;

        WierdProjectile projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Setup(_shootPoint.forward, _stats.WierdProjectileSpeed, _stats.WierdDamage);
    }

    void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }
}
