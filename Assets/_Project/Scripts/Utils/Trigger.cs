using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] protected UnityEvent _onTriggerEnter;
    [SerializeField] protected bool _onlyOnce;
    [SerializeField] protected bool _resetOnRespawn;

    [SerializeField] protected LayerMask _layerMask;

    protected bool _triggered;

    protected virtual void Start()
    {
        if (_resetOnRespawn)
            Game.Instance.OnPacmanRespawn += () => _triggered = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_triggered && _onlyOnce)
            return;

        if ((_layerMask.value & 1 << other.gameObject.layer) != 0)
        {
            _onTriggerEnter.Invoke();
            _triggered = true;
        }
    }

    public void PacmanRespawn()
    {
        Game.Instance.RespawnPacman();
    }

    public void DamageAndTeleportPacman()
    {
        Game.Instance.DamageAndTeleportPacman();
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        Game.Instance.SetNewCheckpoint(checkpoint);
    }

    public void SpawnEnemy(EnemyType type, Transform spawnPoint)
    {
        Game.Instance.SpawnEnemy(type, spawnPoint);
    }
}
