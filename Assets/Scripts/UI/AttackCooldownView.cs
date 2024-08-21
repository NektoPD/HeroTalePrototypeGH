using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AttackCooldownView : MonoBehaviour
{
    [SerializeField] private Image _attackPreparation;
    [SerializeField] private Image _attackingProcess;
    
    private float _attackCooldown;
    private float _elapsedTime;
    private Attacker _attacker;
    private Coroutine _cooldownCoroutine;

    private void Start()
    {
        _attackPreparation.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _attacker.CooldownBegan -= StartCooldownProgress;
        _attacker.AttackBegan -= ActivateAttackProcess;
    }
    
    public void PauseCooldownProgress()
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }
    }

    public void ResumeCooldownProgress()
    {
        if (_cooldownCoroutine == null && _elapsedTime < _attackCooldown)
        {
            _cooldownCoroutine = StartCoroutine(StartProgress());
        }
    }
    
    public void SetAttacker(Attacker attacker)
    {
        if (attacker == null)
            throw new ArgumentNullException(nameof(attacker));

        _attacker = attacker;
        _attacker.CooldownBegan += StartCooldownProgress;
        _attacker.AttackBegan += ActivateAttackProcess;
    }

    public void SetAttackCooldown(float attackCooldown)
    {
        _attackCooldown = attackCooldown;
    }

    private void ActivateAttackProcess()
    {
        _attackingProcess.gameObject.SetActive(true);
    }

    private void StartCooldownProgress()
    {
        _attackingProcess.gameObject.SetActive(false);
        _attackPreparation.gameObject.SetActive(true);
        _attackPreparation.fillAmount = 0f;

        _elapsedTime = 0f;

        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
        }

        _cooldownCoroutine = StartCoroutine(StartProgress());
    }
    
    private IEnumerator StartProgress()
    {
        while (_elapsedTime < _attackCooldown)
        {
            _elapsedTime += Time.deltaTime;
            _attackPreparation.fillAmount = Mathf.Clamp01(_elapsedTime / _attackCooldown);
            yield return null;
        }
        
        _attackPreparation.gameObject.SetActive(false);
        _cooldownCoroutine = null;
    }
}
