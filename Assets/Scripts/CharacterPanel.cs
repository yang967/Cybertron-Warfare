using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] TextMeshProUGUI text;

    string cname = "";

    public void Set(string name)
    {
        cname = name;
        Texture tex = Resources.Load("Image/" + name) as Texture;
        image.texture = tex;

        LocalizedString str = new LocalizedString();
        str.TableReference = "CharacterName";
        str.TableEntryReference = name;
        text.text = str.GetLocalizedString();
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
