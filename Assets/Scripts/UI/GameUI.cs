using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _startAttackButton;
    [SerializeField] private Button _leaveFightButton;
    [SerializeField] private Button _healButton;

    public event Action StartAttackButtonClicked;
    public event Action LeaveFightButtonClicked;
    public event Action HealButtonClicked;

    private void Start()
    {
        _startAttackButton.onClick.AddListener(ProcessAttackButtonClicked);
        _leaveFightButton.onClick.AddListener(ProcessLeaveFightButtonClicked);
        _healButton.onClick.AddListener(ProcessHealButtonClicked);
        _leaveFightButton.gameObject.SetActive(false);
    }

    private void ProcessAttackButtonClicked()
    {
        StartAttackButtonClicked?.Invoke();
        _startAttackButton.gameObject.SetActive(false);
        _leaveFightButton.gameObject.SetActive(true);
        _healButton.gameObject.SetActive(false);
    }

    private void ProcessLeaveFightButtonClicked()
    {
        LeaveFightButtonClicked?.Invoke();
        _leaveFightButton.gameObject.SetActive(false);
        _startAttackButton.gameObject.SetActive(true);
        _healButton.gameObject.SetActive(true);
    }

    private void ProcessHealButtonClicked()
    {
        HealButtonClicked?.Invoke();
    }
}
