using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadedOverloader : TriggerComponent
{

    public override void AttackTrigger(GameObject obj = null, GameObject target = null)
    {
        if (obj == null)
            return;

        obj.GetComponent<PlayerControl>().SetEnergy(100);
    }
}
