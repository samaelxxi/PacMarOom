using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WierdWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] WierdProjectile _projectilePrefab;
    [SerializeField] Transform _shootPoint;

    public event Action<int> OnAmmoChanged;

    public int Ammo => _ammo;

    PacmanStats _stats;

    int _ammo;


float _cooldownTimer = 0f;

    public void Equip()
    {
    }

    public void Setup(PacmanStats stats)
    {
        _stats = stats;
        _ammo = _stats.WierdAmmo;
    }

    public void AddAmmo(int ammo)
    {
        _ammo += ammo;
        OnAmmoChanged?.Invoke(_ammo);
    }

    public void TryShoot()
    {
        if (_cooldownTimer > 0 || _ammo <= 0)
            return;

        _cooldownTimer = _stats.WierdShootCooldown;

        WierdProjectile projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Setup(_shootPoint.forward, _stats.WierdProjectileSpeed, _stats.WierdDamage);
        AddAmmo(-1);
    }

    void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }
}
