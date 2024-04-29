using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    [SerializeField] int _score = 10;

    public int Score => _score;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Game.Instance.GetCollectible(this);
            Destroy(gameObject);
            Game.Instance.AudioManager.Play("collectible", pitch: UnityEngine.Random.Range(0.9f, 1.1f));
        }
    }
}
