using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] int team;
    [SerializeField] GameObject content;

    List<Transformer> transformers;

    // Start is called before the first frame update
    void Start()
    {
        if (team == 0)
            transformers = MainMenu.instance.getAutobots();
        else
            transformers = MainMenu.instance.getDecepticon();

        GameObject obj = Resources.Load("CharacterSelectPanel") as GameObject;
        foreach(Transformer t in transformers) {
            GameObject panel = Instantiate(obj, content.transform);
            panel.GetComponent<CharacterSelectPanel>().Set(team, t.getName());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
