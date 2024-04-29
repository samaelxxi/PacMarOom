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
            Game.Instance.AudioManager.Play("ammo", pitch: UnityEngine.Random.Range(0.9f, 1.1f));
        }
    }

    // public override void Reset()
    // {
    //     Debug.Log("Resetting AmmoPickup");
    //     gameObject.SetActive(true);
    // }
}
