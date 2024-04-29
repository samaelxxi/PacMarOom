using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int _ammo = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Game.Instance.AddWierdAmmo(_ammo);
            gameObject.SetActive(false);
        }
    }

    // public override void Reset()
    // {
    //     Debug.Log("Resetting AmmoPickup");
    //     gameObject.SetActive(true);
    // }
}
