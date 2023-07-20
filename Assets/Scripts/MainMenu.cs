using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    [SerializeField] GameObject CharacterPosition;
    [SerializeField] GameObject CharacterBar;
    [SerializeField] GameObject CharacterSkill;
    [SerializeField] GameObject CharacterDetail;

    [SerializeField] GameObject CharacterSkillBar;
    [SerializeField] GameObject CharacterDetailBar;

    List<Transformer> Autobot, Decepticon;
    List<Transformer> transformers;
    Dictionary<string, int> transformersDict;
    Dictionary<string, float> heights;

    private void Awake()
    {
        instance = this;

        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        if (Data.Language == -1) {
            Debug.Log("empty");
            int indx = 0;
            for (int i = 0; i < locales.Count; i++) {
                if (locales[i] == LocalizationSettings.SelectedLocale) {
                    indx = i;
                    break;
                }
            }
            Data.Language = indx;
        }
        else {
            StartCoroutine(SetLocale(Data.Language));
        }

        transformers = SaveSystem.LoadCh();
        transformersDict = SaveSystem.DecryptDictionary("TransformerDictionary.moba");

        Autobot = new List<Transformer>();
        Decepticon = new List<Transformer>();
        foreach (Transformer transformer in transformers) {
            if(transformer.getTeam() == 0)
                Autobot.Add(transformer);
            else if(transformer.getTeam() == 1)
                Decepticon.Add(transformer);
        }

        heights = SaveSystem.LoadHeights();
    }

    private void Start()
    {
        /*GameObject obj = Resources.Load(Data.LastUsedCharacter + "Model") as GameObject;
        GameObject model = Instantiate(obj, CharacterPosition.transform);*/
        RefreshCharacter(Data.LastUsedCharacter);
        //Destroy(model.GetComponent<PlayerCharacterControl>());
    }

    public void RefreshCharacter(string name)
    {
        GameObject obj = Resources.Load(name + "Model") as GameObject;
        if (obj == null)
            return;

        if (CharacterPosition.transform.childCount > 0)
            Destroy(CharacterPosition.transform.GetChild(0).gameObject);

        CharacterPosition.transform.position = new Vector3(CharacterPosition.transform.position.x, heights[name], CharacterPosition.transform.position.z);

        GameObject model = Instantiate(obj, CharacterPosition.transform);
        PlayerCharacterControl c = model.GetComponent<PlayerCharacterControl>();

        //TextMeshProUGUI text = CharacterSkill.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        RefreshSkillDescription(0, c);

        CharacterDetail.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Transformer.getCharacterDetail(name);
        //Destroy(model.GetComponent<PlayerCharacterControl>());
    }

    public void RefreshCharacterBar(int team = 0)
    {
        CharacterBar.GetComponent<CharacterBar>().Refresh(team);
    }

    public void ShowCharacter(string name)
    {
        RefreshCharacter(name);
        CharacterBar.GetComponent<Animator>().SetTrigger("Exit");
        CharacterSkillBar.GetComponent<Animator>().SetTrigger("Enter");
        CharacterDetailBar.GetComponent<Animator>().SetTrigger("Enter");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transformer getTransformer(string name)
    {
        return transformers[transformersDict[name]];
    }

    public List<Transformer> getAutobots()
    {
        return Autobot;
    }

    public List<Transformer> getDecepticon()
    {
        return Decepticon;
    }

    public void RefreshSkillDescription(int indx = 0, PlayerCharacterControl ctrl = null)
    {
        PlayerCharacterControl c;
        if (ctrl == null)
            c = CharacterPosition.transform.GetChild(0).GetComponent<PlayerCharacterControl>();
        else
            c = ctrl;
        CharacterSkill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = c.getSkillDescription(indx);
    }

    IEnumerator SetLocale(int i)
    {
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];

        if (SettingPanel.instance != null)
            SettingPanel.instance.OnValueChange(i);
    }
}
