using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform _playerSpawnPoint;

    public Transform PlayerSpawnPoint => _playerSpawnPoint;


    public int Score => _score;
    int _score = 0;

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
    }

    void Start()
    {
        Game.Instance.RespawnPacman();
    }
}
