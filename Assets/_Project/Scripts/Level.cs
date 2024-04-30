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
    public event Action<string> OnNewWorld;

    public int Score => _score;
    public string WorldName => _worldName;
    public int Pigs => _pigs;

    int _score = 0;
    int _pigs = 0;

    int _currentCheckpoint = 0;

    public int CheckPointAmmo { get; private set; }


    void Awake()
    {
        Game.Instance.SetLevel(this);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _playerSpawnPoint = checkpoint;
        _currentCheckpoint++;
        string worldName = $"{_currentCheckpoint/5 + 1}-{_currentCheckpoint%5}";
        if (worldName != _worldName)
        {
            _worldName = worldName;
            OnNewWorld?.Invoke(_worldName);
        }
        Game.Instance.AudioManager.Play("powerUp10");
        CheckPointAmmo = Game.Instance.Pacman.WierdWeapon.Ammo;
        Game.Instance.Pacman.Heal();
    }


    public void AddScore(int score)
    {
        _score += score;
        OnScoreAdded?.Invoke(_score);
    }


    List<int> _pigColors = new List<int>();
    public List<int> PigColors => _pigColors;
    public void CatchPig(int HatColor)
    {
        _pigs++;
        _pigColors.Add(HatColor);
        OnPigsAdded?.Invoke(_pigs);
    }

    void Start()
    {
        Game.Instance.InitPacmanOnLevel();
    }
}
