using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable, IFightStarter
{
    public const float WeaponSwitchTime = 2f;

    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private SliderView _healthView;
    [SerializeField] private SliderView _armorView;
    [SerializeField] private AttackCooldownView _view;
    [SerializeField] private Weapon _sword;
    [SerializeField] private Weapon _bow;
    [SerializeField] private PlayerEquipmentView _equipmentView;

    private Attacker _attacker;
    private IEnumerator _attackCoroutine;
    private Health _health;
    private Armor _armor;
    private GameUI _gameUI;
    private IDamagableObjectSpawner _damagableObjectSpawner;
    private IDamagable _target;
    private float _currentAttackSpeed;
    private bool _isSwitchingWeapon;
    private bool _swordEquiped;
    private bool _inAttackFase;
    
    public event Action FightStarted;
    public event Action FightStopped;

    private void Awake()
    {
        _attacker = new Attacker(_playerStats.AttackPower, _playerStats.AttackCooldown);
        _health = new Health(_playerStats.Health);
        _armor = new Armor(_playerStats.Armor);
        _healthView.SetAmountHandler(_health);
        _armorView.SetAmountHandler(_armor);
        _view.SetAttacker(_attacker);
        _view.SetAttackCooldown(_playerStats.AttackCooldown);
        EquipSword();
    }
    
    private void OnEnable()
    {
        _health.Emptied += StopCurrentAttackCoroutine;
        _equipmentView.SwordButtonClicked += SwitchWeapon;
        _equipmentView.BowButtonClicked += SwitchWeapon;
    }
    
    private void OnDisable()
    {
        _gameUI.HealButtonClicked -= _health.Increace;
        _gameUI.StartAttackButtonClicked -= StartFight;
        _gameUI.LeaveFightButtonClicked -= StopCurrentAttackCoroutine;
        _health.Emptied -= StopCurrentAttackCoroutine;
        _equipmentView.SwordButtonClicked -= SwitchWeapon;
        _equipmentView.BowButtonClicked -= SwitchWeapon;
    }
    
    public void SetDamagableObjectProvider(IDamagableObjectSpawner damagableObjectSpawner)
    {
        if (damagableObjectSpawner == null)
            throw new ArgumentNullException(nameof(damagableObjectSpawner));

        _damagableObjectSpawner = damagableObjectSpawner;
        _damagableObjectSpawner.NewDamagableObjectSpawned += SetTarget;
    }

    public void SetGameUI(GameUI gameUI)
    {
        if (gameUI == null)
            throw new ArgumentNullException(nameof(gameUI));

        _gameUI = gameUI;
        _gameUI.HealButtonClicked += _health.Increace;
        _gameUI.StartAttackButtonClicked += StartFight;
        _gameUI.LeaveFightButtonClicked += StopCurrentAttackCoroutine;
    }
    
    public void SwitchWeapon()
    {
        if (_isSwitchingWeapon)
            return;

        _attacker.PauseCooldown();
        _view.PauseCooldownProgress();

        _isSwitchingWeapon = true;
        StartCoroutine(SwitchWeaponCoroutine());
    }
    
    public void TakeDamage(float damage)
    {
        float damageToApply = _armor.GetReducedDamage(damage);

        if (damageToApply <= 0)
            return;

        _health.Decreace(damageToApply);
    }

    private void EquipSword()
    {
        _currentAttackSpeed = _sword.AttackSpeed;
        _swordEquiped = true;
        _equipmentView.ChangeImageToSword();
    }

    private void EquipBow()
    {
        _currentAttackSpeed = _bow.AttackSpeed;
        _swordEquiped = false;
        _equipmentView.ChangeImageToBow();
    }
    
    private void SetTarget(IDamagable target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        _target = target;
        
        if(_inAttackFase)
            ProcessAttack();
    }

    private void StartFight()
    {
        FightStarted?.Invoke();
        ProcessAttack();
        _inAttackFase = true;
    }

    private void ProcessAttack()
    {
        StopCurrentAttackCoroutine();
        _attackCoroutine = _attacker.AttackCoroutine(_target, _currentAttackSpeed);
        StartCoroutine(_attackCoroutine);
    }

    private void StopCurrentAttackCoroutine()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _inAttackFase = false;
            FightStopped?.Invoke();
        }
    }
    
    private IEnumerator SwitchWeaponCoroutine()
    {
        _equipmentView.StartFiillingProcess();
        yield return new WaitForSeconds(WeaponSwitchTime);

        if (_swordEquiped)
        {
            EquipBow();
        }
        else
        {
            EquipSword();
        }

        _isSwitchingWeapon = false;

        _attacker.ResumeCooldown();
        StartCoroutine(_attackCoroutine);
        _view.ResumeCooldownProgress();

        StartFight();
    }
}