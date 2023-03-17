using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    protected LinkedList<GameObject> targets;
    protected GameObject target;
    protected int team_;
    protected Control control_;
    float time_;
    protected NavMeshAgent agent_;
    [SerializeField] protected Animator animator_;
    protected Transformer character_;
    protected InstructionQueue queue;
    protected bool in_range;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        targets = new LinkedList<GameObject>();
        control_ = transform.parent.GetComponent<Control>();
        team_ = control_.getTeam();
        target = null;
        time_ = 0;
        agent_ = transform.parent.GetComponent<NavMeshAgent>();
        character_ = transform.parent.GetComponent<Control>().getCharacter();
        GetComponent<SphereCollider>().radius = character_.getAttackRange();
        queue = transform.parent.GetComponent<InstructionQueue>();
        in_range = false;
    }

    protected virtual void FixedUpdate()
    {
        if (target == null)
            return;
        if(in_range)
            attack();
    }

    protected virtual void attack()
    {
        if(queue.getCurrentInstruction() != null && queue.getCurrentInstruction().getInstructionType() == 1)
            transform.parent.LookAt(target.transform);
        if (Time.time - time_ > 1.0f / character_.getAttackRate())
        {
            animator_.SetTrigger("attacking");
            time_ = Time.time;
        }
    }

    protected virtual void next()
    {
        target = null;
        if (targets.Count == 0)
            return;
        target = targets.First.Value;
        targets.RemoveFirst();
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
        chase();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(target != null && other.gameObject.Equals(target))
        {
            in_range = true;
            animator_.SetBool("attack", true);
            return;
        }

        if(target != null && other.gameObject.layer == 6 && other.transform.parent.parent.gameObject.Equals(target))
        {
            in_range = true;
            animator_.SetBool("attack", true);
            return;
        }
        if (other.gameObject.Equals(transform.parent.GetChild(0).GetChild(0).gameObject))
            return;
        if (other.gameObject.layer != 6 && other.gameObject.layer != 12)
            return;
        if (other.gameObject.layer == 6 && other.transform.parent.parent.GetComponent<Control>().getTeam() == team_)
            return;
        if (other.GetComponent<TurretController>() != null && other.transform.GetChild(3).GetComponent<Turret>().getTeam() == team_)
            return;
        targets.AddLast(other.gameObject.layer == 6 ? other.transform.parent.parent.gameObject : other.gameObject);
    }

    protected virtual void chase()
    {
        if (target == null)
            return;
        //Debug.Log(transform.parent.name + " chase");
        if (in_range)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            return;
        }
        queue.Insert_and_Stash(new Instruction(2, target));
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (target != null)
        {
            if (other.gameObject.layer == 6 && target.Equals(other.transform.parent.parent.gameObject))
            {
                in_range = false;
                animator_.SetBool("attack", false);
                chase();
                return;
            }
        }
        if (!targets.Contains(other.gameObject))
            return;
        targets.Remove(other.gameObject);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 6 && other.gameObject.layer != 12)
            return;
        if (target == null)
            return;
        if ((other.gameObject.layer == 6 && target.Equals(other.transform.parent.parent.gameObject)) || (other.gameObject.layer == 12 && target.Equals(other.gameObject)))
        {
            in_range = true;
            animator_.SetBool("attack", true);
        }
    }

    public bool isInRange()
    {
        return in_range;
    }

    public virtual void setAnimator(GameObject obj)
    {
        animator_ = obj.GetComponent<Animator>();
    }

    public virtual void CancelTarget()
    {
        target = null;
        in_range = false;
        animator_.SetBool("attack", false);
    }

    public GameObject getTarget()
    {
        return target;
    }
}
