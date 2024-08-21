using System;
using UnityEngine;

public class Armor : IAmountHandler
{
    private readonly float _maxAmount;
    private readonly float _minAmount;
    private float _currentAmount;

    public Armor(float maxAmount)
    {
        _maxAmount = maxAmount;
        _minAmount = 0f;
        _currentAmount = _maxAmount;
    }

    public event Action Emptied;
    public event Action ValueChanged;
    
    public float CurrentAmount => _currentAmount;
    public float MaxAmount => _maxAmount;

    public float GetReducedDamage(float damage)
    {
        if(_currentAmount <= _minAmount)
            return damage;

        float amountToReduce = Mathf.Min(_currentAmount, damage);
        _currentAmount -= amountToReduce;
        ValueChanged?.Invoke();
        
        if(_currentAmount <= _minAmount)
            Emptied?.Invoke();
        
        return damage - amountToReduce;
    }
}
