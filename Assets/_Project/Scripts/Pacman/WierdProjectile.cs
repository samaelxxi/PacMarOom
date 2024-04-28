using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WierdProjectile : MonoBehaviour
{
    [SerializeField] float _lifeTime = 5f;

    Vector3 _velocity;
    int _damage;

    void Start()
    {
        // _velocity = transform.forward * _speed;
        Destroy(gameObject, _lifeTime);
    }

    public void Setup(Vector3 direction, float speed, int damage)
    {
        _velocity = direction * speed;
        _damage = damage;
    }

    void Update()
    {
        _velocity += Physics.gravity * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Hit1 {other.gameObject.name}");
        if (other.gameObject.TryGetComponent(out Damageable damageable))
            damageable.TakeDamage(_damage);

        Destroy(gameObject);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"Hit2 {other.name}");
    //     if (other.gameObject.TryGetComponent(out Damageable damageable))
    //         damageable.TakeDamage(_damage);

    //     Destroy(gameObject);
    // }
}
