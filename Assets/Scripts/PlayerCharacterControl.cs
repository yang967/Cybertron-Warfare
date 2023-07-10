using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerCharacterControl : AbstractSkill
{
    protected List<TriggerComponent> triggers;
    protected Attack attack_;

    protected PlayerControl control;
    Ability[] abilities;

    [SerializeField] protected GameObject skill1;
    [SerializeField] protected GameObject skill2;
    [SerializeField] protected GameObject skill3;

    int vehicle;

    GameObject icon;
    bool hasIcon = false;

    GameObject RespawnCD;
    float RespawnTime;

    private void Awake()
    {
        triggers = new List<TriggerComponent>();
    }


    protected virtual void Start()
    {
        RespawnTime = 0;
        RespawnCD = GameManager.instance.RespawnCDObj;

        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();

        control = transform.parent.parent.GetComponent<PlayerControl>();
        abilities = control.getAbility();

        GameManager.instance.ControlPanel.transform.GetChild(1).GetChild(vehicle).gameObject.SetActive(true);
        GameManager.instance.ControlPanel.transform.GetChild(2).GetChild(vehicle).gameObject.SetActive(true);
        GameManager.instance.ControlPanel.transform.GetChild(3).GetChild(vehicle).gameObject.SetActive(true);
        skill1 = GameManager.instance.ControlPanel.transform.GetChild(1).GetChild(vehicle).gameObject;
        skill2 = GameManager.instance.ControlPanel.transform.GetChild(2).GetChild(vehicle).gameObject;
        skill3 = GameManager.instance.ControlPanel.transform.GetChild(3).GetChild(vehicle).gameObject;

        int other = vehicle == 0 ? 1 : 0;
        GameManager.instance.ControlPanel.transform.GetChild(1).GetChild(other).gameObject.SetActive(false);
        GameManager.instance.ControlPanel.transform.GetChild(2).GetChild(other).gameObject.SetActive(false);
        GameManager.instance.ControlPanel.transform.GetChild(3).GetChild(other).gameObject.SetActive(false);


        Transformer character = transform.parent.parent.GetComponent<Control>().getCharacter();
        if (skill1 != null)
            skill1.GetComponent<SkillComponent>().Set("", character.getAbilities()[0].getChargeNum(), character.getAbilities()[0].getCD());
        if (skill2 != null)
            skill2.GetComponent<SkillComponent>().Set("", character.getAbilities()[1].getChargeNum(), character.getAbilities()[1].getCD());
        if (skill3 != null)
            skill3.GetComponent<SkillComponent>().Set("", character.getAbilities()[2].getChargeNum(), character.getAbilities()[2].getCD());


        if(!hasIcon) {
            string path = "MapObjs/" +
                (control.getTeam() == GameManager.instance.Player.GetComponent<Control>().getTeam() ? "FriendlyHero" : "EnemyHero");
            icon = Instantiate(Resources.Load(path) as GameObject, Map.instance.transform);
            Map.SetPosition(transform.parent.parent.gameObject, icon);
        }
    }

    protected virtual void FixedUpdate()
    {
        Map.SetPosition(transform.parent.parent.gameObject, icon);

        if(RespawnCD.activeSelf) {
            float t = RespawnTime - Time.time;
            RespawnCD.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (t < 9 ? "0" : "") + ((int)t + 1);
        }
    }

    public virtual void SetIcon(GameObject icon)
    {
        hasIcon = true;
        this.icon = icon;
    }

    public virtual void transform_to_vehicle()
    {
        
    }

    public virtual void transform_to_robo()
    {

    }

    public GameObject getIcon()
    {
        return icon;
    }

    public virtual void attack()
    {
        foreach (TriggerComponent trigger in triggers)
            trigger.AttackTrigger(transform.parent.parent.gameObject, attack_.getTarget());
    }

    public override bool Skill_1_init()
    {
        if (skill1.GetComponent<SkillComponent>().getCharge() <= 0)
            return false;

        if (control.GetEnergy() < abilities[0].getEnergy())
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
        if (skill2.GetComponent<SkillComponent>().getCharge() <= 0)
            return false;

        if (control.GetEnergy() < abilities[1].getEnergy())
            return false;

        return true;
    }

    public void setVehicle(int v)
    {
        vehicle = v;
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
        if (skill3.GetComponent<SkillComponent>().getCharge() <= 0)
            return false;

        if (control.GetEnergy() < abilities[2].getEnergy())
            return false;

        return true;
    }

    public virtual void Skill_3_trigger()
    {
        foreach (TriggerComponent trigger in triggers)
        {
            GameObject self = transform.parent.parent.gameObject;
            trigger.SkillTrigger(self);
            trigger.Skill3Trigger(self);
        }
    }

    public override void dead()
    {
        foreach (TriggerComponent trigger in triggers)
            trigger.DeathTrigger(transform.parent.parent.gameObject);

        if(transform.parent.parent.gameObject == GameManager.instance.Player) {
            RespawnCD.SetActive(true);
            RespawnTime = Time.time + GameManager.instance.getRespawnTime();
        }
        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(false);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.GetComponent<NavMeshAgent>().enabled = false;
        obj.GetComponent<PlayerControl>().enabled = false;
        obj.GetComponent<InstructionQueue>().clear();
        obj.GetComponent<InstructionQueue>().enabled = false;
        GetComponent<Collider>().enabled = false;
        gameObject.layer = 2;
        icon.SetActive(false);

        StartCoroutine(respawn());
    }

    protected override IEnumerator respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.getRespawnTime());
        RespawnCD.SetActive(false);
        transform.parent.parent.GetComponent<PlayerControl>().Respawn();
        transform.parent.parent.GetComponent<PlayerControl>().transform_to_robo();
        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(true);
        obj.transform.GetChild(1).gameObject.SetActive(true);
        obj.GetComponent<NavMeshAgent>().enabled = true;
        obj.GetComponent<PlayerControl>().enabled = true;
        obj.GetComponent<InstructionQueue>().enabled = true;
        GetComponent<Collider>().enabled = true;
        gameObject.layer = 6;
        //if (transform.parent.parent.gameObject == GameManager.instance.Player)
        //  GameManager.instance.PlayerMiddle();
        Vector3 obj_pos = obj.transform.position;
        Vector3 middle = GameManager.PlayerMiddlePosition;
        GameManager.PlayerCamera.transform.position = new Vector3(obj_pos.x + middle.x, 90, obj_pos.z + middle.z);
        icon.SetActive(true);

        Debug.Log("respawn");
        Debug.Log(transform.parent.parent.position);
    }

    public virtual void AddTrigger(TriggerComponent component)
    {
        Debug.Log("add");
        triggers.Add(component);
        component.EquipTrigger(transform.parent.parent.gameObject);
    }

    public virtual void SetTriggers(List<TriggerComponent> components)
    {
        triggers = new List<TriggerComponent>();

        foreach(TriggerComponent t in components)
        {
            TriggerComponent trigger = gameObject.AddComponent(t.GetType()) as TriggerComponent;
            gameObject.GetComponent<TriggerComponent>().SetStats(t);
            triggers.Add(trigger);
        }
    }

    public virtual void RemoveTrigger(TriggerComponent component)
    {
        Debug.Log("remove");
        triggers.Remove(component);
        component.UnequipTrigger(transform.parent.parent.gameObject);
    }

    public virtual void AttackTrigger(int trigger, GameObject obj = null){  }

    public virtual void AttackTrigger(int trigger, List<GameObject> objs = null) { }

    public void RefreshStats()
    {
        PlayerControl control = transform.parent.parent.GetComponent<PlayerControl>();

        Ability[] a = control.getCharacter().getAbilities();
        skill1.GetComponent<SkillComponent>().SetCD(a[0].getCD() * (1 / control.getBuffAmount()["CDRate"]));
        skill2.GetComponent<SkillComponent>().SetCD(a[1].getCD() * (1 / control.getBuffAmount()["CDRate"]));
        skill3.GetComponent<SkillComponent>().SetCD(a[2].getCD() * (1 / control.getBuffAmount()["CDRate"]));
    }

    public List<TriggerComponent> GetTriggerComponents()
    {
        return triggers;
    }

    public virtual bool SetSkill1Start(Vector3 position)
    {
        if (skill1.GetComponent<SkillComponent>().getCharge() <= 0)
            return false;
        control.SetEnergy(abilities[0].getEnergy() * 10);
        return true;
    }

    public virtual bool SetSkill2Start(Vector3 position)
    {
        if (skill2.GetComponent<SkillComponent>().getCharge() <= 0)
            return false;
        control.SetEnergy(abilities[1].getEnergy() * 10);
        return true;
    }

    public virtual bool SetSkill3Start(Vector3 position)
    {
        if (skill3.GetComponent<SkillComponent>().getCharge() <= 0)
            return false;
        control.SetEnergy(abilities[2].getEnergy() * 10);
        return true;
    }
}
