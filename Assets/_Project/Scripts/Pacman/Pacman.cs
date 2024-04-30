using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pacman : MonoBehaviour
{
    [SerializeField] WierdWeapon _wierdWeapon;
    [SerializeField] PigCatcher _pigCatcher;
    [SerializeField] PacmanStats _stats;

    public WierdWeapon WierdWeapon => _wierdWeapon;

    public event Action<int> OnHealthChanged;
    public event Action OnDamaged;
    public event Action OnHeal;
    public event Action OnInvulnerable;
    public event Action OnVulnerable;


    PacmanController _controller;
    IWeapon _currentWeapon;

    int _health;
    float _invulnerableTimer = 0f;


    void Awake()
    {
        _controller = GetComponent<PacmanController>();
        _controller.Setup(_stats);
        _controller.OnShootClicked += Shoot;
        _controller.OnChangeWeaponClicked += ChangeWeapon;
        _currentWeapon = _wierdWeapon;
        _wierdWeapon.Setup(_stats);
        _pigCatcher.Setup(_stats);
        SetHealth(_stats.Health);

        Game.Instance.SetPacman(this);
    }

    void Start()
    {
        ChangeWeapon(0);
    }

    public void GetDamage(int damage)
    {
        if (_invulnerableTimer > 0)
            return;
        Debug.Log("PacmanAuch");
        SetHealth(_health - damage);
        OnDamaged?.Invoke();
        Game.Instance.AudioManager.PlayRange(Sounds.PlayerHurts, pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);
        _invulnerableTimer = _stats.InvulnerableTime;
        OnInvulnerable?.Invoke();
        if (_health <= 0)
            Game.Instance.RespawnPacman();
    }

    void SetHealth(int health)
    {
        _health = health;
        OnHealthChanged?.Invoke(_health);
    }

    public void Heal()
    {
        SetHealth(_stats.Health);
        OnHealthChanged?.Invoke(_health);
        OnHeal?.Invoke();
    }

    void ChangeWeapon(int weapon)
    {
        if (weapon == 0)
            _currentWeapon = _wierdWeapon;
        else
            _currentWeapon = _pigCatcher;
        _wierdWeapon.gameObject.SetActive(weapon == 0);
        _pigCatcher.gameObject.SetActive(weapon == 1);
        _currentWeapon.Equip();
    }

    public void Respawn(Transform checkpoint)
    {
        SetHealth(_stats.Health);
        _controller.Teleport(checkpoint);
    }

    public void Teleport(Transform checkpoint)
    {
        _controller.Teleport(checkpoint);
    }

    void Shoot()
    {
        _currentWeapon.TryShoot();
    }

    void Update()
    {
        if (_invulnerableTimer > 0)
        {
            _invulnerableTimer -= Time.deltaTime;
            if (_invulnerableTimer <= 0)
                OnVulnerable?.Invoke();
        }
        
        if (transform.position.y < -100)
            Game.Instance.RespawnPacman();
    }
}
