using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionCharacterControl : MonoBehaviour
{

    [SerializeField] ParticleSystem gun_fire_;
    [SerializeField] GameObject bullet_;
    [SerializeField] Transform bulletOut;
    Control control;
    Attack attack_;

    // Start is called before the first frame update
    void Start()
    {
        control = transform.parent.parent.GetComponent<Control>();
        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GunFire()
    {
        if(gun_fire_ != null)
            gun_fire_.Play();
        if(bullet_ != null)
        {
            GameObject bullet = Instantiate(Resources.Load("Bullet") as GameObject, bulletOut.position, bulletOut.parent.rotation);
            bullet.GetComponent<Bullet>().Set(null, (int)control.getCharacter().getDamage() * 10, control.getCharacter().getIgnore(),
                control.getTeam(), 0, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
        }
        else
        {
            if (attack_.getTarget().GetComponent<Control>() != null)
                attack_.getTarget().GetComponent<Control>().SetHP((int)(control.getCharacter().getDamage()) * 10, control.getCharacter().getIgnore());
            else if(attack_.getTarget().GetComponent<PlayerBase>() != null)
                attack_.getTarget().GetComponent<PlayerBase>().setHP((int)(control.getCharacter().getDamage()) * 10, control.getCharacter().getIgnore());
            else
                attack_.getTarget().transform.GetChild(3).GetComponent<Turret>().SetHP((int)(control.getCharacter().getDamage()) * 10, control.getCharacter().getIgnore());
        }
    }

    public void Dead()
    {
        GameObject obj = transform.parent.parent.gameObject;
        obj.transform.GetChild(2).gameObject.SetActive(false);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.GetComponent<NavMeshAgent>().enabled = false;
        obj.GetComponent<Control>().enabled = false;
        obj.GetComponent<InstructionQueue>().clear();
        obj.GetComponent<InstructionQueue>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(transform.parent.parent.gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        InstructionQueue queue = transform.parent.parent.GetComponent<InstructionQueue>();
        Instruction current = queue.getCurrentInstruction();
        if (current != null && current.getInstructionType() == 3 && current.getTargetObject().Equals(other.gameObject))
            queue.next_instruction();
    }
}
