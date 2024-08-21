using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float _spawnChance;
    [SerializeField] private float _health;
    [SerializeField] private float _attackPower;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackSpeed;

    public float AttackSpeed => _attackSpeed;

    public float AttackCooldown => _attackCooldown;

    public float AttackPower => _attackPower;

    public float SpawnChance => _spawnChance;
    public float Health => _health;
}
