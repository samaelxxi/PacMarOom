using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : NPC
{
    [SerializeField] InvaderStats _stats;
    [SerializeField] InvaderProjectile _projectilePrefab;
    [SerializeField] Transform _shootPoint;


    Fsm.State _idleState;
    Fsm.State _patrolState;
    Fsm.State _chaseState;
    Fsm.State _attackState;

    protected override float RotationSpeed => _stats.RotationSpeed;
    protected override float Speed => 0;
    protected override float SightRange => _stats.SightRange;
    protected override float ForgetRange => _stats.ForgetRange;


    // Start is called before the first frame update
    void Start()
    {
        _health = _stats.Health;

        _idleState = Fsm_IdleState;
        _chaseState = Fsm_ChaseState;

        _fsm = new Fsm();
        _fsm.Start(_idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        _fsm.OnUpdate();
    }


    float _idleStateTimer;
    bool _idleMoveRight;
    void Fsm_IdleState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            _idleStateTimer = 0;
        }
        if (step == Fsm.Step.Update)
        {
            _idleStateTimer += Time.deltaTime;
            if (_idleStateTimer >= _stats.MoveCooldown)
            {
                transform.position = transform.position + _stats.MoveDist * (_idleMoveRight ? 1 : -1) * Vector3.right;
                _idleMoveRight = !_idleMoveRight;
                _idleStateTimer = 0;
            }
            if (_isAware)
                fsm.TransitionTo(_chaseState);
        }
    }


    float _chaseStateTimer;
    float _attackCooldownTimer;
    void Fsm_ChaseState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            _chaseStateTimer = 0;
            _attackCooldownTimer = 0;
        }

        if (step == Fsm.Step.Update)
        {
            _chaseStateTimer += Time.deltaTime;
            _attackCooldownTimer += Time.deltaTime;
            _targetPosition = Game.Instance.Pacman.transform.position;
            RotateTowardsTarget();
            if (_chaseStateTimer > _stats.MoveCooldown)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _stats.ForwardMoveDist);
                _chaseStateTimer = 0;
            }
            if (_attackCooldownTimer > _stats.AttackCooldown)
                Shoot();
        }
    }

    void Shoot()
    {
        var projectile = Instantiate(_projectilePrefab, _shootPoint.position, transform.rotation);
        projectile.Setup(transform.forward, _stats.ProjectileSpeed, _stats.Damage);
        _attackCooldownTimer = 0;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, _stats.SightRange);
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, _stats.ForgetRange);
    // }
}
