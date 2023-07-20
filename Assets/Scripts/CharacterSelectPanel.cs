using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RawImage image;
    [SerializeField] GameObject str;

    int team;
    string cname;

    public void Set(int team, string name)
    {
        this.team = team;
        cname = name;
        str.GetComponent<LocalizeStringEvent>().StringReference.TableEntryReference = cname;

        Texture tex = Resources.Load(name + "Image") as Texture;
        if(tex != null )
            image.texture = tex;
    }

    public void OnClick()
    {
        Data.LastUsedCharacter = cname;
        Data.team = team;
        MainMenu.instance.RefreshCharacter(cname);
    }
}
