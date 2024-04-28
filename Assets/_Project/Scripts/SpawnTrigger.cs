using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : Trigger
{
    [SerializeField] List<EnemyType> _enemyTypes;
    [SerializeField] List<Transform> _spawnPoints;


    protected override void OnTriggerEnter(Collider other)
    {
        if (_triggered && _onlyOnce)
            return;

        if ((_layerMask.value & 1 << other.gameObject.layer) != 0)
        {
            _onTriggerEnter.Invoke();
            SpawnEnemies();
            _triggered = true;
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < _enemyTypes.Count; i++)
        {
            SpawnEnemy(_enemyTypes[i], _spawnPoints[i]);
        }
    }
}
