using System;

public interface IFightStarter
{
    public event Action FightStarted;
    public event Action FightStopped;
}