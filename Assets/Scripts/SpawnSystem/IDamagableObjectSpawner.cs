using System;

public interface IDamagableObjectSpawner
{
    public event Action<IDamagable> NewDamagableObjectSpawned;
}