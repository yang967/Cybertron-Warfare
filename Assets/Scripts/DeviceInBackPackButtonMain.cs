using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

public class DeviceInBackPackButtonMain : MonoBehaviour
{
    [SerializeField] GameObject text;

    bool shown = false;
    string DeviceName;

    public void Setup(string name)
    {
        DeviceName = name;

        Texture tex = Resources.Load(name) as Texture;

        if (tex != null)
            GetComponent<RawImage>().texture = tex;

        LocalizedString str = new LocalizedString();

        str.TableReference = "DeviceNameTable";
        str.TableEntryReference = DeviceName;

        text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str.GetLocalizedString();
        text.SetActive(false);
    }

    public void OnClick()
    {
        foreach (Transform t in transform)
            t.gameObject.SetActive(!shown);

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
}
