using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceEquipButton : MonoBehaviour
{
    int indx;
    bool equipped;
    string DeviceName;

    public void Setup(string name, int indx)
    {
        DeviceName = name;
        this.indx = indx % 10;
        equipped = indx / 10 == 1 ? false : true;
    }

    public void OnClick()
    {
        PlayerControl control = GameManager.instance.Player.GetComponent<PlayerControl>();

        if(equipped)
        {
            control.UnequipDevice(indx);
        }
        else
        {
            control.EquipDevice(indx);
        }
    }
}
