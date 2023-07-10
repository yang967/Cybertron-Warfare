using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] Turret turret;
    [SerializeField] GameObject range;
    [SerializeField] GameObject HealthBar;

    GameObject icon;

    private void Start()
    {
        string path = "MapObjs/" +
                (turret.getTeam() == GameManager.instance.Player.GetComponent<Control>().getTeam() ? "FriendlyBuilding" : "EnemyBuilding");
        icon = Instantiate(Resources.Load(path) as GameObject, Map.instance.transform);
        Map.SetPosition(gameObject, icon);
    }

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
        Destroy(icon);
        Destroy(range);
        Destroy(HealthBar);
        Destroy(gameObject, 5f);
    }
}
