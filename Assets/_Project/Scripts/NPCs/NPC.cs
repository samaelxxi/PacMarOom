using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected Fsm _fsm;


    protected Vector3 _targetPosition;
    protected Vector3 TargetDirection => _targetPosition - transform.position;
    protected float TargetAngle => Vector3.Angle(transform.forward, TargetDirection);

    protected virtual float RotationSpeed => 0;
    protected virtual float Speed => 0;
    protected virtual float SightRange => 0;
    protected virtual float ForgetRange => 0;

    protected int _health;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log("Auch");
        _health -= damage;
        if (_health <= 0)
            Destroy(gameObject);
    }

    protected void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                             Quaternion.LookRotation(TargetDirection), 
                             RotationSpeed * Time.deltaTime);
    }

    protected void MoveAndRotateTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(TargetDirection), 
            RotationSpeed * Time.deltaTime);
        if (TargetAngle < 10)
            transform.position = Vector3.MoveTowards(transform.position, 
                transform.position + transform.forward, Speed * Time.deltaTime);
    }

    protected bool IsPlayerInSight()
    {
        return Vector3.Distance(transform.position, Game.Instance.Pacman.transform.position) < SightRange;
    }

    protected bool IsPlayerTooFar()
    {
        return Vector3.Distance(transform.position, Game.Instance.Pacman.transform.position) > ForgetRange;
    }
}
