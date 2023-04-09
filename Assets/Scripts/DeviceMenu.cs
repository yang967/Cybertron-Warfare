using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

public class DeviceMenu : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] LocalizedString str;

    Vector2 text_pos;

    bool initialized = false;
    string DeviceName;
    bool fuse = false;

    public void Set(string name)
    {
        str.TableReference = "DeviceNameTable";
        str.TableEntryReference = name;
        text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str.GetLocalizedString();
        text_pos = text.GetComponent<RectTransform>().anchoredPosition;
        Device d = GameManager.instance.getDevice(name);
        List<List<string>> stats = Device.getDifference(d);

        string stat = "";

        str.TableReference = "StatsTable";
        for(int i = 0; i < stats.Count - 1; i++)
        {
            if (stats[i][0] == "damage")
                Debug.Log(stats[i][1]);
            str.TableEntryReference = stats[i][0];
            stat += str.GetLocalizedString() + ": " + stats[i][1] + "<br>";
        }

        text.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stat;

        str.TableEntryReference = "component";
        string com = str.GetLocalizedString() + "; <br>";

        str.TableReference = "DeviceNameTable";
        if(stats[stats.Count - 1].Count != 0)
        {
            List<string> components = stats[stats.Count - 1];
            for(int i = 0; i < components.Count; i++)
            {
                str.TableEntryReference = components[i];
                com += str.GetLocalizedString() + "<br>";
            }

            text.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = com;
        }

        Texture tex = Resources.Load(name + "Image") as Texture;
        if (tex != null)
            GetComponent<RawImage>().texture = tex;
        text.SetActive(false);
        DeviceName = name;

        fuse = d.getMergeComponent().Count != 0;

        initialized = true;
    }

    public void OnMouseOver()
    {
        if (!initialized)
            return;
        text.SetActive(true);
        text.GetComponent<RectTransform>().SetParent(GameManager.instance.Canvas.transform);
    }

    public void OnMouseExit()
    {
        if (!initialized)
            return;
        text.GetComponent<RectTransform>().SetParent(transform);
        text.GetComponent<RectTransform>().anchoredPosition = text_pos;
        text.SetActive(false);
    }

    public void OnClick()
    {
        if(!fuse)
        {
            GameManager.instance.Player.GetComponent<PlayerControl>().BuyDevice(DeviceName);
            return;
        }
        GameManager.instance.FuseMenu.GetComponent<FuseMenu>().Setup(DeviceName);
    }
}
