using System;

public interface IAmountHandler
{
    public event Action Emptied;
    public event Action ValueChanged;

    public float CurrentAmount { get; }
    public float MaxAmount { get; }
}