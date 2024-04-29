using System;
using System.Collections;
using System.Collections.Generic;
using CarterGames.Assets.AudioManager;
using DesignPatterns.Singleton;
using UnityEngine;

public class Game : Singleton<Game>
{
    public Pacman Pacman => _pacman;
    public AudioManager AudioManager => _audioManager;
    public Level Level => _level;

    public event Action OnPacmanRespawn;

    Pacman _pacman;
    HUD _hud;
    Level _level;
    EnemySpawner _enemySpawner;
    AudioManager _audioManager;

    public void SetPacman(Pacman pacman) => _pacman = pacman;
    public void SetHUD(HUD hud) => _hud = hud;
    public void SetLevel(Level level) => _level = level;

    public override void Awake()
    {
        base.Awake();
        Debug.Log("Game Awake");
        _enemySpawner = Resources.Load<EnemySpawner>("EnemySpawner");

        if (_audioManager == null)
        {
            Debug.Log("Loading audio manager");
            _audioManager = FindAnyObjectByType<AudioManager>();
            if (_audioManager == null)
            {
                _audioManager = Resources.Load<AudioManager>("AudioManager");
                _audioManager = Instantiate(_audioManager);
            }
            // AudioPool.Initialise(_audioManager.AudioManagerFile.soundPrefab, 10);
            DontDestroyOnLoad(_audioManager);
        }
    }

    public void Start()
    {
        _pacman.WierdWeapon.OnAmmoChanged += _hud.SetNewWieirdAmmo;
        _level.OnScoreChanged += _hud.SetNewScore;
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

    public void AddScore(int score)
    {
        _level.AddScore(score);
    }

    public void AddWierdAmmo(int ammo)
    {
        _pacman.WierdWeapon.AddAmmo(ammo);
    }

    public void SpawnEnemy(EnemyType type, Transform spawnPoint)
    {
        _enemySpawner.SpawnEnemy(type, spawnPoint.position);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _level.SetNewCheckpoint(checkpoint);
    }
}
