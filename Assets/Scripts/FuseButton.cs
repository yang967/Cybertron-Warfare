using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuseButton : MonoBehaviour
{
    string DeviceName;

    public void Setup(string name)
    {
        DeviceName = name;
        Texture tex = Resources.Load(name) as Texture;
        if (tex != null)
            GetComponent<RawImage>().texture = tex;
    }

    public void OnClick()
    {
        GameManager.instance.Player.GetComponent<PlayerControl>().BuyDevice(DeviceName);
    }
}
