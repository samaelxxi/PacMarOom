using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WierdProjectile : MonoBehaviour
{
    [SerializeField] float _lifeTime = 5f;
    [SerializeField] BulletHit _bulletHit;

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
        // _velocity += Physics.gravity * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Damageable damageable))
            damageable.TakeDamage(_damage);

        Destroy(gameObject);

        Instantiate(_bulletHit, other.contacts[0].point, Quaternion.identity);
    }
}
