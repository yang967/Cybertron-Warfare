using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : PlayerCharacterControl
{
    [SerializeField] Transform bulletOut;
    [SerializeField] ParticleSystem gun_fire_;
    PlayerControl control;
    NavMeshAgent agent_;

    // Start is called before the first frame update
    protected override void  Awake()
    {
        base.Awake();
        control = transform.parent.parent.GetComponent<PlayerControl>();
        animator_ = GetComponent<Animator>();
        agent_ = transform.parent.parent.GetComponent<NavMeshAgent>();
        attack_.SetAttackRate(2 * control.getBuffAmount()["AttackRate"] * control.getCharacter().getAttackRate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void transform_to_robo()
    {
        transform.parent.parent.GetComponent<PlayerControl>().transform_to_robo();
        agent_.baseOffset = 10;
        agent_.radius = 2.5f;
        attack_.SetAttackRate(control.getBuffAmount()["AttackRate"] * control.getCharacter().getAttackRate());
    }

    public void GunFire()
    {
        if (gun_fire_ != null)
            gun_fire_.Play();
        GameObject bullet = Instantiate(Resources.Load("EnergyCannonBullet") as GameObject, bulletOut.position, bulletOut.parent.rotation);
        bullet.GetComponent<Bullet>().Set(transform.parent.parent.gameObject, (int)(control.getCharacter().getDamage() * control.getBuffAmount()["Damage"] * 2.5f) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), 0, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
    }

    public override bool Transform()
    {
        animator_.SetTrigger("robo");
        return true;
    }
}
