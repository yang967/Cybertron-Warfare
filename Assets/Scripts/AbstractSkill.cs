using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkill : MonoBehaviour, Skill
{
    protected Animator animator_;

    protected static float speed_;
    protected static float vehicle_speed_;

    public virtual bool Skill_1_init()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool Skill_2_init()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool Skill_3_init()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool Transform()
    {
        throw new System.NotImplementedException();
    }

    protected bool isIdleOrRun()
    {
        return isCurrentAnimationState("Idle") || isCurrentAnimationState("Run");
    }

    protected bool isCurrentAnimationState(string name)
    {
        return animator_.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(name);
    }

    public static float getSpeed()
    {
        return speed_;
    }

    public static float getVehicleSpeed()
    {
        return vehicle_speed_;
    }

    public virtual void dead() { }

    public virtual void respawn() { }
}
