using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pacman : MonoBehaviour
{
    [SerializeField] WierdWeapon _wierdWeapon;

    PacmanController _controller;
    IWeapon _currentWeapon;

    void Awake()
    {
        _controller = GetComponent<PacmanController>();
        _controller.OnShootClicked += Shoot;
        _currentWeapon = _wierdWeapon;
    }

    void Shoot()
    {
        _currentWeapon.TryShoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
