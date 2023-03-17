using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Device
{
    string name_;
    float damage_;
    float ignore_;
    float defend_;
    float crit_;
    float crit_damage_;
    float attack_rate_;
    float attack_range_;
    float view_range_;
    float speed_;
    float HP_;
    int price_;
    int slot_;
    float power_;
    float CoolDownRate_;
    List<string> component_;

    //Slots:
    //0: CPU
    //1: Software
    //2: Power-related Device
    //3: Head
    //4: Body
    //5: Hand
    //6: Foot

    public Device()
    {
        name_ = "";
        damage_ = 0;
        ignore_ = 0;
        defend_ = 0;
        crit_ = 0;
        crit_damage_ = 0;
        attack_rate_ = 0;
        attack_range_ = 0;
        component_ = new List<string>();
        HP_ = 0;
        price_ = 0;
        speed_ = 0;
        slot_ = -1;
        power_ = 0;
        CoolDownRate_ = 0;
    }

    public Device(string name, int slot, float damage, float ignore, float defend, float HP, float power, float crit, float crit_damage, float attack_rate, float attack_range, float view_range, float CDRate, float speed, int price)
    {
        name_ = name;
        damage_ = damage;
        ignore_ = ignore;
        defend_ = defend;
        crit_ = crit;
        crit_damage_ = crit_damage;
        attack_rate_ = attack_rate;
        component_ = new List<string>();
        HP_ = HP;
        price_ = price;
        slot_ = slot;
        attack_range_ = attack_range;
        view_range_ = view_range;
        speed_ = speed;
        power_ = power;
        CoolDownRate_ = CDRate;
    }

    public Device(string name, int slot, float damage, float ignore, float defend, float HP, float power, float crit, float crit_damage, float attack_rate, float attack_range, float view_range, float CDRate, float speed, int price, List<string> component)
    {
        name_ = name;
        damage_ = damage;
        ignore_ = ignore;
        defend_ = defend;
        crit_ = crit;
        crit_damage_ = crit_damage;
        attack_rate_ = attack_rate;
        component_ = component;
        HP_ = HP;
        price_ = price;
        slot_ = slot;
        attack_range_ = attack_range;
        view_range_ = view_range;
        speed_ = speed;
        power_ = power;
        CoolDownRate_ = CDRate;
    }

    public Device(string name, int slot, float defend, int price)
    {
        name_ = name;
        slot_ = slot;
        damage_ = 0;
        ignore_ = 0;
        crit_ = 1;
        crit_damage_ = 1;
        defend_ = defend;
        attack_rate_ = 1;
        component_ = new List<string>();
        HP_ = 0;
        price_ = price;
        speed_ = 0;
        view_range_ = 0;
        attack_range_ = 0;
        CoolDownRate_ = 1;
        power_ = 0;
    }

    public Device(Device rhs)
    {
        name_ = rhs.name_;
        slot_ = rhs.slot_;
        damage_ = rhs.damage_;
        ignore_ = rhs.ignore_;
        defend_ = rhs.defend_;
        crit_ = rhs.crit_;
        crit_damage_ = rhs.crit_damage_;
        attack_rate_ = rhs.attack_rate_;
        component_ = rhs.component_;
        HP_ = rhs.HP_;
        price_ = rhs.price_;
        speed_ = rhs.speed_;
        attack_range_ = rhs.attack_range_;
        view_range_ = rhs.view_range_;
        power_ = rhs.power_;
        CoolDownRate_ = rhs.CoolDownRate_;
    }

    public string getName()
    {
        return name_;
    }
    
    public int getSlot()
    {
        return slot_;
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

    public float getAttackRange()
    {
        return attack_range_;
    }

    public float getSpeed()
    {
        return speed_;
    }

    public float getViewRange()
    {
        return view_range_;
    }

    public float getPower()
    {
        return power_;
    }

    public float getCoolDownRate()
    {
        return CoolDownRate_;
    }
}
