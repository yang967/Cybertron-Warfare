using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailPanel : MonoBehaviour
{
    [SerializeField] Slider ExpBar;
    [SerializeField] Slider SpawnTimer;
    [SerializeField] TextMeshProUGUI currency;
    [SerializeField] TextMeshProUGUI SpawnTime;
    [SerializeField] TextMeshProUGUI Level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer.value = 1 - (GameManager.instance.GetSpawn() - Time.time) / GameManager.instance.getSpawnGap();
        SpawnTime.text = ((int)(GameManager.instance.GetSpawn() - Time.time) + 1) + " s";
    }

    public void SetExpBar(float value)
    {
        ExpBar.value = value;
    }

    public void SetCurrency(int c)
    {
        currency.text = "Currency: " + c + "";
    }

    public void SetLevel(int lvl)
    {
        Level.text = "lv. " + lvl;
    }
}
