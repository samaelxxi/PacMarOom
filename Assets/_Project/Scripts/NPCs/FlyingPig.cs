using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using System;

public class FlyingPig : MonoBehaviour
{
    [SerializeField] float _minSpeed = 2f;
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] float _fullReplenishTime = 1f;
    [SerializeField] float _speedReducePerSecond = 0.5f;
    [SerializeField] float _currentSpeed = 5;
    [SerializeField] SplineContainer _route;
    [SerializeField] float _sightRange = 5f;
    [SerializeField] Transform _mesh;
    [SerializeField] int _catchScore = 500;

    Transform _pacman;
    float _currentSplinePos = 0;
    int _direction = 1;

    Fsm _fsm = new();

    Fsm.State _idleState;
    Fsm.State _runState;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _maxSpeed;
        _pacman = Game.Instance.Pacman.transform;

        _idleState = Fsm_IdleState;
        _runState = Fsm_RunState;
        _fsm.Start(_idleState);
        _mesh.DOLocalMoveY(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        UpdatePosOnSpline();
    }

    void Update()
    {
        _fsm.OnUpdate();
    }


    void Fsm_IdleState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Update)
        {
            if (_currentSpeed < _maxSpeed)
            {
                _currentSpeed += (_maxSpeed - _minSpeed) * Time.deltaTime / _fullReplenishTime;
                if (_currentSpeed > _maxSpeed)
                    _currentSpeed = _maxSpeed;
            }
            if (Vector3.Distance(transform.position, _pacman.position) < _sightRange)
                fsm.TransitionTo(_runState);
        }
    }

    bool _noPlayer;
    float _noPlayerTimer = 0;
    void Fsm_RunState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            _noPlayer = false;
            ChooseDirection();
        }
        else if (step == Fsm.Step.Update)
        {
            if (_noPlayer)
            {
                _noPlayerTimer += Time.deltaTime;
                if (_noPlayerTimer > 2)
                    fsm.TransitionTo(_idleState);
                else
                    UpdatePosOnSpline();
            }
            else if (Vector3.Distance(transform.position, _pacman.position) > _sightRange)
            {
                _noPlayer = true;
                _noPlayerTimer = 0;
            }
            else
            {
                _currentSpeed -= _speedReducePerSecond * Time.deltaTime;
                _currentSpeed = Mathf.Max(_currentSpeed, _minSpeed);
                UpdatePosOnSpline();
            }
        }
    }

    private void UpdatePosOnSpline()
    {
        var splineLength = _route.Spline.GetLength();
        _currentSplinePos = (_currentSplinePos + _currentSpeed * _direction * Time.deltaTime) % splineLength;
        if (_currentSplinePos < 0)
            _currentSplinePos += splineLength;

        // most important part: evaluate currentPos on spline
        var normalizedPos = _currentSplinePos / splineLength;
        _route.Evaluate(normalizedPos, out var pos, out var dir, out var up);
        transform.SetPositionAndRotation(pos, Quaternion.LookRotation(dir * _direction, up));
    }

    void ChooseDirection()
    {
        var splineLength = _route.Spline.GetLength();
        var forwardPos  = (_currentSplinePos + _currentSpeed * Time.deltaTime) % splineLength;
        var backwardPos = (_currentSplinePos - _currentSpeed * Time.deltaTime) % splineLength;
        if (backwardPos < 0)
            backwardPos += splineLength;
        var forwardPosNormalized = forwardPos / splineLength;
        var backwardPosNormalized = backwardPos / splineLength;
        _route.Evaluate(forwardPosNormalized, out var pos1, out var dir1, out var up1);
        _route.Evaluate(backwardPosNormalized, out var pos2, out var dir2, out var up2);
        var forwardDist = Vector3.Distance(pos1, _pacman.position);
        var backwardDist = Vector3.Distance(pos2, _pacman.position);
        _direction = forwardDist > backwardDist ? 1 : -1;
    }


    public void BeCatched()
    {
        Destroy(gameObject);
        Game.Instance.AddScore(_catchScore);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
