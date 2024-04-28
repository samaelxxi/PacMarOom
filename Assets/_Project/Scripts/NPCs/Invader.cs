using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : NPC
{
    [SerializeField] InvaderStats _stats;
    [SerializeField] InvaderProjectile _projectilePrefab;
    [SerializeField] Transform _shootPoint;

    protected override float InvulnerableTime => _stats.InvulnerableTime;

    float _shootTime;

    public void SetShootTime(float time) => _shootTime = time;

    // Start is called before the first frame update
    void Start()
    {
        _health = _stats.Health;
    }

    public override void TakeDamage(int damage)
    {
        if (_invulnerableTimer > 0)
            return;

        _health -= damage;
        if (_health <= 0)
            gameObject.SetActive(false);
        else
            BecomeInvulnerable(InvulnerableTime);
    }

    public void Move(Vector3 displacement)
    {
        transform.position += displacement;
    }

    public void PrepareShoot()
    {
        this.InSeconds(UnityEngine.Random.Range(_shootTime - _shootTime*0.2f, _shootTime + _shootTime*0.2f), () => Shoot());
    }

    void Shoot()
    {
        var projectile = Instantiate(_projectilePrefab, _shootPoint.position, transform.rotation, transform.parent);
        projectile.Setup(transform.forward, _stats.ProjectileSpeed, _stats.Damage);
        PrepareShoot();
    }
}
