using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform _playerSpawnPoint;

    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    void Awake()
    {
        Game.Instance.SetLevel(this);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _playerSpawnPoint.position = checkpoint.position;
    }

    void Start()
    {
        Game.Instance.RespawnPacman();
    }
}
