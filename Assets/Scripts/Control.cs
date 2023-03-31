using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Control : MonoBehaviour
{
    [SerializeField] protected HealthBar health_bar_;
    [SerializeField] protected int team = -1;
    [SerializeField] protected bool lock_control = false;

    protected Rigidbody rb_;
    protected NavMeshAgent agent_;
    protected Vector3 dest_;
    protected float radius_;
    protected GameObject character_obj_;
    protected Animator animator_;
    protected string character_name_;
    protected Vector3 prevPosition_;
    protected bool vehicle_;
    protected Skill skill_;
    protected Transformer character;
    protected float height_;
    protected Attack attack;

    protected virtual void Awake()
    {
        rb_ = GetComponent<Rigidbody>();
        agent_ = GetComponent<NavMeshAgent>();
        agent_.isStopped = false;
        agent_.destination = transform.position;
        character_obj_ = transform.GetChild(0).GetChild(0).gameObject;
        animator_ = character_obj_.GetComponent<Animator>();
        dest_ = transform.position;
        prevPosition_ = transform.position;
        vehicle_ = false;
        character_name_ = character_obj_.name.Replace("Model", "");
        skill_ = character_obj_.GetComponent<Skill>();
        character = new Transformer(GameManager.instance.getTransformer(character_name_));
        agent_.speed = character.getSpeed();
        height_ = agent_.baseOffset;
        health_bar_.SetValue(character.getMaxHP() + character.getShield(), character.getHP(), character.getShield());
        radius_ = agent_.radius;
        attack = transform.GetChild(2).GetComponent<Attack>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        prevPosition_ = transform.position;
    }

    protected virtual void GeneralUpdate()
    {
        if (character.getHP() + character.getShield() <= 0)
            animator_.SetTrigger("dead");
    }

    protected virtual void RoboModeUpdate()
    {
        float velocity = getVelocity();
        dest_ = agent_.destination;
        if ((agent_.destination - transform.position).magnitude < agent_.baseOffset + 0.01f)
            agent_.destination = transform.position;

        if (velocity > 5)
            animator_.speed = Mathf.Min(velocity / agent_.speed, 1);
        else
            animator_.speed = 1;
        if (velocity > 5)
            animator_.SetBool("run", true);
        else
            animator_.SetBool("run", false);


    }

    protected float getVelocity()
    {
        Vector3 curMove = transform.position - prevPosition_;
        float velocity = curMove.magnitude / Time.deltaTime;
        //if(transform.name == "Megatron")
          //  Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z + " - " + prevPosition_.x + " " + prevPosition_.y + " " + prevPosition_.z);
        prevPosition_ = transform.position;
        return velocity;
    }

    public void setDestination(Vector3 dest)
    {
        Debug.Log("Set");
        dest_ = dest;
        agent_.destination = dest;
        prevPosition_ = transform.position;
    }

    public bool isVehicleForm()
    {
        return vehicle_;
    }

    public float getHP()
    {
        return character.getHP() + character.getShield();
    }

    public bool SetHP(int value, float ignore)
    {
        character.setHP(value, ignore);
        health_bar_.SetValue(character.getMaxHP() + character.getShield(), character.getHP(), character.getShield());
        if (character.getHP() <= 0)
            return true;
        return false;
    }

    public Transformer getCharacter()
    {
        return character;
    }

    public int getTeam()
    {
        return team;
    }

    public Ability[] getAbility()
    {
        return character.getAbilities();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        dest_ = agent_.destination;
        if (!vehicle_)
            RoboModeUpdate();
        GeneralUpdate();
    }

    public virtual int getLevel()
    {
        return -1;
    }
}
