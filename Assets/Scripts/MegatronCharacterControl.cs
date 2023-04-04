using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MegatronCharacterControl : PlayerCharacterControl
{
    [SerializeField] ParticleSystem gun_fire_;
    [SerializeField] Transform bulletOut;

    PlayerControl control;
    NavMeshAgent agent_;

    // Start is called before the first frame update
    void Start()
    {
        animator_ = GetComponent<Animator>();
        control = transform.parent.parent.GetComponent<PlayerControl>();
        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();
        agent_ = transform.parent.parent.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void transform_to_vehicle()
    {
        transform.parent.parent.GetComponent<PlayerControl>().transform_to_vehicle();
        agent_.baseOffset = 2;
    }

    public void GunFire()
    {
        if (gun_fire_ != null)
            gun_fire_.Play();
        GameObject bullet = Instantiate(Resources.Load("EnergyCannonBullet") as GameObject, bulletOut.position, bulletOut.parent.rotation);
        bullet.GetComponent<Bullet>().Set(transform.parent.parent.gameObject, (int)(control.getCharacter().getDamage() * control.getBuffAmount()["Damage"]) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), 0, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
    }

    public override bool Transform()
    {
        if (!isIdleOrRun() && !isAttacking())
            return false;
        animator_.SetTrigger("vehicle");
        agent_.radius = 7.45f;
        return true;
    }
}
