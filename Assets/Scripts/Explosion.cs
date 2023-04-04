using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    int damage_;
    float ignore_;
    int team_;
    List<GameObject> targets;
    float range_;
    bool include_constructure_;
    bool set_;

    private void Awake()
    {
        targets = new List<GameObject>();
        include_constructure_ = false;
        set_ = false;
    }

    public void Set(int damage, float ignore, int team, float range, bool include)
    {
        damage_ = damage;
        ignore_ = ignore;
        team_ = team;
        range_ = range;
        GetComponent<SphereCollider>().radius = range;
        include_constructure_ = include;
        set_ = true;
    }

    public List<GameObject> Explode()
    {
        if (!set_)
            return null;
        foreach (GameObject obj in targets)
        {
            if (obj.GetComponent<Control>() != null)
                obj.GetComponent<Control>().SetHP(damage_, ignore_);
            else
                obj.transform.GetChild(3).GetComponent<Turret>().SetHP(damage_, ignore_);
        }

        ExplodeEffect();

        return targets;
    }

    public void ExplodeEffect()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!set_)
          //  return;
        if (other.gameObject.layer == 6)
        {
            GameObject obj = other.transform.parent.parent.gameObject;
            if (obj.GetComponent<Control>().getTeam() != team_ && !targets.Contains(obj))
                targets.Add(obj);
        }


        if (other.GetComponent<TurretController>() != null)
            if (include_constructure_ && other.transform.GetChild(3).GetComponent<Turret>().getTeam() != team_ && !targets.Contains(other.gameObject))
                targets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!set_)
            return;
        if(other.gameObject.layer == 6)
        {
            if (targets.Contains(other.transform.parent.parent.gameObject))
                targets.Remove(other.transform.parent.parent.gameObject);
        }
        else if (targets.Contains(other.gameObject))
            targets.Remove(other.gameObject);
    }

}
