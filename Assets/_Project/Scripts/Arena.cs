using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Arena : IResetable
{
    [System.Serializable]
    struct WaveObjects
    {
        public List<GameObject> _enemies;
        public UnityEvent _onWaveStart;
        public UnityEvent _onWaveComplete;
    }

    [SerializeField] List<WaveObjects> _waves = new();
    [SerializeField] UnityEvent _onArenaComplete;
    [SerializeField] bool _resetCompletionOnRespawn = true;

    int _currentWave;
    bool _waveInProgress;
    bool _isCompleted;

    void OnValidate()
    {
        foreach (var wave in _waves)
        {
            foreach (var enemy in wave._enemies)
            {
                if (enemy != null && !enemy.GetComponents<Component>().Any(comp => comp is IDeadable))
                {
                    Debug.LogError($"Enemy {enemy.name} does not have a component that implements IDeadable(NPCs or InvaderRow)");
                }
            }
        }
    }


    public void StartArena()
    {
        if (_isCompleted)
            return;

        _currentWave = 0;
        StartWave();
    }

    void StartWave()
    {
        _waveInProgress = true;
        _waves[_currentWave]._enemies.ForEach(enemy => enemy.gameObject.SetActive(true));
        _waves[_currentWave]._onWaveStart?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (_waveInProgress)
        {
            if (_waves[_currentWave]._enemies.All(enemy => (enemy.GetComponent<IDeadable>() as IDeadable).IsDead))
            {
                _waveInProgress = false;
                _waves[_currentWave]._onWaveComplete?.Invoke();
                _currentWave++;
                if (_currentWave < _waves.Count)
                {
                    StartWave();
                }
                else
                {
                    _isCompleted = true;
                    _onArenaComplete.Invoke();
                }
            }
        }
    }

    public override void Reset()
    {
        _currentWave = 0;
        _waveInProgress = false;
        if (_resetCompletionOnRespawn)
            _isCompleted = false;
    }
}
