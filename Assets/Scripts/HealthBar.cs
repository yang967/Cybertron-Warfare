using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    private RectTransform imageRectTransform;
    public Image fill;

    private void Awake()
    {
        imageRectTransform = fill.GetComponent<RectTransform>();
        //fill.material = Instantiate(fill.material);
        fill.material.SetVector("_ImageSize", new Vector4(imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0));
    }

    public void SetValue(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(float max, float current)
    {
        fill.material.SetFloat("_Steps", max / GameManager.HP_Per_Block);
        slider.value = current / max;
    }
}
