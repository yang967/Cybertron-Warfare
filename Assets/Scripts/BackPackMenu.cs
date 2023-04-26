using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackMenu : MonoBehaviour
{
    [SerializeField] GameObject bag;
    [SerializeField] GameObject device;
    [SerializeField] Color color;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {

        for (int i = 0; i < 7; i++) {
            if (transform.GetChild(i + 1).childCount > 0)
                Destroy(transform.GetChild(i + 1).GetChild(0).gameObject);
        }

        for(int i = 0; i < 7; i++)
            if (bag.transform.GetChild(i).childCount > 0)
                Destroy(bag.transform.GetChild(i).GetChild(0).gameObject);

        List<string> devices = GameManager.instance.Player.GetComponent<PlayerControl>().getDevice();
        List<string> backpack = GameManager.instance.Player.GetComponent<PlayerControl>().getBackPack();

        for(int i = 0; i < 7; i++)
        {
            if (devices[i] == "")
            {
                if (transform.GetChild(i + 1).GetComponent<RawImage>() != null)
                    continue;
                transform.GetChild(i + 1).gameObject.AddComponent<RawImage>();
                transform.GetChild(i + 1).GetComponent<RawImage>().color = color;
                transform.GetChild(i + 1).GetComponent<RawImage>().raycastTarget = false;
                continue;
            }

            if (transform.GetChild(i + 1).TryGetComponent<RawImage>(out RawImage image))
                Destroy(image);

            GameObject d = Instantiate(device, transform.GetChild(i + 1));
            d.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            d.GetComponent<DeviceInBackPackButtonMain>().Setup(devices[i], i, true);

        }

        for (int i = 0; i < 7; i++)
        {
            if (backpack[i] == "")
            {
                if (bag.transform.GetChild(i).GetComponent<RawImage>() != null)
                    continue;
                bag.transform.GetChild(i).gameObject.AddComponent<RawImage>();
                bag.transform.GetChild(i).GetComponent<RawImage>().color = color;
                continue;
            }

            GameObject d = Instantiate(device, bag.transform.GetChild(i));
            d.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            d.GetComponent<DeviceInBackPackButtonMain>().Setup(backpack[i], i, false);
        }
    }
}
