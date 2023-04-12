using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceSwapButton : MonoBehaviour
{
    int indx;

    public void Setup(int indx)
    {
        this.indx = indx;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.instance.ToSwap1 = -1;
            GameManager.instance.SwapMode = false;
        }
    }

    public void OnClick()
    {
        GameManager.instance.ToSwap1 = indx;
        GameManager.instance.SwapMode = true;
    }

    public void OnDestroy()
    {
        GameManager.instance.ToSwap1 = -1;
        GameManager.instance.SwapMode = false;
    }
}
