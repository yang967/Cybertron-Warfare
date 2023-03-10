using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    List<GameObject> targets;
    bool include_constructure_;
    int damage_;
    float ignore_;
    int team_;
    GameObject from;

    private void Awake()
    {
        targets = new List<GameObject>();
        include_constructure_ = false;
    }

    public void Set(int damage, float ignore, int team, float range, bool IncludeConstructure, GameObject from = null)
    {
        damage_ = damage;
        Debug.Log(damage);
        ignore_ = ignore;
        team_ = team;
        GetComponent<SphereCollider>().radius = range;
        include_constructure_ = IncludeConstructure;
        this.from = from;
    }

    public void Damage()
    {
        bool killed;
        foreach(GameObject obj in targets)
        {
            killed = false;
            if (obj.layer == 6)
                killed = obj.GetComponent<Control>().SetHP(damage_, ignore_);
            else if (obj.GetComponent<PlayerBase>() != null)
                obj.GetComponent<PlayerBase>().setHP(damage_, ignore_);
            else
                killed = obj.transform.GetChild(3).GetComponent<Turret>().SetHP(damage_, ignore_);
            if (killed)
                from.GetComponent<PlayerControl>().AddExp(obj.GetComponent<Control>() != null ? obj.GetComponent<Control>().getLevel() : -2, 0);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            GameObject obj = other.transform.parent.parent.gameObject;
            if (obj.GetComponent<Control>().getTeam() != team_ && !targets.Contains(obj))
                targets.Add(obj);
        }
            
        
        if(other.GetComponent<TurretController>() != null)
            if (include_constructure_ && other.transform.GetChild(3).GetComponent<Turret>().getTeam() != team_ && !targets.Contains(other.gameObject))
                targets.Add(other.gameObject);

        if (other.GetComponent<PlayerBase>() != null)
            if (include_constructure_ && other.GetComponent<PlayerBase>().getTeam() != team_ && !targets.Contains(other.gameObject))
                targets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.gameObject))
            targets.Remove(other.gameObject);
        else if (other.gameObject.layer == 6)
            if (targets.Contains(other.transform.parent.parent.gameObject))
                targets.Remove(other.transform.parent.parent.gameObject);
    }
}
