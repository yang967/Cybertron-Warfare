using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : TriggerComponent
{
    int damage;
    int ignore;

    public void Set(int damage, int ignore)
    {
        this.damage = damage;
        this.ignore = ignore;
    }

    public override void UpdateTrigger(GameObject obj = null)
    {
        obj.GetComponent<PlayerControl>().SetHP(damage, ignore);
    }

    public override void SetStats(TriggerComponent trigger)
    {
        if (trigger.GetType() != typeof(DamageTrigger))
            return;

        DamageTrigger t = (DamageTrigger)trigger;

        damage = t.damage;
        ignore = t.ignore;
    }
}
