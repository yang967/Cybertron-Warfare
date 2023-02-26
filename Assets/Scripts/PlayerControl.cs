using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerControl : Control
{
    Dictionary<string, List<Effect>> effects_;
    int level;
    int exp;
    int next_exp;

    protected override  void Awake()
    {
        base.Awake();
        effects_ = new Dictionary<string, List<Effect>>();
        level = 1;
        exp = 0;
        next_exp = GameManager.Initial_LevelUp_Exp;
    }

    protected override void GeneralUpdate()
    {
        if (character.getHP() + character.getShield() <= 0)
            animator_.SetTrigger("dead");
        if (lock_control)
            return;
        //transform.position = new Vector3(transform.position.x, height_, transform.position.z);
        if (Input.GetKeyUp(KeyCode.Q))
            skill_.Transform();
        else if (Input.GetKeyUp(KeyCode.W))
            skill_.Skill_1_init();
        else if (Input.GetKeyUp(KeyCode.E))
            skill_.Skill_2_init();
        else if (Input.GetKeyUp(KeyCode.R))
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
        character.LevelUp();

        exp -= next_exp;
        next_exp = Mathf.RoundToInt(GameManager.Initial_LevelUp_Exp * Mathf.Pow(GameManager.LevelUpExp_Increment, level - 1));
    }
}
