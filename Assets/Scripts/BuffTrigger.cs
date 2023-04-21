using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTrigger : TriggerComponent
{
    float time;
    float rate;
    string name_;
    float start_time;

    public void Set(string name, float time, float rate)
    {
        this.time = time;
        this.rate = rate;
        name_ = name;
        if(gameObject != null)
            GetComponent<PlayerCharacterControl>().AddTrigger(this);
    }

    public override void EquipTrigger(GameObject obj = null)
    {
        if (obj == null)
            return;
        PlayerControl control = obj.GetComponent<PlayerControl>();
        control.AddBuff(name_, rate);
        start_time = Time.time;
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(time);
        if(gameObject != null)
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
        Debug.Log("transfer");
        BuffTrigger t = (BuffTrigger)trigger;
        rate = t.rate;
        name_ = t.name_;
        time = Time.time - start_time;
        StartCoroutine(Stop());
    }
}
