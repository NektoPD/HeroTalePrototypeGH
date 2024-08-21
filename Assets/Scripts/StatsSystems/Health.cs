using System;

public class Health : IAmountHandler
{
    private readonly float _maxAmount;
    private readonly float _minAmount;
    private float _currentAmount;

    public Health(float maxAmount)
    {
        _maxAmount = maxAmount;
        _minAmount = 0f;
        _currentAmount = _maxAmount;
    }

    public event Action Emptied;
    public event Action ValueChanged;

    public float CurrentAmount => _currentAmount;
    public float MaxAmount => _maxAmount;

    public void Increace()
    {
        _currentAmount = _maxAmount;
        ValueChanged?.Invoke();
    }

    public void Decreace(float amount)
    {
        if (_currentAmount > _minAmount)
        {
            _currentAmount -= amount;
            ValueChanged?.Invoke();
        }
        else if (_currentAmount <= _minAmount)
        {
            Emptied?.Invoke();
        }
    }
}