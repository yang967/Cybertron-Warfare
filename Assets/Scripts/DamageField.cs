using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    int damage;
    float ignore;
    int team;
    bool initialized = false;
    List<GameObject> targets;

    private void Awake()
    {
        targets = new List<GameObject>();
    }

    private void Update()
    {
        if (!initialized)
            return;

        foreach(GameObject obj in targets)
        {
            if (obj == null)
                continue;
            obj.GetComponent<Control>().SetHP(Mathf.RoundToInt(damage * Time.deltaTime) * 10, ignore * 5);
        }
    }

    public void Set(int damage, float ignore, float radius, int team)
    {
        this.damage = damage;
        this.ignore = ignore;

        GetComponent<SphereCollider>().radius = radius;
        this.team = team;
        initialized = true;
    }

    public void End()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6)
            return;
        if (other.transform.parent.parent.GetComponent<Control>().getTeam() == team)
            return;
        if (targets.Contains(other.transform.parent.parent.gameObject))
            return;

        targets.Add(other.transform.parent.parent.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6)
            return;
        if (!targets.Contains(other.transform.parent.parent.gameObject))
            return;

        targets.Remove(other.transform.parent.parent.gameObject);
    }
}
