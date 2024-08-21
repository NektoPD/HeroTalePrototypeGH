using System;
using System.Collections;
using UnityEngine;

public class Attacker
{
    private readonly float _attackPower;
    private readonly float _attackCooldown;
    private float _cooldownTimer;
    private bool _canAttack;
    private bool _isCooldownPaused;
    
    public Attacker(float attackPower, float attackCooldown)
    {
        _attackPower = attackPower;
        _attackCooldown = attackCooldown;
        _canAttack = true;
    }

    public event Action CooldownBegan;
    public event Action AttackBegan;
    
    public IEnumerator AttackCoroutine(IDamagable target, float attackSpeed)
    {
        WaitForSeconds weaponAttackSpeed = new WaitForSeconds(attackSpeed);

        while (true)
        {
            if (_canAttack)
            {
                Attack(target);
                yield return weaponAttackSpeed;
                StartCooldown();
            }
            else
            {
                if (_cooldownTimer > 0)
                {
                    while (_cooldownTimer > 0)
                    {
                        if (!_isCooldownPaused)
                        {
                            _cooldownTimer -= Time.deltaTime;
                        }
                        yield return null;
                    }

                    _canAttack = true;
                }
            }
            
            yield return null;
        }
    }
    
    public void PauseCooldown()
    {
        _isCooldownPaused = true;
    }

    public void ResumeCooldown()
    {
        _isCooldownPaused = false;
    }
    
    private void Attack(IDamagable target)
    {
        AttackBegan?.Invoke();
        target.TakeDamage(_attackPower);
    }
    
    private void StartCooldown()
    {
        CooldownBegan?.Invoke();
        _canAttack = false;
        _cooldownTimer = _attackCooldown;
    }
}