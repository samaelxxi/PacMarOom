using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pacman : MonoBehaviour
{
    [SerializeField] WierdWeapon _wierdWeapon;
    [SerializeField] PacmanStats _stats;

    PacmanController _controller;
    IWeapon _currentWeapon;

    int _health;

    void Awake()
    {
        _controller = GetComponent<PacmanController>();
        _controller.Setup(_stats);
        _controller.OnShootClicked += Shoot;
        _currentWeapon = _wierdWeapon;
        _wierdWeapon.Setup(_stats);
        _health = _stats.Health;

        Game.Instance.SetPacman(this);
    }

    public void GetDamage(int damage)
    {
        Debug.Log("PacmanAuch");
        _health -= damage;
        if (_health <= 0)
            Game.Instance.RespawnPacman();
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
        
    }
}
