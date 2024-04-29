using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pacman : MonoBehaviour
{
    [SerializeField] WierdWeapon _wierdWeapon;
    [SerializeField] PigCatcher _pigCatcher;
    [SerializeField] PacmanStats _stats;

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
        _health = _stats.Health;

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
        _health -= damage;
        _invulnerableTimer = _stats.InvulnerableTime;
        if (_health <= 0)
            Game.Instance.RespawnPacman();
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
        _health = _stats.Health;
        _controller.Teleport(checkpoint);
    }

    void Shoot()
    {
        _currentWeapon.TryShoot();
    }

    void Update()
    {
        if (_invulnerableTimer > 0)
            _invulnerableTimer -= Time.deltaTime;
        if (transform.position.y < -100)
            Game.Instance.RespawnPacman();
    }
}
