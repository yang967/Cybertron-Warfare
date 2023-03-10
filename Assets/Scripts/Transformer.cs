using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transformer
{
    string name_;
    float HP_;
    float Max_HP_;
    float damage_;
    float vehicle_damage_;
    float attack_rate_;
    float vehicle_attack_rate_;
    float attack_range_;
    float vehicle_attack_range_;
    float view_range_;
    float defend_;
    float speed_;
    float vehicle_speed_;
    int vehicle_type_;
    bool vehicle_movable_;
    Ability[] abilities_;
    float shield_;
    float ignore_;
    float crit_;
    float crit_damage_;

    public Transformer()
    {
        name_ = "";
        HP_ = 0;
        Max_HP_ = 0;
        damage_ = 0;
        vehicle_damage_ = 0;
        speed_ = 0;
        vehicle_speed_ = 0;
        vehicle_movable_ = true;
        vehicle_type_ = -1;
        defend_ = 0;
        attack_rate_ = 0;
        vehicle_attack_rate_ = 0;
        attack_range_ = 0;
        vehicle_attack_range_ = 0;
        view_range_ = 0;
        abilities_ = new Ability[5];
        shield_ = 0;
        ignore_ = 0;
        crit_ = 0;
        crit_damage_ = 0;
    }

    public Transformer(string name, float HP, float damage, float vehicle_damage, float crit, float crit_damage, float ignore, float attack_rate, float vehicle_attack_rate, float attack_range, 
        float vehicle_attack_range, float view_range, float defend, float speed, float vehicle_speed, bool vehicle_movable, int vehicle_type, Ability[] abilities)
    {
        name_ = name;
        HP_ = HP;
        Max_HP_ = HP;
        damage_ = damage;
        vehicle_damage_ = vehicle_damage;
        ignore_ = ignore;
        speed_ = speed;
        vehicle_speed_ = vehicle_speed;
        vehicle_movable_ = vehicle_movable;
        vehicle_type_ = vehicle_type;
        defend_ = defend;
        attack_rate_ = attack_rate;
        vehicle_attack_rate_ = vehicle_attack_rate;
        attack_range_ = attack_range;
        vehicle_attack_range_ = vehicle_attack_range;
        view_range_ = view_range;
        abilities_ = abilities;
        shield_ = 0;
        crit_ = crit;
        crit_damage_ = crit_damage;
    }

    public Transformer(string name, float HP, float damage, float crit, float crit_damage, float ignore, float attack_rate, float attack_range,
       float view_range, float defend, float speed, float vehicle_speed, bool vehicle_movable, int vehicle_type, Ability[] abilities)
    {
        name_ = name;
        HP_ = HP;
        Max_HP_ = HP;
        damage_ = damage;
        ignore_ = ignore;
        vehicle_damage_ = 0;
        speed_ = speed;
        vehicle_speed_ = vehicle_speed;
        vehicle_movable_ = vehicle_movable;
        vehicle_type_ = vehicle_type;
        defend_ = defend;
        attack_rate_ = attack_rate;
        vehicle_attack_rate_ = 0;
        attack_range_ = attack_range;
        vehicle_attack_range_ = 0;
        view_range_ = view_range;
        abilities_ = abilities;
        shield_ = 0;
        crit_ = crit;
        crit_damage_ = crit_damage;
    }

    public Transformer(Transformer rhs)
    {
        name_ = rhs.getName();
        HP_ = rhs.getHP();
        Max_HP_ = rhs.getMaxHP();
        damage_ = rhs.getDamage();
        vehicle_damage_ = rhs.getVehicleDamage();
        speed_ = rhs.getSpeed();
        vehicle_speed_ = rhs.getVehicleSpeed();
        vehicle_movable_ = rhs.isVehicleMovable();
        vehicle_type_ = rhs.getVehicleType();
        defend_ = rhs.getDefend();
        attack_rate_ = rhs.getAttackRate();
        vehicle_attack_rate_ = rhs.getVehicleAttackRate();
        attack_range_ = rhs.getAttackRange();
        vehicle_attack_range_ = rhs.getVehicleAttackRange();
        view_range_ = rhs.getViewRange();
        abilities_ = new Ability[5];
        for (int i = 0; i < 5; i++)
            abilities_[i] = new Ability(rhs.getAbilities()[i]);
        shield_ = rhs.getShield();
        ignore_ = rhs.getIgnore();
        crit_ = rhs.crit_;
        crit_damage_ = rhs.crit_damage_;
    }

    public void setHP(int dmg, float ignore)
    {
        float HP = HP_;
        int op = dmg % 10;
        dmg /= 10;

        switch(op)
        {
            case 0:
                HP -= dmg * (1 - getResistanceRate(defend_, ignore));
                break;
            case 1:
                HP -= dmg;
                break;
            case 2:
                HP = Mathf.Min(Max_HP_, HP + dmg);
                break;
            default:
                break;
        }
        HP_ = HP;
    }

    public void setMaxHP(float OP)
    {
        if(OP - (int)OP > 0)
        {
            Max_HP_ *= Mathf.Abs(OP);
            HP_ *= Mathf.Abs(OP);
            return;
        }

        int amount = (int)OP;
        int op = amount % 10;
        amount /= 10;
        float proportion;
        switch(op)
        {
            case 1:
                proportion = (Max_HP_ + amount) / Max_HP_;
                Max_HP_ += amount;
                HP_ *= proportion;
                break;
            case 2:
                if(amount > Max_HP_)
                {
                    Max_HP_ = 1;
                    HP_ = 1;
                }
                else
                {
                    proportion = (Max_HP_ - amount) / Max_HP_;
                    Max_HP_ -= amount;
                    HP_ *= proportion;
                }
                break;
            default:
                break;
        }
    }

    public string getName()
    {
        return name_;
    }

    public float getHP()
    {
        return HP_;
    }

    public float getHPProportion()
    {
        return HP_ / Max_HP_;
    }

    public float getSpeed()
    {
        return speed_;
    }

    public float getVehicleSpeed()
    {
        return vehicle_speed_;
    }

    public bool isVehicleMovable()
    {
        return vehicle_movable_;
    }

    public int getVehicleType()
    {
        return vehicle_type_;
    }

    public float getDefend()
    {
        return defend_;
    }

    public float getDamage()
    {
        return damage_;
    }

    public float getVehicleDamage()
    {
        return vehicle_damage_;
    }

    public float getAttackRate()
    {
        return attack_rate_;
    }

    public float getVehicleAttackRate()
    {
        return vehicle_attack_rate_;
    }

    public float getAttackRange()
    {
        return attack_range_;
    }

    public float getVehicleAttackRange()
    {
        return vehicle_attack_range_;
    }

    public float getMaxHP()
    {
        return Max_HP_;
    }

    public float getViewRange()
    {
        return view_range_;
    }

    public static float getResistanceRate(float defend_, float ignore)
    {
        float defend = defend_ - ignore;
        float resistance = 0;
        for(int i = 0; i < 9; i++)
        {
            if(defend < GameManager.DefendRate[i])
            {
                resistance += defend / GameManager.DefendRate[i] * GameManager.ResistanceRate[i];
                defend = 0;
                break;
            }

            resistance += GameManager.ResistanceRate[i];
            defend -= GameManager.DefendRate[i];
        }

        for(int i = 0; i < 5; i++)
        {
            if (defend < 1000)
            {
                resistance += defend / 1000.0f * 0.02f;
                defend = 0;
                break;
            }

            resistance += 0.02f;
            defend -= 1000;
        }

        for(int i = 0; i < 10; i++)
        {
            if(defend < 2000)
            {
                resistance += defend / 2000.0f * 0.01f;
                break;
            }

            resistance += 0.01f;
            defend -= 2000;
        } 

        return resistance;
    }

    public Ability[] getAbilities()
    {
        return abilities_;
    }

    public float getShield()
    {
        return shield_;
    }

    public float getIgnore()
    {
        return ignore_;
    }

    public float getCritical()
    {
        return crit_;
    }

    public float getCriticalDamage()
    {
        return crit_damage_;
    }

    public void SetEffect(Effect added)
    {
        int e_type = added.getEffectType();
        switch(e_type)
        {
            case 0:
                speed_ *= 1 - added.getValue();
                break;
            case 1:
                break;
            default:
                break;
        }
    }

    public void RemoveEffect(Effect remove)
    {
        int e_type = remove.getEffectType();
        switch(e_type)
        {
            case 0:
                speed_ /= 1 - remove.getValue();
                break;
            case 1:
                break;
            default:
                break;
        }
    }

    public void LevelUp()
    {
        float proportion = HP_ / Max_HP_;
        Max_HP_ += GameManager.LevelUpBonus[0];
        HP_ = Max_HP_ * proportion;

        damage_ += GameManager.LevelUpBonus[1];
    }
}
