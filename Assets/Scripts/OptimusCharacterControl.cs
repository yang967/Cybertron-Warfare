using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OptimusCharacterControl : PlayerCharacterControl
{
    [SerializeField] ParticleSystem gun_fire_;
    [SerializeField] float skill_1_range = 20;
    [SerializeField] Transform bulletOut;
    [SerializeField] TrailRenderer skill_2_axe;
    [SerializeField] ParticleSystem Roar;
    

    [SerializeField] GameObject Skill_1_Indicator;
    GameObject Skill_2_;
    GameObject Skill_3_;

    float height_;
    bool skill_1 = false;
    GameObject skill_indicator_;
    Ability[] abilities_;
    int skill;
    NavMeshAgent agent_;
    Quaternion rotation;
    List<GameObject> skill_3_objs;

    bool in_wall;

    protected override void Start()
    {
        base.Start();
        speed_ = 30;
        vehicle_speed_ = 50;
        animator_ = GetComponent<Animator>();
        abilities_ = control.getAbility();
        skill_indicator_ = null;
        skill = -1;
        agent_ = control.GetComponent<NavMeshAgent>();
        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();
    }

    private void Update()
    {
        if(skill_indicator_ != null)
        {
            Camera camera_ = GameManager.PlayerCamera;
            Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit, Mathf.Infinity, GameManager.instance.SkillLayer))
            {
                skill_indicator_.transform.eulerAngles = new Vector3(0,
                    (Quaternion.LookRotation((rayhit.point - skill_indicator_.transform.position).normalized, Vector3.up)).eulerAngles.y,
                    0);
            }

            skill_indicator_.transform.position = new Vector3(transform.parent.parent.position.x, 0, transform.parent.parent.position.z);

            if(Input.GetMouseButtonUp(0))
            {
                if (skill == 1)
                    SetSkill1Start(skill_indicator_.transform.position + skill_indicator_.transform.forward * 1);
            }
        }
        else if(skill_1 && !in_wall)
        {
            transform.parent.parent.eulerAngles = new Vector3(transform.parent.parent.eulerAngles.x, rotation.eulerAngles.y, transform.parent.parent.eulerAngles.z);
            float speed = skill_1_range * (Time.deltaTime / 60.0f * 95);
            transform.parent.parent.position += transform.parent.forward * speed;
            Vector3 obj_pos = transform.parent.parent.position;
            transform.parent.parent.position = new Vector3(obj_pos.x, height_, obj_pos.z);
            //transform.parent.parent.GetComponent<NavMeshAgent>().destination = transform.parent.parent.position;
        }
    }

    public override bool SetSkill1Start(Vector3 position)
    {
        if (!base.SetSkill1Start(position))
            return false;

        agent_.isStopped = true;
        Vector3 r = (position - transform.position).normalized;

        rotation = Quaternion.LookRotation(r, Vector3.up);
        transform.parent.parent.eulerAngles = new Vector3(transform.parent.parent.position.x, rotation.eulerAngles.y, transform.parent.parent.position.z);
        skill1_trigger();
        skill = -1;
        Destroy(skill_indicator_);
        skill_indicator_ = null;
        return true;
    }

    public override void attack()
    {
        base.attack();
        GunFire();
    }

    public override void transform_to_vehicle()
    {
        base.transform_to_vehicle();

        transform.parent.parent.GetComponent<PlayerControl>().transform_to_vehicle();
    }

    public void GunFire()
    {
        base.attack();
        gun_fire_.Play();
        GameObject bullet = Instantiate(Resources.Load("Bullet") as GameObject, bulletOut.position, bulletOut.parent.rotation);
        bullet.GetComponent<Bullet>().Set(transform.parent.parent.gameObject, (int)(control.getCharacter().getDamage() * control.getBuffAmount()["Damage"]) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), 0, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
    }

    public void Skill_1_GunFire()
    {
        gun_fire_.Emit(1);
        GameObject bullet = Instantiate(Resources.Load("Bullet") as GameObject, bulletOut.position, bulletOut.parent.rotation);
        bullet.GetComponent<Bullet>().Set(transform.parent.parent.gameObject, Mathf.RoundToInt(control.getCharacter().getDamage() * control.getAbility()[0].getRate() * control.getBuffAmount()["Damage"]) * 10, control.getCharacter().getIgnore(), 
            control.getTeam(), 6, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
        bullet.GetComponent<Bullet>().SetTrigger(1);
    }

    public void skill_1_start()
    {
        skill_1 = true;
        transform.parent.parent.GetComponent<NavMeshAgent>().enabled = false;
        height_ = transform.parent.parent.position.y;
        base.Skill_1_trigger();
    }

    public void skill_1_end()
    {
        skill_1 = false;
        NavMeshAgent agent = transform.parent.parent.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.destination = transform.parent.parent.position;
    }

    public void skill_2_start()
    {
        skill_2_axe.emitting = true;
    }

    public void skill_2_end()
    {
        skill_2_axe.emitting = false;
        Skill_2_.GetComponent<AreaDamage>().Damage();
    }

    public override bool Transform()
    {
        if (!isIdleOrRun() && !isAttacking())
            return false;
        animator_.SetTrigger("vehicle");
        Destroy(skill_indicator_);
        agent_.radius = 7.45f;
        return true;
    }

    public override bool Skill_1_init()
    {
        if (skill_indicator_ != null)
        {
            skill = -1;
            Destroy(skill_indicator_);
            skill_indicator_ = null;
            return false;
        }
        if (!base.Skill_1_init())
            return false;

        skill_indicator_ = Instantiate(Skill_1_Indicator, transform.parent.parent.position, Quaternion.identity);
        skill_indicator_.transform.GetChild(0).localScale = new Vector3(abilities_[0].getRangeY() / 9.0f, 1, abilities_[0].getRangeX() / 9.0f);
        skill_indicator_.transform.GetChild(0).position += skill_indicator_.transform.forward * (abilities_[0].getRangeX() / 2.0f);
        skill = 1;
        return true;
    }

    public void skill1_trigger()
    {
        skill1.GetComponent<SkillComponent>().UseSkill();
        animator_.SetTrigger("skill1");
        base.Skill_1_trigger();
    }

    public override bool Skill_2_init()
    {
        if (skill_indicator_ != null)
            return false;

        if (!base.Skill_2_init())
            return false;

        base.Skill_2_trigger();

        /*skill2.GetComponent<SkillComponent>().UseSkill();
        agent_.destination = transform.parent.parent.position;
        Skill_2_ = Instantiate(Resources.Load("AreaDamage") as GameObject, transform.position, Quaternion.identity);
        Skill_2_.GetComponent<AreaDamage>().Set(Mathf.RoundToInt(control.getAbility()[1].getRate() * control.getCharacter().getDamage() * control.getBuffAmount()["Damage"] * control.getBuffAmount()["SkillDamage"]) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), control.getAbility()[1].getRangeX(), false, transform.parent.parent.gameObject);
        Skill_2_.GetComponent<AreaDamage>().SetTrigger(2);
        animator_.SetTrigger("skill2");*/

        SetSkill2Start(new Vector3());

        return true;
    }

    public override bool SetSkill2Start(Vector3 position)
    {
        if (!base.SetSkill2Start(position))
            return false;

        skill2.GetComponent<SkillComponent>().UseSkill();
        agent_.destination = transform.parent.parent.position;
        Skill_2_ = Instantiate(Resources.Load("AreaDamage") as GameObject, transform.position, Quaternion.identity);
        Skill_2_.GetComponent<AreaDamage>().Set(Mathf.RoundToInt(control.getAbility()[1].getRate() * control.getCharacter().getDamage() * control.getBuffAmount()["Damage"] * control.getBuffAmount()["SkillDamage"]) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), control.getAbility()[1].getRangeX(), false, transform.parent.parent.gameObject);
        Skill_2_.GetComponent<AreaDamage>().SetTrigger(2);
        animator_.SetTrigger("skill2");
        return true;
    }

    public override bool SetSkill3Start(Vector3 position)
    {
        if(!base.SetSkill3Start(position)) return false;

        base.Skill_3_trigger();
        skill3.GetComponent<SkillComponent>().UseSkill();
        agent_.destination = transform.parent.parent.position;
        animator_.SetTrigger("skill3");
        Skill_3_ = Instantiate(Resources.Load("AreaDamage") as GameObject, transform.position, Quaternion.identity);
        Skill_3_.GetComponent<AreaDamage>().Set(Mathf.RoundToInt(control.getAbility()[2].getRate() * control.getCharacter().getMaxHP() * control.getBuffAmount()["SkillDamage"] * control.getBuffAmount()["SkillDamage"]) * 10 + 3,
            control.getCharacter().getIgnore(), control.getTeam() == 1 ? 2 : 1, control.getAbility()[2].getRangeX(), false, transform.parent.parent.gameObject);

        return true;
    }

    public override bool Skill_3_init()
    {
        if (skill_indicator_ != null)
            return false;

        if (!base.Skill_3_init())
            return false;

        /*base.Skill_3_trigger();
        skill3.GetComponent<SkillComponent>().UseSkill();
        agent_.destination = transform.parent.parent.position;
        animator_.SetTrigger("skill3");
        Skill_3_ = Instantiate(Resources.Load("AreaDamage") as GameObject, transform.position, Quaternion.identity);
        Skill_3_.GetComponent<AreaDamage>().Set(Mathf.RoundToInt(control.getAbility()[2].getRate() * control.getCharacter().getMaxHP() * control.getBuffAmount()["SkillDamage"] * control.getBuffAmount()["SkillDamage"]) * 10 + 3,
            control.getCharacter().getIgnore(), control.getTeam() == 1 ? 2 : 1, control.getAbility()[2].getRangeX(), false, transform.parent.parent.gameObject);*/

        SetSkill3Start(new Vector3());

        return true;
    }

    public void Skill_3_Effect()
    {
        Roar.Play();
        skill_3_objs = Skill_3_.GetComponent<AreaDamage>().GetTargets();
        foreach (GameObject obj in skill_3_objs)
            if(obj.TryGetComponent<PlayerControl>(out PlayerControl c))
                c.AddShield(control.getAbility()[2].getDuration(), control.getAbility()[2].getName());
        skill_3_objs = Skill_3_.GetComponent<AreaDamage>().Damage();

    }

    /*IEnumerator remove_shield()
    {
        yield return new WaitForSeconds(control.getAbility()[2].getDuration());

        if (skill_3_objs == null)
            yield return null;
        foreach (GameObject obj in skill_3_objs)
            if(obj.TryGetComponent<PlayerControl>(out PlayerControl c))
                c.RemoveShield(control.getAbility()[2].getName());
        skill_3_objs = null;
    }*/

    public void skill_cancel()
    {
        skill = -1;
        Destroy(skill_indicator_);
        skill_indicator_ = null;
    }

    public override void AttackTrigger(int trigger, List<GameObject> objs = null)
    {
        if (objs == null)
            return;
        if(trigger == 1)
        {
            foreach(GameObject obj in objs)
            {
                if (obj.GetComponent<PlayerControl>() == null)
                    continue;
                obj.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BuffTrigger>();
                obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<BuffTrigger>().Set("Damage", 3, 0.93f);
            }
        }
        if(trigger == 2)
        {
            foreach (GameObject obj in objs)
            {
                if (obj.GetComponent<PlayerControl>() == null)
                    continue;
                obj.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BuffTrigger>();
                obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<BuffTrigger>().Set("Speed", 2, 0.75f);
            }
        }
    }

    public void Inwall()
    {
        in_wall = true;
    }

    public void OutOfWall()
    {
        in_wall = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 13)
            in_wall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 13)
            in_wall = false;
    }

    public override void dead()
    {
        base.dead();
        Destroy(skill_indicator_);
    }

    /*public override void dead()
    {
        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(false);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.GetComponent<NavMeshAgent>().enabled = false;
        obj.GetComponent<PlayerControl>().enabled = false;
        obj.GetComponent<InstructionQueue>().clear();
        obj.GetComponent<InstructionQueue>().enabled = false;
        GetComponent<Collider>().enabled = false;
        gameObject.layer = 2;
    }

    public override void respawn()
    {
        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(true);
        obj.transform.GetChild(1).gameObject.SetActive(true);
        obj.GetComponent<NavMeshAgent>().enabled = true;
        obj.GetComponent<PlayerControl>().enabled = true;
        obj.GetComponent<InstructionQueue>().enabled = true;
        GetComponent<Collider>().enabled = true;
        gameObject.layer = 6;
    }*/
}
