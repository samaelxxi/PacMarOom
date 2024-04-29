using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    [SerializeField] int _score = 10;

    public int Score => _score;

    void Start()
    {
        transform.DOLocalMoveY(0.3f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 45 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Game.Instance.GetCollectible(this);
            Destroy(gameObject);
        }
    }
}
