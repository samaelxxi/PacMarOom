using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Debug.Log(_audioManager.AudioManagerFile);
            Debug.Log(_audioManager.AudioManagerFile.soundPrefab);
            AudioPool.Initialise(_audioManager.AudioManagerFile.soundPrefab, 10);
            DontDestroyOnLoad(_audioManager);
        }
    }

    public void Start()
    {
        CollectResetables();
        Debug.Log(_pacman);
        Debug.Log(_pacman.WierdWeapon);
        Debug.Log(_hud);
        _pacman.WierdWeapon.OnAmmoChanged += _hud.SetNewWieirdAmmo;
        _pacman.WierdWeapon.OnAmmoAdded += _hud.GetBonus;
        _level.OnScoreAdded += _hud.SetNewScore;
        _level.OnPigsAdded += _hud.SetNewPigs;
        _level.OnPigsAdded += (i) => _hud.GetPig();
        _level.OnNewWorld += _hud.SetNewWorld;
        _pacman.OnHealthChanged += _hud.SetNewHealth;
        _pacman.OnDamaged += _hud.GetDamaged;
        _pacman.OnInvulnerable += _hud.OnInvulnerable;
        _pacman.OnVulnerable += _hud.OnVulnerable;
        Game.Instance.AudioManager.Play("PACMAROOM", loop: true, volume: 0.5f);
    }

    public void CollectResetables()
    {
        var startResetables = FindObjectsOfType<IResetable>(true);
        foreach (var resetable in startResetables)
        {
            bool isActive = resetable.gameObject.activeSelf;
            resetable.ActiveOnStart = isActive;
            if (!isActive)
            {
                resetable.gameObject.SetActive(true);  // call awake and stuff
                resetable.gameObject.SetActive(false);
            }
        }
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
        
        var resetables = FindObjectsOfType<IResetable>(true);
        foreach (var resetable in resetables)
        {
            Debug.Log($"Resetting {resetable.gameObject.name}");
            resetable.gameObject.SetActive(true);
            resetable.Reset();
            resetable.gameObject.SetActive(resetable.ActiveOnStart);
        }
    }

    public void AddScore(int score)
    {
        _level.AddScore(score);
    }

    public void GetCollectible(Collectible collectible)
    {
        _hud.GetBonus();
        AddScore(collectible.Score);
    }

    public void AddWierdAmmo(int ammo)
    {
        _pacman.WierdWeapon.AddAmmo(ammo);
    }

    public void CatchPig()
    {
        _level.CatchPig();
    }

    public void SpawnEnemy(EnemyType type, Transform spawnPoint)
    {
        _enemySpawner.SpawnEnemy(type, spawnPoint.position);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _level.SetNewCheckpoint(checkpoint);
    }

    public void DamageAndTeleportPacman()
    {
        _pacman.GetDamage(1);
        _pacman.Teleport(_level.PlayerSpawnPoint);
    }

    public void InitPacmanOnLevel()
    {
        _pacman.Teleport(_level.PlayerSpawnPoint);
    }
}
