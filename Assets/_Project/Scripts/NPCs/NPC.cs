using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected Fsm _fsm;


    protected Vector3 _targetPosition;
    protected Vector3 TargetDirection => _targetPosition - transform.position;

    protected virtual float RotationSpeed => 0;
    protected virtual float Speed => 0;
    protected virtual float SightRange => 0;
    protected virtual float ForgetRange => 0;
    protected virtual float InvulnerableTime => 1;

    protected int _health;


    protected float _awareTimer;

    protected float _invulnerableTimer;

    MeshRenderer _meshRenderer;


    protected virtual void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected virtual void Update()
    {
        if (_invulnerableTimer > 0)
            _invulnerableTimer -= Time.deltaTime;
    }

    public virtual void TakeDamage(int damage)
    {
        if (_invulnerableTimer > 0)
            return;
        Debug.Log("Auch");
        _health -= damage;
        if (_health <= 0)
            Die();
        else
            BecomeInvulnerable(InvulnerableTime);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void BecomeInvulnerable(float duration)
    {
        _invulnerableTimer = duration;
        _meshRenderer.material.SetFloat("_IsDamagedFloat", 1);
        this.InSeconds(duration, () => _meshRenderer.material.SetFloat("_IsDamagedFloat", 0));
    }

    protected void RotateTowardsTarget()
    {
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, 
                             Quaternion.LookRotation(TargetDirection), 
                             RotationSpeed * Time.deltaTime);
        newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
        transform.rotation = newRotation;
    }

    protected float TargetAngle()
    {
        Vector3 targetDirection = TargetDirection;
        targetDirection.y = 0; // Ignore vertical difference with target

        Vector3 npcForward = transform.forward;
        npcForward.y = 0; // Ignore vertical orientation of the NPC

        float angle = Vector3.Angle(npcForward, targetDirection);

        return Mathf.Abs(angle);
    }


    protected void MoveAndRotateTowardsTarget()
    {
        RotateTowardsTarget();
        if (TargetAngle() < 10)
        {
            Vector3 targetPos = transform.position + TargetDirection.normalized;

            transform.position = Vector3.MoveTowards(transform.position, 
                targetPos, Speed * Time.deltaTime);
        }
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
