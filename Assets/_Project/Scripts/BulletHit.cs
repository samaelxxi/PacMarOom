using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletHit : MonoBehaviour
{
    Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = Game.Instance.Pacman.transform;
        Material material = GetComponent<Renderer>().material;
        material.DOFloat(5, "_CurrentFrame", 1).OnComplete(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_transform);
    }
}
