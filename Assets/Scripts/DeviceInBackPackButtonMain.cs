using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

public class DeviceInBackPackButtonMain : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] DeviceSwapButton swap;
    [SerializeField] DeviceEquipButton equip;
    [SerializeField] DeviceSellButton sell;

    bool shown = false;
    string DeviceName;
    int indx;
    bool equiped;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && shown && !GameManager.instance.SwapMode)
        {
            SwitchChildButton();
        }
    }

    public void Setup(string name, int indx, bool equipped)
    {
        DeviceName = name;
        this.indx = indx;

        Texture tex = Resources.Load("Images/" + name) as Texture;

        if (tex != null)
            GetComponent<RawImage>().texture = tex;

        LocalizedString str = new LocalizedString();

        str.TableReference = "DeviceNameTable";
        str.TableEntryReference = DeviceName;

        int combined_indx = (equipped ? 0 : 1) * 10 + indx;

        swap.Setup(combined_indx);
        sell.Setup(combined_indx);
        equip.Setup(DeviceName, combined_indx);

        text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str.GetLocalizedString();

        foreach (Transform t in transform)
            t.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if(GameManager.instance.SwapMode && GameManager.instance.ToSwap1 != -1)
        {
            GameManager.instance.ToSwap2 = indx;

            GameManager.instance.Player.GetComponent<PlayerControl>().SwapDevice(GameManager.instance.ToSwap1, GameManager.instance.ToSwap2);

            GameManager.instance.ToSwap2 = -1;
            GameManager.instance.ToSwap1 = -1;
            GameManager.instance.SwapMode = false;
            return;
        }

        SwitchChildButton();
    }

    void SwitchChildButton()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
            transform.GetChild(i).gameObject.SetActive(!shown);
        GameManager.instance.MenuChildComponent = !shown;
        shown = !shown;
    }

    public void OnMouseOver()
    {
        text.SetActive(true);
    }

    public void OnMouseExit()
    {
        text.SetActive(false);
    }

    public void OnDestroy()
    {
        GameManager.instance.MenuChildComponent = false;
    }
}
