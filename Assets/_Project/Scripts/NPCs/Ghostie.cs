using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

[SelectionBase]
public class Ghostie : NPC
{
    [SerializeField] GhostieStats _stats;
    [SerializeField] BoxCollider _patrolArea;
    [SerializeField] BoxCollider _attackArea;
    [SerializeField] TriggerObserver _attackTrigger;
    [SerializeField] int _killScore = 100;


    Fsm.State _idleState;
    Fsm.State _patrolState;
    Fsm.State _chaseState;
    Fsm.State _attackState;


    protected override int Health => _stats.Health;
    protected override float RotationSpeed => _stats.RotationSpeed;
    protected override float Speed => _stats.Speed;
    protected override float SightRange => _stats.SightRange;
    protected override float ForgetRange => _stats.ForgetRange;
    protected override float InvulnerableTime => _stats.InvulnerableTime;
    
    bool _isAware;
    bool _attackDamageDealt;
    bool _shouldPatrol;



    void Start()
    {
        _health = _stats.Health;

        _idleState = Fsm_IdleState;
        _patrolState = Fsm_PatrolState;
        _chaseState = Fsm_ChaseState;
        _attackState = Fsm_AttackState;

        _fsm = new Fsm();
        _fsm.Start(_idleState);

        _attackTrigger.SetTestPredicate((Collider other) => other.gameObject == Game.Instance.Pacman.gameObject);
        _attackTrigger.OnTriggerEnterEvent += DamagePacman;

        if (_patrolArea == null && gameObject.transform.parent != null &&
            gameObject.transform.parent.TryGetComponent(out BoxCollider boxCollider))
            _patrolArea = boxCollider;
        _shouldPatrol = _patrolArea != null;
    }

    protected override void Update()
    {
        base.Update();
        _attackCooldownTimer -= Time.deltaTime;

        if (_isAware)
        {
            _awareTimer += Time.deltaTime;
            if (IsPlayerTooFar() && _awareTimer > 5)
                _isAware = false;
        }
        else if (IsPlayerInSight())
        {
            _isAware = true;
            _awareTimer = 0;
        }

        _fsm.OnUpdate();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _isAware = true;
    }

    protected override void Die()
    {
        Game.Instance.AddScore(_killScore);
        base.Die();
    }

    public override void Reset()
    {
        base.Reset();
        _isAware = false;
        // _fsm.TransitionTo(_idleState);
        _attackTween?.Kill();
    }

    void DamagePacman(Collider other)
    {
        if (_attackDamageDealt)
            return;
        other.GetComponent<Pacman>().GetDamage(_stats.Damage);
        _attackDamageDealt = true;
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
            if (_isAware)
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
            if (_isAware)
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
            if (!_isAware)
            {
                fsm.TransitionTo(_idleState);
                return;
            }

            Transform pacman = Game.Instance.Pacman.transform;
            _targetPosition = pacman.position;
            _targetPosition = pacman.position - TargetDirection.normalized;
            _targetPosition = _targetPosition.SetY(pacman.position.y);
            float dist = Vector3.Distance(transform.position, pacman.position);
            float dist2 = Vector3.Distance(transform.position, _targetPosition);
            if (Vector3.Distance(transform.position, _targetPosition) > _stats.AttackRange-1)
                MoveAndRotateTowardsTarget();
            else if (_attackCooldownTimer <= 0 && TargetAngle() < 10)
                fsm.TransitionTo(_attackState);
            else
                RotateTowardsTarget();
        }
    }

    float _attackStateTimer;
    float _attackCooldownTimer;
    Tween _attackTween;
    private void Fsm_AttackState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            _attackDamageDealt = false;
            _attackStateTimer = 0;
            _attackArea.gameObject.SetActive(true);
            _targetPosition = Game.Instance.Pacman.transform.position;
            Vector3 attackPos = transform.position + TargetDirection.normalized * _stats.AttackRange;
            _attackTween = transform.DOMove(attackPos, _stats.AttackDuration).SetEase(Ease.InOutCubic).SetDelay(_stats.AttackDelay);
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



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stats.SightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _stats.ForgetRange);

        Gizmos.color = Color.blue;
        if (_targetPosition != null)
            Gizmos.DrawWireSphere(_targetPosition, 0.5f);
    }
}
