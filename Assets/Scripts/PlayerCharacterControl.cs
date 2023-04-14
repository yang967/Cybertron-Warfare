using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacterControl : AbstractSkill
{
    protected List<TriggerComponent> triggers;
    protected Attack attack_;



    [SerializeField] protected SkillComponent skill1;
    [SerializeField] protected SkillComponent skill2;
    [SerializeField] protected SkillComponent skill3;


    protected virtual void Awake()
    {
        triggers = new List<TriggerComponent>();
        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();


        
    }

    protected virtual void Start()
    {
        Transformer character = transform.parent.parent.GetComponent<Control>().getCharacter();
        if (skill1 != null)
            skill1.Set("OptimusPrimeSkill1", character.getAbilities()[0].getChargeNum(), character.getAbilities()[0].getCD());
        if (skill2 != null)
            skill2.Set("OptimusPrimeSkill2", character.getAbilities()[1].getChargeNum(), character.getAbilities()[1].getCD());
        if (skill3 != null)
            skill3.Set("OptimusPrimeSkill3", character.getAbilities()[2].getChargeNum(), character.getAbilities()[2].getCD());
    }

    public virtual void attack()
    {
        foreach (TriggerComponent trigger in triggers)
            trigger.AttackTrigger(transform.parent.parent.gameObject, attack_.getTarget());
    }

    public override bool Skill_1_init()
    {
        if (skill1.getCharge() <= 0)
            return false;

        return true;
    }

    public virtual void Skill_1_trigger()
    {
        foreach (TriggerComponent trigger in triggers)
        {
            GameObject self = transform.parent.parent.gameObject;
            trigger.SkillTrigger(self);
            trigger.Skill1Trigger(self);
        }
    }

    public override bool Skill_2_init()
    {
        if (skill2.getCharge() <= 0)
            return false;

        return true;
    }

    public virtual void Skill_2_trigger()
    {
        foreach (TriggerComponent trigger in triggers)
        {
            GameObject self = transform.parent.parent.gameObject;
            trigger.SkillTrigger(self);
            trigger.Skill2Trigger(self);
        }
    }

    public override bool Skill_3_init()
    {
        if (skill3.getCharge() <= 0)
            return false;

        foreach (TriggerComponent trigger in triggers)
        {
            GameObject self = transform.parent.parent.gameObject;
            trigger.SkillTrigger(self);
            trigger.Skill3Trigger(self);
        }
        return true;
    }

    public override void dead()
    {
        foreach (TriggerComponent trigger in triggers)
            trigger.DeathTrigger(transform.parent.parent.gameObject);

        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(false);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.GetComponent<NavMeshAgent>().enabled = false;
        obj.GetComponent<PlayerControl>().enabled = false;
        obj.GetComponent<InstructionQueue>().clear();
        obj.GetComponent<InstructionQueue>().enabled = false;
        GetComponent<Collider>().enabled = false;
        gameObject.layer = 2;
        StartCoroutine(respawn());
    }

    protected override IEnumerator respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.getRespawnTime());
        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(true);
        obj.transform.GetChild(1).gameObject.SetActive(true);
        obj.GetComponent<NavMeshAgent>().enabled = true;
        obj.GetComponent<PlayerControl>().enabled = true;
        obj.GetComponent<InstructionQueue>().enabled = true;
        GetComponent<Collider>().enabled = true;
        gameObject.layer = 6;
        transform.parent.parent.GetComponent<PlayerControl>().Respawn();
        transform.parent.parent.GetComponent<PlayerControl>().transform_to_robo();
        Debug.Log("respawn");
    }

    public virtual void AddTrigger(TriggerComponent component)
    {
        triggers.Add(component);
        component.EquipTrigger(transform.parent.parent.gameObject);
    }

    public virtual void SetTriggers(List<TriggerComponent> components)
    {
        triggers = components;
        foreach(TriggerComponent trigger in triggers)
        {
            gameObject.AddComponent<TriggerComponent>();
            gameObject.GetComponent<TriggerComponent>().SetStats(trigger);
        }
    }

    public virtual void RemoveTrigger(TriggerComponent component)
    {
        triggers.Remove(component);
        component.UnequipTrigger(transform.parent.parent.gameObject);
    }

    public virtual void AttackTrigger(int trigger, GameObject obj = null){  }

    public virtual void AttackTrigger(int trigger, List<GameObject> objs = null) { }

    public void RefreshStats()
    {
        PlayerControl control = transform.parent.parent.GetComponent<PlayerControl>();

        Ability[] a = control.getCharacter().getAbilities();
        skill1.SetCD(a[0].getCD() * (1 / control.getBuffAmount()["CDRate"]));
        skill2.SetCD(a[1].getCD() * (1 / control.getBuffAmount()["CDRate"]));
        skill3.SetCD(a[2].getCD() * (1 / control.getBuffAmount()["CDRate"]));
    }

    public List<TriggerComponent> GetTriggerComponents()
    {
        return triggers;
    }
}
