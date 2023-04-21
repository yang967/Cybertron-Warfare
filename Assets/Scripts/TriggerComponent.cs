using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    public virtual void UpdateTrigger(GameObject obj = null) { }

    public virtual void EquipTrigger(GameObject obj = null) { }

    public virtual void UnequipTrigger(GameObject obj = null) { }

    public virtual void DeathTrigger(GameObject obj = null) { }

    public virtual void SkillTrigger(GameObject obj = null) { }

    public virtual void Skill1Trigger(GameObject obj = null) { }

    public virtual void Skill2Trigger(GameObject obj = null) { }

    public virtual void Skill3Trigger(GameObject obj = null) { }

    public virtual void Transform2VehicleTrigger(GameObject obj = null) { }

    public virtual void Transform2RoboTrigger(GameObject obj = null) { }

    public virtual void AttackTrigger(GameObject obj = null, GameObject target = null) { }

    public virtual void SetStats(TriggerComponent trigger) { }
}
