using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarNonPlayer : HealthBar
{
    Slider slider;
    [SerializeField] Gradient f;

    protected override void Awake()
    {
        slider = transform.GetChild(0).GetComponent<Slider>();
        slider.value = 1;
    }

    public override void SetValue(float max, float current, float shield)
    {
        slider.value = current / max;
    }

}
