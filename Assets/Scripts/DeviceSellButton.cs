using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceSellButton : MonoBehaviour
{
    int indx;

    public void Setup(int indx)
    {
        this.indx = indx;
    }

    public void OnClick()
    {
        GameManager.instance.Player.GetComponent<PlayerControl>().SellDevice(indx);
    }
}
