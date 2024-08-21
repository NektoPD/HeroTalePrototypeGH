using UnityEngine;

public class PlayerSpawner : MonoBehaviour, IDamagableObjectProvider, IFightStarterProvider
{
    [SerializeField] private Player _prefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameUI _gameUI;

    private IDamagable _player;
    private IFightStarter _fightStarter;
    
    private void Awake()
    {
        var player = Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
        player.SetDamagableObjectProvider(_enemySpawner);
        player.SetGameUI(_gameUI);
        _player = player;
        _fightStarter = player;
    }
    
    public IDamagable GetDamagableObject()
    {
        return _player;
    }

    public IFightStarter GetFightStarter()
    {
        return _fightStarter;
    }
}