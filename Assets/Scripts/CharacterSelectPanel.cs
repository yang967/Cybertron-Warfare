using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RawImage image;

    int team;
    string cname;

    public void Set(int team, string name)
    {
        this.team = team;
        cname = name;
        LocalizedString str = new LocalizedString();
        str.TableReference = "CharacterName";
        str.TableEntryReference = cname;

        text.text = str.GetLocalizedString();

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
