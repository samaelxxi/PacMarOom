using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

[SelectionBase]
public class Ghostie : MonoBehaviour
{
    [SerializeField] GhostieStats _stats;
    [SerializeField] bool _shouldPatrol;
    [SerializeField] BoxCollider _patrolArea;
    [SerializeField] BoxCollider _attackArea;

    Fsm _fsm;

    Fsm.State _idleState;
    Fsm.State _patrolState;
    Fsm.State _chaseState;
    Fsm.State _attackState;

    Vector3 _targetPosition;
    int _health;

    Vector3 TargetDirection => _targetPosition - transform.position;
    float TargetAngle => Vector3.Angle(transform.forward, TargetDirection);

    void Start()
    {
        _health = _stats.Health;

        _idleState = Fsm_IdleState;
        _patrolState = Fsm_PatrolState;
        _chaseState = Fsm_ChaseState;
        _attackState = Fsm_AttackState;

        _fsm = new Fsm();
        _fsm.Start(_idleState);
    }

    void Update()
    {
        _fsm.OnUpdate();
        _attackCooldownTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Auch");
        _health -= damage;
        if (_health <= 0)
            Destroy(gameObject);
    }

    float _idleStateTimer;
    float _idleWaitTime;
    void Fsm_IdleState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            _idleStateTimer = 0;
            _idleWaitTime = UnityEngine.Random.Range(1, 3);
        }
        else if (step == Fsm.Step.Update)
        {
            _idleStateTimer += Time.deltaTime;
            if (IsPlayerInSight())
                fsm.TransitionTo(_chaseState);
            else if (_shouldPatrol && _idleStateTimer > _idleWaitTime)
                fsm.TransitionTo(_patrolState);
        }
    }

    private void Fsm_PatrolState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            FindNewPatrolPosition();
        }
        else if (step == Fsm.Step.Update)
        {
            if (IsPlayerInSight())
                fsm.TransitionTo(_chaseState);
            else if (Vector3.Distance(transform.position, _targetPosition) < 0.5f)
                fsm.TransitionTo(_idleState);
            else
                MoveAndRotateTowardsTarget();
        }
    }

    void FindNewPatrolPosition()
    {
        _targetPosition = transform.position;

        int i = 0;  // safety check to avoid infinite loop
        while (Vector3.Distance(transform.position, _targetPosition) < 2 && i < 100)
        {
            _targetPosition = new Vector3(UnityEngine.Random.Range(_patrolArea.bounds.min.x, _patrolArea.bounds.max.x),
                                          UnityEngine.Random.Range(_patrolArea.bounds.min.y, _patrolArea.bounds.max.y),
                                          UnityEngine.Random.Range(_patrolArea.bounds.min.z, _patrolArea.bounds.max.z));
            i++;
        }
    }

    private void Fsm_ChaseState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Update)
        {
            if (IsPlayerTooFar())
            {
                fsm.TransitionTo(_idleState);
                return;
            }

            _targetPosition = Game.Instance.Pacman.transform.position;

            if (Vector3.Distance(transform.position, _targetPosition) > _stats.AttackRange)
                MoveAndRotateTowardsTarget();
            else if (_attackCooldownTimer <= 0 && TargetAngle < 10)
                fsm.TransitionTo(_attackState);
            else
                RotateTowardsTarget();
        }
    }

    float _attackStateTimer;
    float _attackCooldownTimer;
    private void Fsm_AttackState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            _attackStateTimer = 0;
            _attackArea.gameObject.SetActive(true);
            transform.DOMove(_targetPosition, _stats.AttackDuration).SetEase(Ease.InOutCubic).SetDelay(_stats.AttackDelay);
        }
        else if (step == Fsm.Step.Update)
        {
            _attackStateTimer += Time.deltaTime;
            if (_attackStateTimer > _stats.AttackDuration + _stats.AttackDelay)
                fsm.TransitionTo(_chaseState);
        }
        else if (step == Fsm.Step.Exit)
        {
            _attackCooldownTimer = _stats.AttackCooldown;
            _attackArea.gameObject.SetActive(false);
        }
    }



    void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                             Quaternion.LookRotation(TargetDirection), 
                             _stats.RotationSpeed * Time.deltaTime);
    }

    void MoveAndRotateTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(TargetDirection), 
            _stats.RotationSpeed * Time.deltaTime);
        if (TargetAngle < 10)
            transform.position = Vector3.MoveTowards(transform.position, 
                transform.position + transform.forward, _stats.Speed * Time.deltaTime);
    }

    bool IsPlayerInSight()
    {
        return Vector3.Distance(transform.position, Game.Instance.Pacman.transform.position) < _stats.SightRange;
    }

    bool IsPlayerTooFar()
    {
        return Vector3.Distance(transform.position, Game.Instance.Pacman.transform.position) > _stats.ForgetRange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stats.SightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _stats.ForgetRange);
    }
}
