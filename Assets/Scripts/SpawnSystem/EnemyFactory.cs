using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy _zombiePrefab;
    [SerializeField] private Enemy _skeletonPrefab;
    [SerializeField] private Enemy _witchPrefab;
    [SerializeField] private Enemy _graveMonsterPrefab;
    
    public Enemy GetEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Zombie:
                 return _zombiePrefab;
            case EnemyType.Skeleton:
                return _skeletonPrefab;
            case EnemyType.Witch:
                return _witchPrefab;
            case EnemyType.GraveMonster:
                return _graveMonsterPrefab;
            default:
                return null;
        }
    }
}

public enum EnemyType
{
    Zombie,
    Skeleton,
    Witch,
    GraveMonster
}
