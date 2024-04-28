using System;
using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Singleton;
using UnityEngine;

public class Game : Singleton<Game>
{
    public Pacman Pacman => _pacman;

    public event Action OnPacmanRespawn;

    Pacman _pacman;
    HUD _hud;
    Level _level;

    EnemySpawner _enemySpawner;

    public void SetPacman(Pacman pacman) => _pacman = pacman;
    public void SetHUD(HUD hud) => _hud = hud;
    public void SetLevel(Level level) => _level = level;

    public override void Awake()
    {
        base.Awake();
        _enemySpawner = Resources.Load<EnemySpawner>("EnemySpawner");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void RespawnPacman()
    {
        if (_level == null)
        {
            Debug.LogError("Create Level and setup spawn point");
            return;
        }
        OnPacmanRespawn?.Invoke();
        _pacman.Respawn(_level.PlayerSpawnPoint);
    }

    public void SpawnEnemy(EnemyType type, Transform spawnPoint)
    {
        _enemySpawner.SpawnEnemy(type, spawnPoint.position);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _level.SetNewCheckpoint(checkpoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
