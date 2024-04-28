using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderProjectile : MonoBehaviour
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
        transform.position += _velocity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Pacman pacman))
        {
            pacman.GetDamage(_damage);
        }

        Destroy(gameObject);
    }
}
