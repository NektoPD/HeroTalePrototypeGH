using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipmentView : MonoBehaviour
{
    [SerializeField] private Button _swordButton;
    [SerializeField] private Button _bowButton;
    [SerializeField] private SpriteRenderer _swordImage;
    [SerializeField] private SpriteRenderer _bowImage;
    [SerializeField] private Image _changingProcess;

    public event Action BowButtonClicked;
    public event Action SwordButtonClicked;

    private void Awake()
    {
        _swordButton.onClick.AddListener(ProcessSwordButtonClick);
        _bowButton.onClick.AddListener(ProcessBowButtonClick);
        _swordButton.interactable = false;
        _changingProcess.gameObject.SetActive(false);
    }

    public void ChangeImageToSword()
    {
        _swordImage.gameObject.SetActive(true);
        _bowImage.gameObject.SetActive(false);
    }

    public void ChangeImageToBow()
    {
        _bowImage.gameObject.SetActive(true);
        _swordImage.gameObject.SetActive(false);
    }

    public void StartFiillingProcess()
    {
        _changingProcess.gameObject.SetActive(true);
        StartCoroutine(StartProgress());
    }
    
    private void ProcessSwordButtonClick()
    {
        SwordButtonClicked?.Invoke();
        _bowButton.interactable = true;
        _swordButton.interactable = false;
    }
    
    private void ProcessBowButtonClick()
    {
        BowButtonClicked?.Invoke();
        _bowButton.interactable = false;
        _swordButton.interactable = true;
    }
    
    private IEnumerator StartProgress()
    {
        float elapsedTime = 0f;
        float weaponSwitchTime = Player.WeaponSwitchTime;
        
        while (elapsedTime < weaponSwitchTime)
        {
            elapsedTime += Time.deltaTime;
            _changingProcess.fillAmount = Mathf.Clamp01(elapsedTime/weaponSwitchTime);
            yield return null;
        }

        _changingProcess.gameObject.SetActive(false);
    }
}
