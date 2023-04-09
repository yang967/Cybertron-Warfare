using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuseMenu : MonoBehaviour
{
    [SerializeField] GameObject fuse;
    [SerializeField] GameObject device_menu;

    public void Setup(string name)
    {
        Device device = GameManager.instance.getDevice(name);
        if (device.getMergeComponent().Count == 0)
            return;

        fuse.GetComponent<FuseButton>().Setup(name);

        for(int i = 0; i < 2; i++)
        {
            GameObject d = Instantiate(device_menu, transform.GetChild(i));
            d.GetComponent<DeviceMenu>().Set(device.getMergeComponent()[i]);
            d.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        GameManager.instance.getFuse();
    }
}
