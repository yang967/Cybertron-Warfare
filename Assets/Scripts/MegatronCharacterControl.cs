using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class MegatronCharacterControl : PlayerCharacterControl
{
    [SerializeField] ParticleSystem gun_fire_;
    [SerializeField] Transform bulletOut;
    [SerializeField] Transform Melee;
    [SerializeField] VisualEffect Charge;
    [SerializeField] GameObject Skill_2_GunFire;
    [SerializeField] GameObject Crush;

    int skill_indicating, skill;
    GameObject skill_indicator;

    Animator Melee_Animator;

    PlayerControl control;
    NavMeshAgent agent_;
    Vector3 Skill_1_target;
    float skill_1_distance;

    AreaDamage area;

    bool in_wall;


    // Start is called before the first frame update
    protected override void Start()
    {
        animator_ = GetComponent<Animator>();
        control = transform.parent.parent.GetComponent<PlayerControl>();
        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();
        agent_ = transform.parent.parent.GetComponent<NavMeshAgent>();
        Melee_Animator = Melee.GetComponent<Animator>();

        Charge.Stop();

        skill = 0;
        skill_indicating = 0;
        skill_indicator = null;
        in_wall = false;
        area = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(skill_indicating == 1)
        {
            Camera camera_ = GameManager.PlayerCamera;
            Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit, Mathf.Infinity, GameManager.instance.AreaSkillLayer))
            {
                skill_indicator.transform.position = new Vector3(rayhit.point.x, 0, rayhit.point.z);
                if(getDistance() > attack_.getRange() * attack_.getRange())
                {
                    skill_indicator.transform.position = transform.parent.parent.position + (skill_indicator.transform.position - transform.parent.parent.position).normalized * attack_.getRange();
                    skill_indicator.transform.position -= new Vector3(0, skill_indicator.transform.position.y, 0);
                }

                if (Input.GetMouseButtonUp(0))
                    SetSkill1Start();
            }
        }
        else if(skill_indicating == 2)
        {
            Camera camera_ = GameManager.PlayerCamera;
            Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit, Mathf.Infinity, GameManager.instance.AreaSkillLayer))
            {
                skill_indicator.transform.position = new Vector3(transform.parent.parent.position.x, 0, transform.parent.parent.position.z);
                skill_indicator.transform.eulerAngles = new Vector3(0,
                    (Quaternion.LookRotation((rayhit.point - skill_indicator.transform.position).normalized)).eulerAngles.y,
                    0);
                Ability s = control.getAbility()[1];
                skill_indicator.transform.position += skill_indicator.transform.forward * s.getRangeX() / 2;

                if (Input.GetMouseButtonUp(0))
                    SetSkill2Start();
            }
        }

        if(skill == 1 && !in_wall)
        {
            animator_.speed = 1;
            transform.parent.parent.position += transform.parent.parent.forward * skill_1_distance / 1.2f * Time.deltaTime;
        }
    }

    void SetSkill1Start()
    {
        skill = 1;
        Skill_1_target = skill_indicator.transform.position;
        skill_1_distance = Mathf.Max(Mathf.Sqrt(getDistance()) - 15, 0);
        Destroy(skill_indicator);
        skill_indicator = null;
        skill_indicating = 0;
        animator_.SetTrigger("skill1");
        agent_.angularSpeed = 0;
        agent_.isStopped = true;
        Quaternion rotation = Quaternion.LookRotation((Skill_1_target - transform.parent.parent.position).normalized);
        transform.parent.parent.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        animator_.speed = 1;
        skill1.UseSkill();
    }

    void SetSkill2Start()
    {
        skill_indicating = 0;
        agent_.angularSpeed = 0;
        agent_.isStopped = true;
        transform.parent.parent.rotation = skill_indicator.transform.rotation;
        animator_.speed = 1;
        animator_.SetTrigger("skill2");

        Ability s = control.getAbility()[1];

        GameObject obj = Instantiate(Resources.Load("AreaDamage") as GameObject, transform.parent.parent.position, transform.parent.parent.rotation);
        area = obj.GetComponent<AreaDamage>();
        area.Set((int)(control.getCharacter().getDamage() * s.getRate() * control.getBuffAmount()["Damage"] * control.getBuffAmount()["SkillDamage"]) * 10,
            control.getCharacter().getIgnore(), control.getTeam(), s.getRangeY(), false, transform.parent.parent.gameObject, false, s.getRangeX());
        Destroy(skill_indicator);
        skill2.UseSkill();
    }


    public void transform_to_vehicle()
    {
        transform.parent.parent.GetComponent<PlayerControl>().transform_to_vehicle();
        agent_.baseOffset = 2;
    }

    public override bool Skill_1_init()
    {
        if (skill_indicator != null)
        {
            if(skill_indicating == 1)
            {
                skill_indicating = 0;
                Destroy(skill_indicator);
                skill_indicator = null;
            }
            return false;
        }

        if (!base.Skill_1_init())
            return false;

        skill_indicator = Instantiate(Resources.Load("TargetCircle") as GameObject, transform.parent.parent.position, Quaternion.identity);
        skill_indicator.transform.localScale = skill_indicator.transform.localScale / 4.3f * control.getAbility()[0].getRangeX();
        skill_indicating = 1;

        return true;
    }

    public override bool Skill_2_init()
    {
        if(skill_indicator != null)
        {
            if(skill_indicating == 2)
            {
                skill_indicating = 0;
                Destroy(skill_indicator);
                skill_indicator = null;
            }
            return false;
        }

        if (!base.Skill_2_init())
            return false;

        Ability s = control.getAbility()[1];
        skill_indicator = Instantiate(Resources.Load("TargetLine") as GameObject, transform.parent.parent.position, Quaternion.identity);
        skill_indicator.transform.localScale = new Vector3(skill_indicator.transform.localScale.x / 7 * s.getRangeY(), 1, skill_indicator.transform.localScale.y / 10 * s.getRangeX());
        skill_indicating = 2;

        return true;
    }

    public void Skill_1_Crush()
    {
        Melee_Animator.SetTrigger("Crush");
        GameObject obj = Instantiate(Resources.Load("AreaDamage") as GameObject, Skill_1_target, Quaternion.identity);
        Ability s = control.getAbility()[0];
        obj.GetComponent<AreaDamage>().Set((int)(control.getCharacter().getDamage() * control.getBuffAmount()["Damage"] * s.getRate() * control.getBuffAmount()["SkillDamage"]) * 10,
            control.getCharacter().getIgnore(), control.getTeam(), s.getRangeX(), false, transform.parent.parent.gameObject);
        area = obj.GetComponent<AreaDamage>();
    }

    public void Skill_1_CrushFinish()
    {
        Melee_Animator.SetTrigger("CrushFinish");
        Destroy(Instantiate(Crush, Skill_1_target, Quaternion.identity), 3);
        skill = 0;
        area.Damage();
    }

    public void Skill_1_Finish()
    {
        Melee_Animator.SetTrigger("Finish");
        agent_.angularSpeed = 300;
        agent_.isStopped = false;
        agent_.destination = transform.parent.parent.position;
    }

    public void Skill_2_Charge()
    {
        Charge.Play();
    }

    public void Skill_2_Fire()
    {
        Charge.Stop();
        Quaternion rotation = bulletOut.rotation;
        rotation.eulerAngles += new Vector3(-90, 0, 0);
        Instantiate(Skill_2_GunFire, bulletOut.position, rotation);
        area.Damage();
    }

    public void Skill_2_Stop()
    {
        agent_.angularSpeed = 300;
        agent_.isStopped = false;
        agent_.destination = transform.parent.parent.position;
    }

    public void GunFire()
    {
        if (gun_fire_ != null)
            gun_fire_.Play();
        GameObject bullet = Instantiate(Resources.Load("EnergyCannonBullet") as GameObject, bulletOut.position, bulletOut.parent.rotation);
        bullet.GetComponent<Bullet>().Set(transform.parent.parent.gameObject, (int)(control.getCharacter().getDamage() * control.getBuffAmount()["Damage"]) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), 0, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
    }

    public override bool Transform()
    {
        if (!isIdleOrRun() && !isAttacking())
            return false;
        animator_.SetTrigger("vehicle");
        agent_.radius = 7.45f;
        return true;
    }

    float getDistance()
    {
        if (skill_indicator == null)
            return 0;
        return (transform.parent.parent.position.x - skill_indicator.transform.position.x) * (transform.parent.parent.position.x - skill_indicator.transform.position.x) +
            (transform.parent.parent.position.z - skill_indicator.transform.position.z) * (transform.parent.parent.position.z - skill_indicator.transform.position.z);
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
}
