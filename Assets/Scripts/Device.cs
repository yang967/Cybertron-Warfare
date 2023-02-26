using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device
{
    string name_;
    string description_;
    float damage_;
    float ignore_;
    float defend_;
    float crit_;
    float crit_damage_;
    float attack_rate_;
    float HP_;
    int price_;
    List<string> component_;

    public Device()
    {
        name_ = "";
        description_ = "";
        damage_ = 0;
        ignore_ = 0;
        defend_ = 0;
        crit_ = 0;
        crit_damage_ = 0;
        attack_rate_ = 0;
        component_ = new List<string>();
        HP_ = 0;
        price_ = 0;
    }

    public Device(string name, string description, float damage, float ignore, float defend, float HP, float crit, float crit_damage, float attack_rate, int price)
    {
        name_ = name;
        description_ = description;
        damage_ = damage;
        ignore_ = ignore;
        defend_ = defend;
        crit_ = crit;
        crit_damage_ = crit_damage;
        attack_rate_ = attack_rate;
        component_ = new List<string>();
        HP_ = HP;
        price_ = price;
    }

    public Device(string name, string description, float damage, float ignore, float defend, float HP, float crit, float crit_damage, float attack_rate, int price, List<string> component)
    {
        name_ = name;
        description_ = description;
        damage_ = damage;
        ignore_ = ignore;
        defend_ = defend;
        crit_ = crit;
        crit_damage_ = crit_damage;
        attack_rate_ = attack_rate;
        component_ = component;
        HP_ = HP;
        price_ = price;
    }

    public Device(string name, string description, float defend, int price)
    {
        name_ = name;
        description_ = description;
        damage_ = 0;
        ignore_ = 0;
        crit_ = 1;
        crit_damage_ = 1;
        defend_ = defend;
        attack_rate_ = 1;
        component_ = new List<string>();
        HP_ = 0;
        price_ = price;
    }

    public Device(Device rhs)
    {
        name_ = rhs.name_;
        description_ = rhs.description_;
        damage_ = rhs.damage_;
        ignore_ = rhs.ignore_;
        defend_ = rhs.defend_;
        crit_ = rhs.crit_;
        crit_damage_ = rhs.crit_damage_;
        attack_rate_ = rhs.attack_rate_;
        component_ = rhs.component_;
        HP_ = rhs.HP_;
        price_ = rhs.price_;
    }

    public string getName()
    {
        return name_;
    }

    public string getDescription()
    {
        return description_;
    }

    public float getDamage()
    {
        return damage_;
    }

    public float getDefendIgnore()
    {
        return ignore_;
    }

    public float getDefend()
    {
        return defend_;
    }

    public float getCriticalRate()
    {
        return crit_;
    }

    public float getCriticalDamage()
    {
        return crit_damage_;
    }

    public float getAttackRate()
    {
        return attack_rate_;
    }

    public List<string> getMergeComponent()
    {
        return component_;
    }

    public float getHP()
    {
        return HP_;
    }

    public int getPrice()
    {
        return price_;
    }
}
