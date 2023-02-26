using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] Turret turret;
    [SerializeField] GameObject range;
    [SerializeField] GameObject HealthBar;

    public void Emit1()
    {
        turret.Emit1();
    }

    public void Emit2()
    {
        turret.Emit2();
    }

    public void Dead()
    {
        Destroy(range);
        Destroy(HealthBar);
        Destroy(gameObject, 5f);
    }
}
