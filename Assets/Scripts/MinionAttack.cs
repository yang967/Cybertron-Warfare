using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAttack : Attack
{
    bool started = false;
    //Control control_;
    GameObject[] TargetBuilding;
    bool isTargetinList = false;

    protected override void Awake()
    {
        base.Awake();
        started = true;
    }

    protected override void Start()
    {
        base.Start();

        TargetBuilding = GameManager.instance.getMinionTarget(team_);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        isTargetinList = false;
        foreach (GameObject t in TargetBuilding)
            if (target == t)
                isTargetinList = true;
        if (target == null || isTargetinList) {
            //if (team_ == 1)
               // Debug.Log("next");
            next();
        }
        try {
            if (queue.getCurrentInstruction().getInstructionType() == 2)
                target = queue.getCurrentInstruction().getTargetObject();
        } catch (System.NullReferenceException) { }
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!started)
            return;

        try
        {
            if (target != null && other.gameObject.Equals(target))
            {
                in_range = true;
                animator_.SetBool("attack", true);
                return;
            }

            if (other.gameObject.layer != 6 && other.gameObject.layer != 12)
                return;

            if (target != null && other.gameObject.layer == 6 && other.transform.parent.parent.gameObject.Equals(target))
            {
                in_range = true;
                animator_.SetBool("attack", true);
                return;
            }
            /*if (other.gameObject.Equals(transform.parent.GetChild(0).GetChild(0).gameObject))
                return;
            if (other.gameObject.layer != 6 && other.gameObject.layer != 12)
                return;
            if (other.gameObject.layer == 6 && (other.transform.parent.parent.GetComponent<Control>().getTeam() == team_ || other.transform.parent.parent.GetComponent<Control>().getTeam() == -1))
                return;
            if (other.GetComponent<TurretController>() != null && other.transform.GetChild(3).GetComponent<Turret>().getTeam() == team_)
                return;
            if (other.GetComponent<PlayerBase>() != null && other.GetComponent<PlayerBase>().getTeam() == team_)
                return;
            Instruction current = queue.getCurrentInstruction();
            if (other.gameObject.layer != 12)
            {
                if (current != null && (((current.getInstructionType() == 2 || current.getInstructionType() == 1) && (current.getTargetObject() != null && current.getTargetObject().layer == 12)) || current.getInstructionType() == 0))
                    queue.Insert_and_Stash(new Instruction(1, other.transform.parent.parent.gameObject));
                else
                    queue.Insert_Instruction(new Instruction(1, other.transform.parent.parent.gameObject));
            }*/
        }
        catch(System.NullReferenceException)
        {
            Debug.Log(team_ + " " + other.name);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!started)
            return;

        base.OnTriggerExit(other);
        if(other.gameObject.layer == 6 && !other.transform.parent.parent.gameObject.Equals(target))
        {
            chase();
        }
    }

    public void SetTeam(int team)
    {
        if (team_ != -1)
            return;
        team_ = team;
    }

    public void RemoveTarget(GameObject obj)
    {
        targets.Remove(obj);
        if (target == obj)
            target = null;

        queue.RemoveInstruction(new Instruction(2, obj));
        queue.RemoveInstruction(new Instruction(1, obj));
    }

    public void AddTarget(GameObject obj)
    {

        foreach (GameObject t in TargetBuilding) {
            if (target == null || target.Equals(t)) {
                targets.AddFirst(obj);
                next();
                return;
            }
        }
            

        targets.AddLast(obj);
              
    }
}
