using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InvaderRow : IResetable, IDeadable
{
    [SerializeField] int _invaderCount = 5;
    [SerializeField] int _rowWidth = 10;
    [SerializeField] int _initialInvaderIndex = 0;
    [SerializeField] float _cellWidth = 1;
    [SerializeField] float _moveTime = 1;
    [SerializeField] float _averageShootTime = 1;
    [SerializeField] bool _initialDirectionIsRight = true;
    [SerializeField] Invader _firstInvader;

    List<Invader> _invaders = new();

    public bool ActiveAtStart { get; set; }
    public bool IsDead => _invaders.All(inv => inv.IsDead);

    float _moveTimer = 0;

    bool _currentDirectionIsRight;
    int _currentFirstInvaderIndex;
    int _currentLastInvaderIndex;
    bool _isRunning = false;

    void OnValidate()
    {
        _invaders.ForEach(inv => inv.SetShootTime(_averageShootTime));
        if (!_isRunning)
        {
            _invaderCount = Mathf.Min(_rowWidth, _invaderCount);

            _initialInvaderIndex = Mathf.Clamp(_initialInvaderIndex, 0, _rowWidth-_invaderCount);
            if (_firstInvader != null)
            {
                _firstInvader.transform.position = GetInvaderStartPosition(_initialInvaderIndex);
                _firstInvader.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void Start()
    {
        _isRunning = true;
        _invaders.Add(_firstInvader);
        for (int i = _initialInvaderIndex+1; i < _initialInvaderIndex + _invaderCount; i++)
        {
            Invader invader = Instantiate(_firstInvader, GetInvaderStartPosition(i), Quaternion.identity, transform);
            invader.transform.localRotation = Quaternion.identity;
            _invaders.Add(invader);
        }
        _invaders.ForEach(inv => inv.SetShootTime(_averageShootTime));
        _invaders.ForEach(inv => inv.PrepareShoot());
        _currentDirectionIsRight = _initialDirectionIsRight;
        _currentFirstInvaderIndex = _initialInvaderIndex;
        _currentLastInvaderIndex = _initialInvaderIndex + _invaderCount - 1;
        if (!_initialDirectionIsRight)
            _invaders.Reverse();
    }



    void Update()
    {
        _moveTimer += Time.deltaTime;
        if (_moveTimer > _moveTime)
        {
            MoveInvaders();
            _moveTimer = 0;
        }
    }

    void MoveInvaders()
    {
        if (_currentDirectionIsRight && _currentLastInvaderIndex == _rowWidth-1)
            _currentDirectionIsRight = false;
        else if (!_currentDirectionIsRight && _currentFirstInvaderIndex == 0)
            _currentDirectionIsRight = true;

        Vector3 displacement = _cellWidth * (_currentDirectionIsRight ? 1 : -1) * transform.right;
        foreach (var invader in _invaders)
        {
            invader.Move(displacement);
        }
        if (_currentDirectionIsRight)
        {
            _currentFirstInvaderIndex++;
            _currentLastInvaderIndex++;
        }
        else
        {
            _currentFirstInvaderIndex--;
            _currentLastInvaderIndex--;
        }

    }

    Vector3 GetInvaderStartPosition(int index)
    {
        index %= _rowWidth;
        return transform.position + transform.right * index * _cellWidth;
    }

    void OnDrawGizmosSelected()
    {
        bool moveRight = _initialDirectionIsRight;
        for (int i = _initialInvaderIndex; i < _rowWidth+_initialInvaderIndex; i++)
        {
            int j = i % _rowWidth;
            bool isInvader = i < _initialInvaderIndex + _invaderCount;
            Vector3 position = GetInvaderStartPosition(j);
            Gizmos.color = isInvader ? Color.red : Color.white;
            Gizmos.DrawWireCube(position, Vector3.one * _cellWidth);
        }
    }

    public override void Reset()
    {
        
    }
}
