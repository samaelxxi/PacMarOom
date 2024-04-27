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
        Debug.Log("Setup");
        _velocity = direction * _speed;
    }

    void Update()
    {
        _velocity += Physics.gravity * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Damageable damageable))
            damageable.TakeDamage();

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Damageable damageable))
            damageable.TakeDamage();

        Destroy(gameObject);
    }
}
