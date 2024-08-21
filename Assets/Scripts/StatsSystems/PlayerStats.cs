using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float _armor;
    [SerializeField] private float _health;
    [SerializeField] private float _attackPower;
    [SerializeField] private float _attackCooldown;
    
    public float Armor => _armor;

    public float Health => _health;

    public float AttackPower => _attackPower;

    public float AttackCooldown => _attackCooldown;
}
