using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Invader,
    BlueGhost,
    RedGhost,
    PinkGhost,
    OrangeGhost
}

[CreateAssetMenu(fileName = "EnemySpawner", menuName = "EnemySpawner")]
public class EnemySpawner : ScriptableObject
{
    [SerializeField] GameObject _invaderPrefab;
    [SerializeField] GameObject _blueGhostPrefab;
    [SerializeField] GameObject _redGhostPrefab;
    [SerializeField] GameObject _pinkGhostPrefab;
    [SerializeField] GameObject _orangeGhostPrefab;

    public GameObject SpawnEnemy(EnemyType enemyType, Vector3 position)
    {
        GameObject enemyPrefab = null;
        switch (enemyType)
        {
            case EnemyType.Invader:
                enemyPrefab = _invaderPrefab;
                break;
            case EnemyType.BlueGhost:
                enemyPrefab = _blueGhostPrefab;
                break;
            case EnemyType.RedGhost:
                enemyPrefab = _redGhostPrefab;
                break;
            case EnemyType.PinkGhost:
                enemyPrefab = _pinkGhostPrefab;
                break;
            case EnemyType.OrangeGhost:
                enemyPrefab = _orangeGhostPrefab;
                break;
        }

        return Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
