using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Instruction
{
    int type_;
    Vector3 target_;
    GameObject target_obj_;

    public Instruction(int type, Vector3 target)
    {
        type_ = type;
        target_ = target;
        target_obj_ = null;
    }

    public Instruction(int type, Vector3 target, GameObject target_obj)
    {
        type_ = type;
        target_ = target;
        target_obj_ = target_obj;
    }

    public Instruction(int type, GameObject target_obj)
    {
        type_ = type;
        target_obj_ = target_obj;
    }

    public int getInstructionType()
    {
        return type_;
    }

    public Vector3 getTarget()
    {
        return target_;
    }

    public GameObject getTargetObject()
    {
        return target_obj_;
    }

    public bool Equals(Instruction other)
    {
        if (other == null)
            return false;
        if (other.type_ == type_ &&
            other.target_.Equals(target_) &&
            other.target_obj_.Equals(target_obj_))
            return true;
        return false;
    }
}
