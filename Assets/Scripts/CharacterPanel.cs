using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject str;

    string cname = "";

    public void Set(string name)
    {
        cname = name;
        Texture tex = Resources.Load("Image/" + name) as Texture;
        image.texture = tex;

        str.GetComponent<LocalizeStringEvent>().StringReference.TableEntryReference = name;
    }

    public void OnClick()
    {
        if (cname == "")
            return;

        if(MainMenu.instance == null)
            return;

        MainMenu.instance.ShowCharacter(cname);
    }
}
