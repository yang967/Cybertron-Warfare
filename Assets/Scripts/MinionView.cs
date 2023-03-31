using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionView : CharacterView
{
    InstructionQueue queue_;
    MinionControl control_;
    MinionAttack attack_;

    private void Awake()
    {
        queue_ = transform.parent.GetComponent<InstructionQueue>();
        GetComponent<SphereCollider>().radius = transform.parent.GetComponent<Control>().getCharacter().getViewRange();
        control_ = transform.parent.GetComponent<MinionControl>();
        attack_ = transform.parent.GetChild(2).GetComponent<MinionAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.Equals(transform.parent.GetChild(0).GetChild(0).gameObject) 
            && other.gameObject.layer == 6 
            && other.transform.parent.parent.GetComponent<Control>().getTeam() != -1 
            && other.transform.parent.parent.GetComponent<Control>().getTeam() != control_.getTeam())
        {
            /*Instruction current = queue_.getCurrentInstruction();
            if (current != null && (((current.getInstructionType() == 2 || current.getInstructionType() == 1) && (current.getTargetObject() != null && current.getTargetObject().layer == 12)) || current.getInstructionType() == 0))
                queue_.Insert_and_Stash(new Instruction(1, other.transform.parent.parent.gameObject));
            else
                queue_.Insert_Instruction(new Instruction(1, other.transform.parent.parent.gameObject));*/
            attack_.AddTarget(other.transform.parent.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6 && other.transform.parent.parent.GetComponent<Control>().getTeam() != control_.getTeam())
        {
            queue_.RemoveInstruction(new Instruction(1, other.transform.parent.parent.gameObject));
            attack_.RemoveTarget(other.transform.parent.parent.gameObject);
        }
    }
}
