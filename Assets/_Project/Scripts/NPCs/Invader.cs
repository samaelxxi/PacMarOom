using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : NPC
{
    [SerializeField] InvaderStats _stats;
    [SerializeField] InvaderProjectile _projectilePrefab;
    [SerializeField] Transform _shootPoint;
    [SerializeField] int _killScore = 50;

    protected override float InvulnerableTime => _stats.InvulnerableTime;

    float _shootTime;

    public void SetShootTime(float time) => _shootTime = time;

    // Start is called before the first frame update
    void Start()
    {
        _health = _stats.Health;
        _shootTimer = 0;
    }

    public override void TakeDamage(int damage)
    {
        if (_invulnerableTimer > 0)
            return;

        Game.Instance.AudioManager.PlayRange(Sounds.EnemyHurts, pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);

        _health -= damage;
        if (_health <= 0)
        {
            Game.Instance.AddScore(_killScore);
            Game.Instance.AudioManager.Play("invaderDied", pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);
            Die();
        }
        else
        {
            BecomeInvulnerable(InvulnerableTime);
            Game.Instance.AudioManager.Play("invaderDamaged", pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);
        }
    }

    protected override void Die()
    {
        base.Die();
    }

    public void Move(Vector3 displacement)
    {
        transform.position += displacement;
    }

    public void PrepareShoot()
    {
        _shootTimer = UnityEngine.Random.Range(_shootTime - _shootTime*0.2f, _shootTime + _shootTime*0.2f);
    }

    float _shootTimer;
    protected override void Update()
    {
        base.Update();
        if (_shootTimer <= 0)
            PrepareShoot();

        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0)
                Shoot();
        }
    }

    List<GameObject> _projectiles = new();
    void Shoot()
    {
        var projectile = Instantiate(_projectilePrefab, _shootPoint.position, transform.rotation, transform.parent);
        _projectiles.Add(projectile.gameObject);
        projectile.Setup(transform.forward, _stats.ProjectileSpeed, _stats.Damage);
        // Hashtable hash = new Hashtable
        // {
        //     { "maxdistance", 30.0f },
        //     { "pitch", UnityEngine.Random.Range( 0.9f, 1.1f )},
        //     { "volume", 0.3f},
        //     { "spatialblend", 1.0f },
        //     { "rolloffmode", AudioRolloffMode.Linear},
        //     { "position", transform.position}
        // };
        // Game.Instance.AudioManager.Play("shoot", hash);
    }

    public override void Reset()
    {
        foreach (var projectile in _projectiles)
            if (projectile != null) Destroy(projectile);
        _projectiles.Clear();
        _health = _stats.Health;
        gameObject.SetActive(true);
    }
}
