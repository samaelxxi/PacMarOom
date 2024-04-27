using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WierdProjectile : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _lifeTime = 5f;

    Vector3 _velocity;

    void Start()
    {
        _velocity = transform.forward * _speed;
        Destroy(gameObject, _lifeTime);
    }

    public void Setup(Vector3 direction)
    {
        _velocity = direction * _speed;
    }

    void Update()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
