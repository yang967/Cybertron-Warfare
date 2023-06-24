using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject bullet_;
    [SerializeField] int damage_ = 300;
    [SerializeField] float attack_rate = 0.7f;

    [SerializeField] Transform gun_fire_1_;
    [SerializeField] Transform gun_fire_2_;

    [SerializeField] int team = 0;
    [SerializeField] HealthBar health_bar_;
    [SerializeField] float max_HP_ = 20000;
    float HP_;

    float defend_;

    LinkedList<GameObject> targets;
    GameObject target;
    Animator animator_;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        if(team != GameManager.instance.Player.GetComponent<Control>().getTeam())
            transform.parent.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        targets = new LinkedList<GameObject>();
        animator_ = transform.parent.GetComponent<Animator>();
        HP_ = max_HP_;
        health_bar_.SetValue(max_HP_, HP_, 0);
        time = 0;
        defend_ = 2000;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            next();

        if (HP_ <= 0)
            animator_.SetTrigger("Destroy");
        if(target != null && target.GetComponent<Control>().getHP() <= 0)
        {
            targets.Remove(target);
            target = null;
            next();
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.parent.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y + 180, 0)), 0.3f);
        if(Time.time > time)
        {
            animator_.SetTrigger("Fire");
            time = Time.time + attack_rate;
        }
    }

    void next()
    {
        if (target != null)
            return;
        if (targets.Count == 0)
        {
            //Debug.Log("null1");
            target = null;
            return;
        }
        target = targets.First.Value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 || targets.Contains(other.transform.parent.parent.gameObject))
            return;
        if (other.transform.parent.parent.GetComponent<Control>().getTeam() == team)
            return;
        targets.AddLast(other.transform.parent.parent.gameObject);
    }

    public int getTeam()
    {
        return team;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6)
            return;
        if (targets.Contains(other.transform.parent.parent.gameObject))
            targets.Remove(other.transform.parent.parent.gameObject);
        if (other.transform.parent.parent.gameObject.Equals(target))
        {
            target = null;
            next();
        }
    }

    public bool SetHP(int value, float ignore)
    {
        HP_ -= value / 10 * (1 - getResistanceRate(ignore));
        health_bar_.SetValue(max_HP_, HP_, 0);
        if (HP_ <= 0)
            return true;
        return false;
    }

    public float getResistanceRate(float ignore)
    {
        float defend = defend_ - ignore;
        float resistance = 0;
        for (int i = 0; i < 9; i++)
        {
            if (defend < GameManager.DefendRate[i])
            {
                resistance += defend / GameManager.DefendRate[i] * GameManager.ResistanceRate[i];
                defend = 0;
                break;
            }

            resistance += GameManager.ResistanceRate[i];
            defend -= GameManager.DefendRate[i];
        }

        for (int i = 0; i < 5; i++)
        {
            if (defend < 1000)
            {
                resistance += defend / 1000.0f * 0.02f;
                defend = 0;
                break;
            }

            resistance += 0.02f;
            defend -= 1000;
        }

        for (int i = 0; i < 10; i++)
        {
            if (defend < 2000)
            {
                resistance += defend / 2000.0f * 0.01f;
                break;
            }

            resistance += 0.01f;
            defend -= 2000;
        }

        return resistance;
    }

    public float getHP()
    {
        return HP_;
    }

    public void Emit1()
    {
        gun_fire_1_.GetComponent<ParticleSystem>().Play();
        GameObject bullet = Instantiate(bullet_, gun_fire_1_.position, Quaternion.identity);
        bullet.GetComponent<TrackBullet>().Set((damage_ / 2) * 10 + 1, 0, 0, 0, target);
    }

    public void Emit2()
    {
        gun_fire_2_.GetComponent<ParticleSystem>().Play();
        GameObject bullet = Instantiate(bullet_, gun_fire_2_.position, Quaternion.identity);
        bullet.GetComponent<TrackBullet>().Set((damage_ / 2) * 10 + 1, 0, 0, 0, target);
    }

    public GameObject getTarget()
    {
        return target;
    }
}
