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
    public event Action OnAmmoAdded;

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
        AddAmmo(stats.WierdAmmo);
    }

    public void AddAmmo(int ammo)
    {
        _ammo += ammo;
        OnAmmoChanged?.Invoke(_ammo);
        if (ammo > 0)
            OnAmmoAdded?.Invoke();
    }

    public void TryShoot()
    {
        if (_cooldownTimer > 0 || _ammo <= 0)
            return;

        _cooldownTimer = _stats.WierdShootCooldown;

        WierdProjectile projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Setup(_shootPoint.forward, _stats.WierdProjectileSpeed, _stats.WierdDamage);
        Game.Instance.AudioManager.Play("playerShoot", pitch: UnityEngine.Random.Range(0.9f, 1.1f));
        AddAmmo(-1);
    }

    void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }
}
