using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    protected float max, current;
    protected  RectTransform imageRectTransform;
    [SerializeField] protected Image fill;

    protected virtual void Awake()
    {
        max = 1;
        current = 1;
        fill.material = Instantiate(fill.material);
        imageRectTransform = fill.GetComponent<RectTransform>();
        //fill.material = Instantiate(fill.material);
        //fill.material.SetVector("_ImageSize", new Vector4(imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0));
    }

    public virtual void SetValue(float max, float current, float shield)
    {
        this.max = max;
        this.current = current;
        //Debug.Log(max / GameManager.HP_Per_Block);
        fill.material.SetFloat("_Blocks", max / GameManager.HP_Per_Block);
        fill.material.SetFloat("_Percentage", current / max);
        fill.material.SetFloat("_ShieldPercentage", shield / max);
    }
}
