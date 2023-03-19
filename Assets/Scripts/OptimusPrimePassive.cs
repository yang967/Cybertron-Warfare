using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimusPrimePassive : MonoBehaviour
{
    PlayerControl control_;

    // Start is called before the first frame update
    void Start()
    {
        control_ = transform.parent.GetComponent<PlayerControl>();
        GetComponent<SphereCollider>().radius = control_.getAbility()[3].getRangeX();
        control_.AddBuff(control_.getAbility()[3]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6 && other.transform.parent.parent.GetComponent<PlayerControl>() != null && other.transform.parent.parent.GetComponent<PlayerControl>().getTeam() == control_.getTeam())
        {
            other.transform.parent.parent.GetComponent<PlayerControl>().AddBuff(control_.getAbility()[3]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6 && 
            other.transform.parent.parent.GetComponent<PlayerControl>() != null && 
            other.transform.parent.parent.GetComponent<PlayerControl>().getTeam() == control_.getTeam())
        {
            other.transform.parent.parent.GetComponent<PlayerControl>().RemoveBuff(control_.getAbility()[3]);
        }
    }
}
