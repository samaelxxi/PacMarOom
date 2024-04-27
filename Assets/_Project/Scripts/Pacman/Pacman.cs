using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pacman : MonoBehaviour
{
    PacmanController _controller;

    WierdWeapon _weapon;

    void Awake()
    {
        _controller = GetComponent<PacmanController>();
        _controller.OnShootClicked += Shoot;
    }

    void Shoot()
    {
        _weapon.TryShoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
