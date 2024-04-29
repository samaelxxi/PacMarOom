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

    [SerializeField] GameObject _aliveMesh;
    [SerializeField] GameObject _deadMesh;


    Fsm.State _idleState;
    Fsm.State _patrolState;
    Fsm.State _chaseState;
    Fsm.State _attackState;
    Fsm.State _deadState;


    protected override int Health => _stats.Health;
    protected override float RotationSpeed => _stats.RotationSpeed;
    protected override float Speed => _stats.Speed;
    protected override float SightRange => _stats.SightRange;
    protected override float ForgetRange => _stats.ForgetRange;
    protected override float InvulnerableTime => _stats.InvulnerableTime;
    
    bool _isAware;
    bool _attackDamageDealt;
    bool _shouldPatrol;
    bool _isRevived;



    protected override void Awake()
    {
        base.Awake();

        _idleState = Fsm_IdleState;
        _patrolState = Fsm_PatrolState;
        _chaseState = Fsm_ChaseState;
        _attackState = Fsm_AttackState;
        _deadState = Fsm_DeadState;
        _fsm = new Fsm();
        _attackTrigger.SetTestPredicate((Collider other) => other.gameObject == Game.Instance.Pacman.gameObject);
        _attackTrigger.OnTriggerEnterEvent += DamagePacman;

        if (_patrolArea == null && gameObject.transform.parent != null &&
            gameObject.transform.parent.TryGetComponent(out BoxCollider boxCollider))
            _patrolArea = boxCollider;
        _shouldPatrol = _patrolArea != null;
    }

    void Start()
    {
        // Debug.Log($"{gameObject.name} Start");
        _health = _stats.Health;
        _fsm.Start(_idleState);
    }

    protected override void Update()
    {
        base.Update();
        _attackCooldownTimer -= Time.deltaTime;

        if (_isAware)
        {
            // Debug.Log($"{gameObject.name} Aware");
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

        if (!_isDead)
            Game.Instance.AudioManager.Play("ghostDamaged", pitch: UnityEngine.Random.Range(0.9f, 1.1f));
        else
            Game.Instance.AudioManager.Play("ghostDied", pitch: UnityEngine.Random.Range(0.9f, 1.1f));
    }


    protected override void Die()
    {
        Game.Instance.AddScore(_killScore);
        _isDead = true;
        _fsm.TransitionTo(_deadState);
    }

    public override void Reset()
    {
        _isAware = false;
        // Debug.Log($"Resetting NPC {gameObject.name}");
        if (_isDead)
            _isRevived = true;
        _attackTween?.Kill();
        _attackArea.gameObject.SetActive(false);
        base.Reset();
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
            if (_isRevived)
                fsm.TransitionTo(_idleState);
            else if (_isAware)
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
            if (!_isAware || _isRevived)
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
            if (_isRevived)
                fsm.TransitionTo(_idleState);

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


    Tween _deadTween;
    private void Fsm_DeadState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {

        if (step == Fsm.Step.Enter)
        {
            // Debug.Log($"{gameObject.name} Dead");
            _aliveMesh.SetActive(false);
            _deadMesh.SetActive(true);
            _deadMesh.transform.localPosition = Vector3.zero;
            _deadTween = _deadMesh.transform.DOLocalMoveY(10, 5).SetEase(Ease.InCubic).OnComplete(() => gameObject.SetActive(false));
            _isRevived = false;
        }
        else if (step == Fsm.Step.Update)
        {
            if (_isRevived)
                fsm.TransitionTo(_idleState);
        }
        else if (step == Fsm.Step.Exit)
        {
            // Debug.Log($"{gameObject.name} Revive");
            _deadTween.Kill();
            _deadMesh.SetActive(false);
            _aliveMesh.SetActive(true);
            _isRevived = false;
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
