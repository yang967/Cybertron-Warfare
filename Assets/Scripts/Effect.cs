using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    int type_;
    string name_;
    float time_;
    float value_;

    public Effect()
    {
        type_ = -1;
        name_ = "";
        time_ = 0;
        value_ = 0;
    }

    public Effect(int type, string name, float time, float value)
    {
        type_ = type;
        name_ = name;
        time_ = time;
        value_ = value;
    }

    public Effect(Effect rhs)
    {
        type_ = rhs.type_;
        time_ = rhs.time_;
        value_ = rhs.value_;
    }

    public int getEffectType()
    {
        return type_;
    }

    public float getTime()
    {
        return time_;
    }

    public float getValue()
    {
        return value_;
    }

    public string getName()
    {
        return name_;
    }

    public bool Equals(Effect rhs)
    {
        return type_ == rhs.type_ && value_ == rhs.value_ && time_ == rhs.time_ && name_ == rhs.name_;
    }
}
