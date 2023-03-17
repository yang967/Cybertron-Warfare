using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionControl : Control
{
    InstructionQueue queue;

    protected override void Awake()
    {
        base.Awake();
        queue = GetComponent<InstructionQueue>();
        team = -1;
    }

    protected override void Start()
    {
        GameObject rallypoint = GameManager.instance.getMinionRallyPoint();
        GameObject[] minion_targets = GameManager.instance.getMinionTarget(team);
        queue.Add_Instruction(new Instruction(3, rallypoint));
        foreach (GameObject obj in minion_targets)
            if(obj != null)
                queue.Add_Instruction(new Instruction(1, obj));
    }

    public void SetTeam(int team)
    {
        if (this.team != -1)
            return;
        this.team = team;
        transform.GetChild(2).GetComponent<MinionAttack>().SetTeam(team);
    }
}
