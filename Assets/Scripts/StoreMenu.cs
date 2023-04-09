using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class StoreMenu : MonoBehaviour
{
    [SerializeField] GameObject Content;
    [SerializeField] GameObject LevelTag;
    [SerializeField] GameObject DeviceCom;
    [SerializeField] GameObject HBox;
    [SerializeField] float device_PosY = 65;
    [SerializeField] float device_PosX = 37;
    [SerializeField] float text_PosY = 40;
    [SerializeField] float text_PosX = -13.3f;
    [SerializeField] float start_PosY = 135.05f;

    // Start is called before the first frame update
    void Start()
    {
        List<Device> devices = new List<Device>();
        devices.AddRange(GameManager.instance.Devices.Values);


        LocalizedString str = new LocalizedString();
        str.TableReference = "LevelTable";

        GameObject tag = Instantiate(LevelTag, Content.transform);
        tag.GetComponent<RectTransform>().anchoredPosition = new Vector3(text_PosX, start_PosY, 0);
        str.TableEntryReference = 1 + "";
        tag.GetComponent<TextMeshProUGUI>().text = str.GetLocalizedString();
        float local = start_PosY - device_PosY;

        GameObject DeviceComponent;
        GameObject hbox = new GameObject();

        int level = 1;
        int left = 1;

        foreach(Device d in devices)
        {
            if (d.getLevel() > level)
            {
                left = 1;
                local -= text_PosY;
                level = d.getLevel();
                tag = Instantiate(LevelTag, Content.GetComponent<RectTransform>());
                str.TableEntryReference = level.ToString();
                tag.GetComponent<TextMeshProUGUI>().text = str.GetLocalizedString();
                local -= device_PosY;
            }
            if (left == 1)
            {
                hbox = Instantiate(HBox, Content.transform);
            }

            DeviceComponent = Instantiate(DeviceCom, hbox.transform);
            DeviceComponent.GetComponent<DeviceMenu>().Set(d.getName());
            left++;
            left %= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
