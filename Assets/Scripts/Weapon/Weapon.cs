using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private float _attackSpeed;

    public float AttackSpeed => _attackSpeed;
}
