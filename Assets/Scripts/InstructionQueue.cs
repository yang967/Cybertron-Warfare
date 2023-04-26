using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InstructionQueue : MonoBehaviour
{
    LinkedList<Instruction> queue_;
    Instruction current;
    NavMeshAgent agent_;
    Attack attack;
    bool locked;
    float time;
    int count;

    // Start is called before the first frame update
    void Awake()
    {
        queue_ = new LinkedList<Instruction>();
        current = null;
        agent_ = GetComponent<NavMeshAgent>();
        attack = transform.GetChild(2).GetComponent<Attack>();
        locked = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(instruction_complete())
            next_instruction();
        count = queue_.Count;

    }

    private void FixedUpdate()
    {
        if (locked)
        {
            if (current.getInstructionType() != 2 || current.getTargetObject() == null)
            {
                locked = false;
                next_instruction();
            }
            else
            {
                if(Time.time > time)
                {
                    agent_.destination = current.getTargetObject().transform.position;
                    time = Time.time + 2;   
                }
            }
        }
    }

    //0: Go to a certain position
    //1: Attack an object
    //2: Chase an object
    //3: Go to an object

    public void next_instruction()
    {
        if (queue_.Count == 0)
        {
            current = null;
            return;
        }


        current = queue_.First.Value;
        queue_.RemoveFirst();
        execute();
    }

    void execute()
    {
        switch(current.getInstructionType())
        {
            case 0:
                agent_.destination = current.getTarget();
                break;
            case 1:
                if (attack.getTarget() == current.getTargetObject())
                    return;
                attack.setTarget(current.getTargetObject());
                break;
            case 2:
                locked = true;
                break;
            case 3:
                agent_.destination = current.getTargetObject().transform.position;
                break;
            default:
                Debug.Log("Instruction Type " + current.getInstructionType() + " NOT EXIST!");
                break;
        }
    }

    bool instruction_complete()
    {
        if (current == null)
            return true;

        switch(current.getInstructionType())
        {
            case 0:
            case 3:
                Vector3 destination = agent_.destination;
                if(Mathf.Abs((transform.position - destination).magnitude) <= agent_.baseOffset + 0.01)
                    return true;
                break;
            case 1:
                if (attack.getTarget() == null || attack.getTarget() != current.getTargetObject()) {
                    execute();
                    return false;
                }
                if(current.getTargetObject() == null)
                {
                    attack.CancelTarget();
                    return true;
                }
                if(current.getTargetObject().layer == 6 && current.getTargetObject().GetComponent<Control>().getHP() <= 0)
                {
                    attack.CancelTarget();
                    return true;
                }
                else if (current.getTargetObject().GetComponent<TurretController>() != null && current.getTargetObject().transform.childCount < 4)
                {
                    attack.CancelTarget();
                    return true;
                }
                else if(current.getTargetObject().GetComponent<PlayerBase>() != null && current.getTargetObject().GetComponent<PlayerBase>().getHP() <= 0)
                {
                    attack.CancelTarget();
                    return true;
                }
                break;
            case 2:
                if (current.getTargetObject() == null || attack.isInRange()
                    || (current.getTargetObject().GetComponent<PlayerBase>() != null && current.getTargetObject().GetComponent<PlayerBase>().getHP() <= 0)
                    || (current.getTargetObject().GetComponent<Control>() != null && current.getTargetObject().GetComponent<Control>().getHP() <= 0))
                {
                    locked = false;
                    agent_.SetDestination(transform.position);
                    return true;
                }
                break;
            default:
                Debug.Log("Instruction Type " + current.getInstructionType() + " NOT EXIST!");
                break;
        }

        return false;
    }

    public void Add_Instruction(Instruction ins)
    {
        queue_.AddLast(ins);
    }

    public void Insert_Instruction(Instruction ins)
    {
        queue_.AddFirst(ins);
    }

    public void Insert_and_Stash(Instruction ins)
    {
        if(current != null)
            queue_.AddFirst(current);
        current = ins;
        execute();
    }

    public void Clear_and_Execute(Instruction ins)
    {
        clear();
        current = ins;
        execute();
    }

    public void clear_queue()
    {
        queue_.Clear();
    }

    public void clear()
    {
        queue_.Clear();
        current = null;
        attack.CancelTarget();
    }

    public void RemoveInstruction(Instruction other)
    {
        foreach(Instruction ins in queue_)
            if(other.Equals(ins))
            {
                queue_.Remove(ins);
                break;
            }

        if (other.Equals(current))
            next_instruction();
    }

    public Instruction getCurrentInstruction()
    {
        return current;
    }
}
