using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealing : MonoBehaviour
{
    [SerializeField] int team;

    List<GameObject> targets;

    private void Awake()
    {
        targets = new List<GameObject>();
    }

    private void Start()
    {
        StartCoroutine(effect());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6)
            return;

        GameObject obj = other.transform.parent.parent.gameObject;

        if (obj.GetComponent<PlayerControl>() != null && !targets.Contains(obj))
            targets.Add(obj);
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6)
            return;

        GameObject obj = other.transform.parent.parent.gameObject;

        if(obj.GetComponent<PlayerControl>() != null && targets.Contains(obj))
            targets.Remove(obj);
    }

    IEnumerator effect()
    {
        while(true)
        {
            yield return new WaitForSeconds(GameManager.HEAL_DELAY);

            foreach(GameObject obj in targets)
            {
                PlayerControl control = obj.GetComponent<PlayerControl>();
                if (control.getTeam() == team)
                    control.SetHP((int)(control.getCharacter().getMaxHP() * GameManager.HEAL_PROPORTION) * 10 + 2, 0);
                else
                    control.SetHP((int)(control.getCharacter().getMaxHP() * GameManager.HEAL_PROPORTION) * 10 + 1, 0);
            }
        }
    }
}
