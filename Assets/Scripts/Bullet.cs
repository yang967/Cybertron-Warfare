using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject HitEffect_;
    GameObject from;
    [SerializeField] float speed_ = 100.0f;
    bool hit_;
    int damage_;
    float ignore_;
    int team_;
    GameObject target_;
    float crit_;
    float crit_damage_;
    bool target_set_;

    void Awake()
    {
        hit_ = false;
        target_set_ = false;
    }

    public void Set(GameObject from, int damage, float ignore, int team, float range, float crit, float crit_damage, GameObject target)
    {
        damage_ = damage;
        ignore_ = ignore;
        team_ = team;
        target_ = target;
        crit_ = crit;
        crit_damage_ = crit_damage;
        this.from = from;
        if(target != null)
        {
            transform.LookAt(target.transform);
            target_set_ = true;
        } 
        else
        {
            HitEffect_.GetComponent<Explosion>().Set(damage_, ignore_, team_, range, false);
        }
    }

    private void FixedUpdate()
    {
        if (target_ == null && target_set_)
        {
            hit_ = true;
            Destroy(gameObject, 3);
        }
        if (!hit_)
            transform.position += transform.forward * speed_ * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            hit_ = true;
            HitEffect_.GetComponent<Explosion>().ExplodeEffect();
            Destroy(gameObject, 3);
            return;
        }

        if (other.gameObject.layer != 12 && other.gameObject.layer != 6)
            return;

        if (other.gameObject.layer == 12 && other.gameObject != target_)
            return;
        if (other.gameObject.layer == 6 && other.transform.parent.parent.gameObject != target_)
            return;

        int dmg = damage_ / 10;
        int op = damage_ % 10;
        float r = Random.Range(0, 100) / 99.0f;
        if (r < crit_)
            dmg = (int)(dmg * crit_damage_);
        damage_ = dmg * 10 + op;

        hit_ = true;
        //transform.position += new Vector3(0, 1.55f - transform.position.y, 0);
        transform.rotation = Quaternion.identity;
        bool killed = false;
        if(target_set_)
        {
            if (target_.GetComponent<Control>() != null)
                killed = target_.GetComponent<Control>().SetHP(damage_, ignore_);
            else if (target_.GetComponent<PlayerBase>() != null)
                target_.GetComponent<PlayerBase>().setHP(damage_, ignore_);
            else
                killed = target_.transform.GetChild(3).GetComponent<Turret>().SetHP(damage_, ignore_);
            HitEffect_.GetComponent<Explosion>().ExplodeEffect();

            if (killed && from != null)
            {
                from.GetComponent<PlayerControl>().AddExp(target_.GetComponent<Control>() != null ? target_.GetComponent<Control>().getLevel() : -2, 0);
                from.GetComponent<PlayerControl>().addCurrency(target_.name.Replace("(Clone)", ""));
            }
        } else
        {
            HitEffect_.GetComponent<Explosion>().Explode();
        }
        Destroy(gameObject, 3);
    }
}
