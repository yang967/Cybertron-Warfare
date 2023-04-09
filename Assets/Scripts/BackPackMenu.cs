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

        List<string> devices = GameManager.instance.Player.GetComponent<PlayerControl>().getDevice();
        List<string> backpack = GameManager.instance.Player.GetComponent<PlayerControl>().getBackPack();

        for(int i = 0; i < 7; i++)
        {
            if (devices[i] == "")
            {
                if (transform.GetChild(i).GetComponent<RawImage>() != null)
                    continue;
                transform.GetChild(i).gameObject.AddComponent<RawImage>();
                transform.GetChild(i).GetComponent<RawImage>().color = color;
                continue;
            }

            if (transform.GetChild(i).TryGetComponent<RawImage>(out RawImage image))
                Destroy(image);

            GameObject d = Instantiate(device, transform.GetChild(i));
            d.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            d.GetComponent<DeviceInBackPackButtonMain>().Setup(name);

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
            d.GetComponent<DeviceInBackPackButtonMain>().Setup(name);
        }
    }
}
