using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : Control
{
    [SerializeField] DetailPanel detail;
    [SerializeField] bool isPlayer = false;

    Dictionary<string, List<Effect>> effects_;
    int level;
    int exp;
    int next_exp;
    List<string> Devices;
    List<string> Backpack;
    int backpack_count;
    int currency;
    Dictionary<string, float> BuffAmount;
    HashSet<string> Buffs;
    Dictionary<string, float> shield;

    protected override void Awake()
    {
        base.Awake();
        effects_ = new Dictionary<string, List<Effect>>();
        level = 1;
        exp = 0;
        next_exp = GameManager.Initial_LevelUp_Exp;
        Devices = new List<string>(GameManager.DEVICE_NUM);
        for (int i = 0; i < Devices.Capacity; i++)
            Devices.Add("");
        Backpack = new List<string>(GameManager.BACKPACK_SIZE);
        for (int i = 0; i < Backpack.Capacity; i++)
            Backpack.Add("");
        currency = GameManager.INITIAL_CURRENCY;
        backpack_count = 0;
        currency = GameManager.INITIAL_CURRENCY;
        if (detail != null)
        {
            detail.SetLevel(level);
            detail.SetCurrency(currency);
        }
        BuffAmount = new Dictionary<string, float>();
        Buffs = new HashSet<string>();
        BuffAmount.Add("Damage", 1);
        BuffAmount.Add("AttackRange", 1);
        BuffAmount.Add("ViewRange", 1);
        BuffAmount.Add("AttackRate", 1);
        BuffAmount.Add("Speed", 1);
        BuffAmount.Add("SkillDamage", 1);
        BuffAmount.Add("CDRate", 1);
        shield = new Dictionary<string, float>();
    }

    protected override void Start()
    {
        base.Start();
        attack.SetAttackRate(character.getAttackRate() * BuffAmount["AttackRate"]);
    }

    protected override void GeneralUpdate()
    {
        if (character.getHP() + character.getShield() <= 0)
            animator_.SetTrigger("dead");
        if (lock_control)
            return;


        //transform.position = new Vector3(transform.position.x, height_, transform.position.z);
        if (Input.GetKeyUp(KeyCode.Q) && (((AbstractSkill)skill_).isIdleOrRun() || ((AbstractSkill)skill_).isAttacking()))
            skill_.Transform();
        else if (Input.GetKeyUp(KeyCode.W) && (((AbstractSkill)skill_).isIdleOrRun() || ((AbstractSkill)skill_).isAttacking()))
            skill_.Skill_1_init();
        else if (Input.GetKeyUp(KeyCode.E) && (((AbstractSkill)skill_).isIdleOrRun() || ((AbstractSkill)skill_).isAttacking()))
            skill_.Skill_2_init();
        else if (Input.GetKeyUp(KeyCode.R) && (((AbstractSkill)skill_).isIdleOrRun() || ((AbstractSkill)skill_).isAttacking()))
            skill_.Skill_3_init();
    }

    public void hitWall()
    {
        rb_.velocity = Vector3.zero;
    }

    public void transform_to_vehicle()
    {
        Destroy(character_obj_);
        vehicle_ = true;
        character_obj_ = Instantiate(Resources.Load(character_name_ + "VehicleForm") as GameObject, transform.GetChild(0));
        animator_ = character_obj_.GetComponent<Animator>();
        skill_ = character_obj_.GetComponent<Skill>();
        agent_.speed = character.getVehicleSpeed();
        attack.setAnimator(character_obj_);
    }



    public void transform_to_robo()
    {
        Destroy(character_obj_);
        vehicle_ = false;
        character_obj_ = Instantiate(Resources.Load(character_name_ + "Model") as GameObject, transform.GetChild(0));
        animator_ = character_obj_.GetComponent<Animator>();
        skill_ = character_obj_.GetComponent<Skill>();
        agent_.speed = character.getSpeed();
        agent_.radius = radius_;
        attack.setAnimator(character_obj_);
    }

    public void AddDevice(Device device)
    {
        if(Devices[device.getSlot()] != "")
        {
            if (backpack_count < GameManager.BACKPACK_SIZE)
            {
                Backpack.Add(device.getName());
                backpack_count++;
            }
            return;
        }

        character.AddDevice(device);
        BuffAmount["CDRate"] *= device.getCoolDownRate();
        BuffAmount["SkillDamage"] *= device.getSkillDamage();
        Devices[device.getSlot()] = device.getName();
    }

    public void AddDevice(string name)
    {
        Device d = GameManager.instance.getDevice(name);
        AddDevice(d);
    }

    public void RemoveDevice(int slot)
    {
        if (Devices[slot] == "")
            return;
        character.RemoveDevice(GameManager.instance.getDevice(Devices[slot]));
        Devices[slot] = "";
    }

    public void RemoveBackpack(int slot)
    {
        if (Backpack[slot] == "")
            return;
        Backpack[slot] = "";
        backpack_count--;
    }

    public void AddEffect(Effect added)
    {
        if (effects_.ContainsKey(added.getName()) && effects_[added.getName()].Contains(added))
            return;

        if (!effects_.ContainsKey(added.getName()))
            effects_.Add(added.getName(), new List<Effect>());
        int indx = effects_[added.getName()].Count - 1;
        int e_type = added.getEffectType();
        character.SetEffect(added);
        switch (e_type)
        {
            case 0:
                agent_.speed = character.getSpeed();
                break;
            case 1:
                break;
            default:
                Debug.Log("Error! Effect Type " + e_type + " NOT FOUND!");
                break;
        }
        IEnumerator EffectStop()
        {
            yield return new WaitForSeconds(added.getTime());
            RemoveEffect(added.getName(), indx);
        }
        StartCoroutine(EffectStop());
    }

    public void RemoveEffect(string name, int indx)
    {
        if (!effects_.ContainsKey(name) || effects_[name].Count <= indx)
            return;
        Effect remove = effects_[name][indx];
        character.RemoveEffect(remove);
        int e_type = remove.getEffectType();
        switch (e_type)
        {
            case 0:
                agent_.speed = character.getSpeed();
                break;
            case 1:
                break;
            default:
                Debug.Log("Error! Effect Type " + e_type + " NOT FOUND!");
                break;
        }
    }

    public void AddExp(int target_level_, int kill_type)
    {
        int exp_incre;

        if(target_level_ == -1)
            exp_incre = GameManager.Minion_Exp;
        else if(target_level_ == -2)
            exp_incre = GameManager.Turret_Exp;
        else
            exp_incre = Mathf.RoundToInt(target_level_ * GameManager.Hero_Exp_Per_Level * (kill_type == 0 ? 1 : GameManager.Assistant_Kill_Proportion));
        exp += exp_incre;
        if(detail != null)
            detail.SetExpBar((float)exp / next_exp);
        if (exp >= next_exp)
            LevelUp();
    }

    public override int getLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        if (level >= GameManager.MaxLevel)
            return;
        level++;
        detail.SetLevel(level);
        character.LevelUp();

        exp -= next_exp;
        next_exp = Mathf.RoundToInt(GameManager.Initial_LevelUp_Exp * Mathf.Pow(GameManager.LevelUpExp_Increment, level - 1));
        if (detail != null)
            detail.SetExpBar((float)exp / next_exp);
    }

    public void addCurrency(string name)
    {
        currency += GameManager.instance.getCurrency(name);
        if (detail != null)
            detail.SetCurrency(currency);
    }

    public void AddBuff(Ability ability)
    {
        if (Buffs.Contains(ability.getName()))
            return;
        switch(ability.getAbilityType())
        {
            case 3:
                Buffs.Add(ability.getName());
                BuffAmount["Damage"] += ability.getRate();
                break;
            default:
                Debug.Log("Error! Ability Type " + ability.getAbilityType() + " NOT FOUND or is NOT a Passive Ability");
                return;
        }

        RefreshStats();
    }

    public void AddBuff(string name, float rate)
    {
        BuffAmount[name] *= rate;
        RefreshStats();
    }

    public void RemoveBuff(Ability ability)
    {
        if (!BuffAmount.ContainsKey(ability.getName()))
            return;
        BuffAmount[ability.getName()] -= ability.getRate();
        RefreshStats();
    }

    void RefreshStats()
    {
        transform.GetChild(2).GetComponent<SphereCollider>().radius = character.getAttackRange() * BuffAmount["AttackRange"];
        transform.GetChild(3).GetComponent<SphereCollider>().radius = character.getViewRange() * BuffAmount["ViewRange"];
        agent_.speed = character.getSpeed() * BuffAmount["Speed"];
        if (transform.GetChild(0).GetChild(0).GetType() != typeof(TankController))
            attack.SetAttackRate(character.getAttackRate() * BuffAmount["AttackRate"]);
    }

    public Dictionary<string, float> getBuffAmount()
    {
        return BuffAmount;
    }
    
    public HashSet<string> getBuffs()
    {
        return Buffs;
    }

    public void Respawn()
    {
        GameObject respawn = GameManager.instance.getSpawnPoint(team);
        transform.position = respawn.transform.position;
        transform.rotation = respawn.transform.rotation;
    }

    public void RemoveShield(string name)
    {
        if (character.getShield() <= shield[name])
            return;
        character.setHP((int)shield[name] * 10 + 4, 0);
        Debug.Log(character.getShield());
        shield.Remove(name);
        health_bar_.SetValue(character.getMaxHP() + character.getShield(), character.getHP(), character.getShield());
    }

    public void AddShield(string name)
    {
        if (shield.ContainsKey(name))
            return;
        shield.Add(name, character.getShield());
    }

    public List<string> getDevice()
    {
        return Devices;
    }

    public List<string> getBackPack()
    {
        return Backpack;
    }

    public void BuyDevice(string name)
    {
        Device d = GameManager.instance.getDevice(name);
        if (currency < d.getPrice())
        {
            Debug.Log("Not enough money");
            return;
        }
        int emptyslot = -1;

        List<string> components = d.getMergeComponent();
        if(components.Count != 0)
        {
            int[] hasDevice = new int[2];
            hasDevice[0] = -1;
            hasDevice[1] = -1;

            for(int i = 0; i < Devices.Count; i++)
            {
                if (hasDevice[0] == -1 && Devices[i] == components[0])
                    hasDevice[0] = i;

                if (hasDevice[1] == -1 && Devices[i] == components[1])
                    hasDevice[1] = i;
            }

            for (int i = 0; i < Backpack.Count; i++)
            {
                if (hasDevice[0] == -1 && Backpack[i] == components[0])
                    hasDevice[0] = i;

                if (hasDevice[1] == -1 && Backpack[i] == components[1])
                    hasDevice[1] = i;
            }

            if(hasDevice[0] != -1 && hasDevice[1] != -1)
            {
                if (hasDevice[0] / 10 == 0)
                    RemoveDevice(hasDevice[0] % 10);
                else
                    Backpack[hasDevice[0] % 10] = "";

                if (hasDevice[1] / 10 == 0)
                    RemoveDevice(hasDevice[1] % 10);
                else
                    Backpack[hasDevice[1] % 10] = "";
            } else
            {
                Debug.Log("Not Enough Component");
                return;
            }
        }

        if(Devices[d.getSlot()] == "")
        {
            AddDevice(d.getName());
        }
        else
        {
            for (int i = 0; i < Backpack.Count; i++)
                if (Backpack[i] == "")
                {
                    emptyslot = i;
                    break;
                }

            if (emptyslot == -1)
            {
                Debug.Log("No empty slot");
                return;
            }
            Backpack[emptyslot] = d.getName();
        }


        currency -= d.getPrice();
        detail.GetComponent<DetailPanel>().SetCurrency(currency);

        if (isPlayer)
            GameManager.instance.Bag.GetComponent<BackPackMenu>().Refresh();
    }
}
