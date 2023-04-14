using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    protected LinkedList<GameObject> targets;
    protected GameObject target;
    protected int team_;
    Control control_;
    protected float time_;
    protected NavMeshAgent agent_;
    [SerializeField] protected Animator animator_;
    protected Transformer character_;
    protected InstructionQueue queue;
    protected bool in_range;
    protected AbstractSkill skill;
    protected float attack_rate_;
    protected int Count;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        targets = new LinkedList<GameObject>();
        control_ = transform.parent.GetComponent<Control>();
        team_ = control_.getTeam();
        target = null;
        time_ = 0;
        agent_ = transform.parent.GetComponent<NavMeshAgent>();
        queue = transform.parent.GetComponent<InstructionQueue>();
        in_range = false;
        skill = transform.parent.GetChild(0).GetChild(0).GetComponent<AbstractSkill>();
        Count = 0;
    }

    protected virtual void Start()
    {
        character_ = transform.parent.GetComponent<Control>().getCharacter();
        GetComponent<SphereCollider>().radius = character_.getAttackRange();
    }

    protected virtual void FixedUpdate()
    {
        if (target == null)
            return;
        if(in_range)
            attack();
        Count = targets.Count;
    }

    protected virtual void attack()
    {
        if(queue.getCurrentInstruction() != null && queue.getCurrentInstruction().getInstructionType() == 1)
            transform.parent.LookAt(target.transform);
        if (Time.time > time_)
        {
            animator_.SetTrigger("attacking");
            time_ = Time.time + attack_rate_;
        }
    }

    protected virtual void next()
    {
        target = null;
        if (targets.Count == 0)
            return;
        if(targets.First.Value == null || targets.First.Value.GetComponent<Control>().getHP() <= 0)
        {
            targets.RemoveFirst();
            next();
            return;
        }
        in_range = false;
        //target = targets.First.Value;
        queue.Insert_and_Stash(new Instruction(1, targets.First.Value));
        targets.RemoveFirst();
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
        in_range = false;
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

    public bool isCurrentAnimationState(string name)
    {
        return animator_.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(name);
    }

    public void SetAttackRate(float attack_rate)
    {
        attack_rate_ = attack_rate;
    }

    public float getRange()
    {
        return GetComponent<SphereCollider>().radius;
    }
}
