using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Unity.VisualScripting;

public class SettingPanel : MonoBehaviour, MenuCall
{
    [SerializeField] TMP_Dropdown language;

    public static SettingPanel instance;

    List<string> languages;
    List<string> options;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LocalizationSettings setting = LocalizationSettings.Instance;

        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        options = new List<string>();
        languages = new List<string>();

        int i = 0, indx = 0;
        string current = LocalizationSettings.SelectedLocale.ToString();
        LocalizedString str = new LocalizedString();
        str.TableReference = "Languages";
        foreach (Locale locale in locales) {
            str.TableEntryReference = locale.ToString();
            options.Add(str.GetLocalizedString());
            languages.Add(str.GetLocalizedString());
            if (locale.ToString() == current)
                indx = i;
            i++;
        }

        string tmp = options[indx];
        options[indx] = options[0];
        options[0] = tmp;

        language.ClearOptions();
        language.AddOptions(options);
    }

    IEnumerator setLocales(int locale_id)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[locale_id];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChange(TMP_Dropdown dropdown)
    {
        /*int indx = languages.IndexOf(options[dropdown.value]);
        options = new List<string>(languages);
        string tmp = options[0];
        options[0] = options[indx];
        options[indx] = tmp;
        language.ClearOptions();
        language.AddOptions(options);
        StartCoroutine(setLocales(indx));*/
        OnValueChange(dropdown.value);
    }

    public void OnValueChange(int i)
    {
        int indx = languages.IndexOf(options[i]);
        options = new List<string>(languages);
        string tmp = options[0];
        options[0] = options[indx];
        options[indx] = tmp;
        language.ClearOptions();
        language.AddOptions(options);
        StartCoroutine(setLocales(indx));

        Data.Language = i;
    }

    public void OnClick()
    {
        
    }
}
