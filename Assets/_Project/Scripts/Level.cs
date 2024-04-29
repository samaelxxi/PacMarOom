using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform _playerSpawnPoint;
    [SerializeField] string _worldName = "1-1";

    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    public event Action<int> OnScoreAdded;
    public event Action<int> OnPigsAdded;

    public int Score => _score;
    public string WorldName => _worldName;

    int _score = 0;
    int _pigs = 0;

    void Awake()
    {
        Game.Instance.SetLevel(this);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _playerSpawnPoint.position = checkpoint.position;
    }



    public void AddScore(int score)
    {
        _score += score;
        OnScoreAdded?.Invoke(_score);
    }

    public void CatchPig()
    {
        _pigs++;
        OnPigsAdded?.Invoke(_pigs);
    }

    void Start()
    {
        Game.Instance.RespawnPacman();
    }
}
