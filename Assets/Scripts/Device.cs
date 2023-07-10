using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Device
{
    string name_;
    int level_;
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
    float SkillDamage_;
    List<string> component_;
    bool hasTrigger;

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
        crit_ = 1;
        crit_damage_ = 1;
        attack_rate_ = 1;
        attack_range_ = 0;
        component_ = new List<string>();
        HP_ = 0;
        price_ = 0;
        speed_ = 0;
        slot_ = -1;
        power_ = 0;
        CoolDownRate_ = 1;
        level_ = 0;
        SkillDamage_ = 1;
        hasTrigger = false;
    }

    public Device(string name, int slot, int level, float damage, float ignore, float defend, float HP, float power, float crit, float crit_damage, float attack_rate, float attack_range, float view_range, float CDRate, float speed, float SkillDamage, bool hasTrigger, int price)
    {
        name_ = name;
        damage_ = damage;
        level_ = level;
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
        SkillDamage_ = SkillDamage;
        this.hasTrigger = hasTrigger;
    }

    public Device(string name, int slot, int level, float damage, float ignore, float defend, float HP, float power, float crit, float crit_damage, float attack_rate, float attack_range, float view_range, float CDRate, float speed, float SkillDamage, bool hasTrigger, int price, List<string> component)
    {
        name_ = name;
        level_ = level;
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
        SkillDamage_ = SkillDamage;
        this.hasTrigger = hasTrigger;
    }

    public Device(string name, int slot, int level, float defend, int price)
    {
        name_ = name;
        slot_ = slot;
        level_ = level;
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
        SkillDamage_ = 1;
        hasTrigger = false;
    }

    public Device(Device rhs)
    {
        name_ = rhs.name_;
        slot_ = rhs.slot_;
        level_ = rhs.level_;
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
        SkillDamage_ = rhs.SkillDamage_;
        hasTrigger = rhs.hasTrigger;
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

    public int getLevel()
    {
        return level_;
    }

    public float getSkillDamage()
    {
        return SkillDamage_;
    }

    public bool HasTrigger()
    {
        return hasTrigger;
    }

    public static List<List<string>> getDifference(Device rhs)
    {
        Device d = new Device();
        List<List<string>> result = new List<List<string>>();

        result.Add(new List<string> { "price", rhs.price_.ToString() });

        if (rhs.damage_ != d.damage_)
        {
            result.Add(new List<string> { "damage", (rhs.damage_ - d.damage_ > 0 ? "" : "+") + (rhs.damage_ - d.damage_).ToString() });
        }

        if (rhs.ignore_ != d.ignore_)
            result.Add(new List<string> { "ignore", (rhs.ignore_ - d.ignore_ > 0 ? "" : "+") + (rhs.ignore_ - d.ignore_).ToString() });

        if (rhs.defend_ != d.defend_)
            result.Add(new List<string> { "defend", (rhs.defend_ - d.defend_ > 0 ? "" : "+") + (rhs.defend_ - d.defend_).ToString() });

        if (rhs.crit_ != d.crit_)
            result.Add(new List<string> { "crit", (rhs.crit_ - d.crit_ > 0 ? "" : "+") + ((rhs.crit_ - d.crit_) * 100).ToString() + "%" });

        if (rhs.crit_damage_ != d.crit_damage_)
            result.Add(new List<string> { "crit damage", (rhs.crit_damage_ - d.crit_damage_ > 0 ? "" : "+") + ((rhs.crit_damage_ - d.crit_damage_) * 100).ToString() + "%" });

        if (rhs.attack_rate_ != d.attack_rate_)
            result.Add(new List<string> { "attack rate", (rhs.attack_rate_ - d.attack_rate_ > 0 ? "" : "+") + ((rhs.attack_rate_ - d.attack_rate_) * 100).ToString() + "%" });

        if (rhs.attack_range_ != d.attack_range_)
            result.Add(new List<string> { "attack range", (rhs.attack_range_ - d.attack_range_ > 0 ? "" : "+") + (rhs.attack_range_ - d.attack_range_).ToString() });

        if (rhs.view_range_ != d.view_range_)
            result.Add(new List<string> { "view range", (rhs.view_range_ - d.view_range_ > 0 ? "" : "+") + (rhs.view_range_ - d.view_range_).ToString() });

        if (rhs.speed_ != d.speed_)
            result.Add(new List<string> { "speed", (rhs.speed_ - d.speed_ > 0 ? "" : "+") + (rhs.speed_ - d.speed_).ToString() });

        if (rhs.HP_ != d.HP_)
            result.Add(new List<string> { "HP", (rhs.HP_ - d.HP_ > 0 ? "" : "+") + (rhs.HP_ - d.HP_).ToString() });

        if (rhs.power_ != d.power_)
            result.Add(new List<string> { "power", (rhs.power_ - d.power_ > 0 ? "" : "+") + (rhs.power_ - d.power_).ToString() });

        if (rhs.CoolDownRate_ != d.CoolDownRate_)
            result.Add(new List<string> { "cool down rate", (rhs.CoolDownRate_ - d.CoolDownRate_ > 0 ? "" : "+") + ((rhs.CoolDownRate_ - d.CoolDownRate_) * 100).ToString() + "%" });

        if (rhs.SkillDamage_ != d.SkillDamage_)
            result.Add(new List<string> { "skill damage", (rhs.SkillDamage_ - d.SkillDamage_ > 0 ? "" : "+") + ((rhs.SkillDamage_ - d.SkillDamage_) * 100).ToString() + "%" });

        result.Add(rhs.component_);

        return result;
    }
}
