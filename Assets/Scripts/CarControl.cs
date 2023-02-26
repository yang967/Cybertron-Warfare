using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarControl : AbstractSkill
{
    bool on_high_way_;
    GameObject highway_;
    float height;

    private void Awake()
    {
        animator_ = GetComponent<Animator>();
        on_high_way_ = false;
        height = transform.parent.parent.GetComponent<NavMeshAgent>().baseOffset;
    }

    private void Update()
    {
        if(on_high_way_)
        {
            transform.parent.parent.position = Vector3.MoveTowards(transform.parent.parent.position, new Vector3(highway_.transform.position.x, highway_.transform.position.y + 6.1f, highway_.transform.position.z), 300 * Time.deltaTime);
            transform.parent.parent.rotation = highway_.transform.rotation;
        }
        else
        {
            transform.parent.parent.position = new Vector3(transform.parent.parent.position.x, height, transform.parent.parent.position.z);
        }
    }

    public void HighWay(GameObject highway)
    {
        on_high_way_ = true;
        highway_ = highway;
        transform.parent.parent.GetComponent<NavMeshAgent>().enabled = false;
    }

    public void OutOfHighWay()
    {
        on_high_way_ = false;
        highway_ = null;
    }

    public override bool Skill_1_init()
    {
        return true;
    }

    public override bool Skill_2_init()
    {
        return true;
    }

    public override bool Skill_3_init()
    {
        return true;
    }

    public override bool Transform()
    {
        animator_.SetTrigger("robo");
        return true;
    }

    public void transform_to_robo()
    {
        transform.parent.parent.GetComponent<PlayerControl>().transform_to_robo();
    }
}
