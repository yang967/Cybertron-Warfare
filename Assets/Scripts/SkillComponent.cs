using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillComponent : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] GameObject TimerPanel;
    [SerializeField] GameObject ChargePanel;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI charges;
    float CD;
    int max_charge;
    int charge;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Set(string name, int max_charge, float CD)
    {
        TimerPanel.SetActive(false);
        this.max_charge = max_charge;
        if (max_charge == 1)
        {
            ChargePanel.SetActive(false);
        }
        time = 0;
        this.CD = CD;
        charge = max_charge;
    }

    public void SetCD(float CD)
    {
        float proportion = CD / this.CD;
        float left = time - Time.time;
        time = time - left + left * proportion;
        this.CD = CD;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerPanel.activeSelf)
        {
            if(time <= Time.time)
            {
                charge++;
                charges.text = charge + "";
                if (charge < max_charge)
                    time = Time.time + CD;
                else
                    TimerPanel.SetActive(false);
            }
            timer.text = ((int)(time - Time.time) + 1) + " s";
        }
    }

    public bool UseSkill()
    {
        if (charge <= 0)
            return false;

        time = Time.time + CD;
        TimerPanel.SetActive(true);
        charge--;
        charges.text = charge + "";
        return true;
    }

    public int getCharge()
    {
        return charge;
    }
}
