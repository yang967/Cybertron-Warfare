using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : PlayerCharacterControl
{
    [SerializeField] Transform bulletOut;
    [SerializeField] ParticleSystem gun_fire_;
    NavMeshAgent agent_;

    static float fire_rate = 2;
    static float damage = 2.5f;

    // Start is called before the first frame update
    protected void  Awake()
    {
        attack_ = transform.parent.parent.GetChild(2).GetComponent<Attack>();
        control = transform.parent.parent.GetComponent<PlayerControl>();
        animator_ = GetComponent<Animator>();
        agent_ = transform.parent.parent.GetComponent<NavMeshAgent>();
        attack_.SetAttackRate(fire_rate * control.getBuffAmount()["AttackRate"] * control.getCharacter().getAttackRate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void transform_to_robo()
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
        bullet.GetComponent<Bullet>().Set(transform.parent.parent.gameObject, (int)(control.getCharacter().getDamage() * control.getBuffAmount()["Damage"] * damage) * 10, control.getCharacter().getIgnore(),
            control.getTeam(), 0, control.getCharacter().getCritical(), control.getCharacter().getCriticalDamage(), attack_.getTarget());
    }

    public override bool Transform()
    {
        animator_.SetTrigger("robo");
        return true;
    }

    public static float getFireRate()
    {
        return fire_rate;
    }

    public static float getDamage()
    {
        return damage;
    }
}
