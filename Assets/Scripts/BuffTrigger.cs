using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTrigger : TriggerComponent
{
    float time;
    float rate;
    string name_;

    public void Set(string name, float time, float rate)
    {
        this.time = time;
        this.rate = rate;
        name_ = name;
        GetComponent<PlayerCharacterControl>().AddTrigger(this);
    }

    public override void EquipTrigger(GameObject obj = null)
    {
        if (obj == null)
            return;
        PlayerControl control = obj.GetComponent<PlayerControl>();
        control.AddBuff(name_, rate);
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(time);
        GetComponent<PlayerCharacterControl>().RemoveTrigger(this);
    }

    public override void UnequipTrigger(GameObject obj = null)
    {
        if (obj == null)
            return;
        PlayerControl control = obj.GetComponent<PlayerControl>();
        control.AddBuff(name_, 1 / rate);
        Destroy(this);
    }

    public override void DeathTrigger(GameObject obj = null)
    {
        UnequipTrigger();
    }

    public override void SetStats(TriggerComponent trigger)
    {
        if (trigger.GetType() != typeof(BuffTrigger))
            return;
        BuffTrigger t = (BuffTrigger)trigger;
        time = t.time;
        rate = t.rate;
        name_ = t.name_;
    }
}
