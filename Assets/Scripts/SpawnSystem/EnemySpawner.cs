using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour,IDamagableObjectProvider,IDamagableObjectSpawner
{
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PlayerSpawner _playerSpawner;

    private EnemyType[] _enemyTypes;
    private float _totalSpawnChance;
    private IDamagable _currentEnemy;
    
    public event Action<IDamagable> NewDamagableObjectSpawned;

    private void Start()
    {
        _enemyTypes = Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().ToArray();
        _totalSpawnChance = _enemyTypes.Sum(type => _enemyFactory.GetEnemy(type).SpawnChance);
        SpawnEnemy();
    }
    
    public IDamagable GetDamagableObject()
    {
        return _currentEnemy;
    }

    private void SpawnEnemy()
    {
        float randomValue = Random.Range(0, _totalSpawnChance);
        float enemySpawnChance = 0f;

        foreach (EnemyType type in _enemyTypes)
        {
            Enemy enemy = _enemyFactory.GetEnemy(type);
            if (enemy == null)
                continue;

            enemySpawnChance += enemy.SpawnChance;

            if (randomValue <= enemySpawnChance)
            {
                var spawnedEnemy = Instantiate(enemy, _spawnPoint.position, Quaternion.identity);
                spawnedEnemy.SetDamagableObjectProvider(_playerSpawner);
                spawnedEnemy.SetFightStarterProvider(_playerSpawner);
                spawnedEnemy.Died += ProcessEnemyDeath;
                NewDamagableObjectSpawned?.Invoke(spawnedEnemy);
                _currentEnemy = spawnedEnemy;
                break;
            }
        }
    }

    private void ProcessEnemyDeath(Enemy enemy)
    {
        enemy.Died -= ProcessEnemyDeath;
        _currentEnemy = null;
        Destroy(enemy.gameObject);
        SpawnEnemy();
    }
}