using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamagable
{
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private SliderView _healthView;
    [SerializeField] private AttackCooldownView _attackCooldownView;

    private IDamagableObjectProvider _damagableObjectProvider;
    private IDamagable _target;
    private IFightStarterProvider _fightStarterProvider;
    private IFightStarter _fightStarter;
    private IEnumerator _attackCoroutine;
    private Health _health;
    private Attacker _attacker;

    public event Action<Enemy> Died;
    
    public float SpawnChance => _enemyStats.SpawnChance;

    private void Awake()
    {
        _health = new Health(_enemyStats.Health);
        _attacker = new Attacker(_enemyStats.AttackPower, _enemyStats.AttackCooldown);
        _healthView.SetAmountHandler(_health);
        _attackCooldownView.SetAttackCooldown(_enemyStats.AttackCooldown);
        _attackCooldownView.SetAttacker(_attacker);
    }

    private void OnDisable()
    {
        _fightStarter.FightStarted -= ProcessAttack;
        _fightStarter.FightStopped -= StopCurrentAttackCoroutine;
    }

    public void SetDamagableObjectProvider(IDamagableObjectProvider damagableObjectProvider)
    {
        if (damagableObjectProvider == null)
            throw new ArgumentNullException(nameof(damagableObjectProvider));

        _damagableObjectProvider = damagableObjectProvider;
        _target = _damagableObjectProvider.GetDamagableObject();
    }
    
    public void SetFightStarterProvider(IFightStarterProvider fightStarterProvider)
    {
        if (fightStarterProvider == null)
            throw new ArgumentNullException(nameof(fightStarterProvider));

        _fightStarterProvider = fightStarterProvider;
        _fightStarter = _fightStarterProvider.GetFightStarter();
        _fightStarter.FightStarted += ProcessAttack;
        _fightStarter.FightStopped += StopCurrentAttackCoroutine;
    }
    
    public void TakeDamage(float damage)
    {
        _health.Decreace(damage);
        _health.Emptied += () => Died?.Invoke(this);
    }
    
    private void ProcessAttack()
    {
        if (_target == null)
            throw new ArgumentNullException(nameof(_target));

        StopCurrentAttackCoroutine();
        _attackCoroutine = _attacker.AttackCoroutine(_target, _enemyStats.AttackSpeed);
        StartCoroutine(_attackCoroutine);
    }
    
    private void StopCurrentAttackCoroutine()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
    }
}
