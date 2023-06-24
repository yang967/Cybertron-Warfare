using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    string name_;
    string description_;
    int type_;
    float rate_;
    float range_x_;
    float range_y_;
    bool isRoundRange_;
    float time_;
    int charge_;
    float CD_;
    List<Effect> effects_;
    float energy_;

    public Ability()
    {
        name_ = "Empty";
        description_ = "";
        type_ = -1;
        rate_ = 0;
        range_x_ = 0;
        range_y_ = 0;
        isRoundRange_ = false;
        time_ = 0;
        charge_ = 0;
        CD_ = 0;
        effects_ = null;
        energy_ = 0;
    }

    public Ability(string name, string description, int type, float rate, float range_x, float range_y, bool isRoundRange, float time, int charge, int CD, float energy)
    {
        name_ = name;
        description_ = description;
        type_ = type;
        rate_ = rate;
        range_x_ = range_x;
        range_y_ = range_y;
        isRoundRange_ = isRoundRange;
        time_ = time;
        charge_ = charge;
        CD_ = CD;
        effects_ = new List<Effect>();
        energy_ = energy;
    }

    public Ability(string name, string description, int type, float rate, float range_x, float range_y, bool isRoundRange, float time, int charge, int CD, float energy, List<Effect> effects)
    {
        name_ = name;
        description_ = description;
        type_ = type;
        rate_ = rate;
        range_x_ = range_x;
        range_y_ = range_y;
        isRoundRange_ = isRoundRange;
        time_ = time;
        charge_ = charge;
        CD_ = CD;
        effects_ = effects;
        energy_ = energy;
    }

    public Ability(Ability rhs)
    {
        name_ = rhs.getName();
        description_ = rhs.getDescription();
        type_ = rhs.getAbilityType();
        rate_ = rhs.getRate();
        range_x_ = rhs.getRangeX();
        range_y_ = rhs.getRangeY();
        isRoundRange_ = rhs.isRoundRange();
        time_ = rhs.getDuration();
        charge_ = rhs.getChargeNum();
        CD_ = rhs.getCD();
        effects_ = rhs.effects_;
        energy_ = rhs.energy_;
    }

    public string getName()
    {
        return name_;
    }

    public string getDescription()
    {
        return description_;
    }

    public float getRate()
    {
        return rate_;
    }

    public float getRangeX()
    {
        return range_x_;
    }

    public float getRangeY()
    {
        return range_y_;
    }

    public bool isRoundRange()
    {
        return isRoundRange_;
    }

    public float getDuration()
    {
        return time_;
    }

    public int getAbilityType()
    {
        return type_;
    }

    public int getChargeNum()
    {
        return charge_;
    }

    public float getCD()
    {
        return CD_;
    }

    public List<Effect> getEffects()
    {
        return effects_;
    }

    public float getEnergy()
    {
        return energy_;
    }
}
