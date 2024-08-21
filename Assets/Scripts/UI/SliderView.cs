using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderView : MonoBehaviour
{
    [SerializeField] private Slider _smoothSlider;
    
    private IAmountHandler _health;
    private float _changeSpeed = 10;

    private void OnDisable()
    {
        _health.ValueChanged -= StartSmoothChange;
    }

    public void SetAmountHandler(IAmountHandler amountHandler)
    {
        if (amountHandler == null)
            throw new ArgumentNullException(nameof(amountHandler));

        _health = amountHandler;
        _health.ValueChanged += StartSmoothChange;
        _smoothSlider.maxValue = _health.MaxAmount;
        _smoothSlider.value = _health.CurrentAmount;
    }

    private void StartSmoothChange()
    {
        StartCoroutine(ChangeSmoothSlider());
    }

    private IEnumerator ChangeSmoothSlider()
    {
        while (!Mathf.Approximately(_smoothSlider.value, _health.CurrentAmount))
        {
            _smoothSlider.value = Mathf.MoveTowards(_smoothSlider.value, _health.CurrentAmount, _changeSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
