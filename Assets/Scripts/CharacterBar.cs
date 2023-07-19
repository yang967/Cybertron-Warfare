using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBar : MonoBehaviour
{
    [SerializeField] Transform content;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh(int team = 0)
    {
        foreach(Transform t in content.transform) {
            Destroy(t.gameObject);
        }

        List<Transformer> lst = team == 0 ? MainMenu.instance.getAutobots() : MainMenu.instance.getDecepticon();
        GameObject obj = Resources.Load("CharacterPanel") as GameObject;

        foreach (Transformer transformer in lst) {
            GameObject panel = Instantiate(obj, content);
            panel.GetComponent<CharacterPanel>().Set(transformer.getName());
        }
    }
}
