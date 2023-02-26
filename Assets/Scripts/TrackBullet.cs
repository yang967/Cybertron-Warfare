using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBullet : MonoBehaviour
{
    [SerializeField] GameObject HitEffect_;
    [SerializeField] float speed_ = 100.0f;
    int damage_;
    float ignore_;
    GameObject target_;
    bool set;
    float crit_;
    float crit_damage_;

    void Awake()
    {
        set = false;
    }

    public void Set(int damage, float ignore, float crit, float crit_damage, GameObject target)
    {
        damage_ = damage;
        ignore_ = ignore;
        target_ = target;
        set = true;
        crit_ = crit;
        crit_damage_ = crit_damage;
    }

    private void FixedUpdate()
    {
        if (set)
        {
            if(target_ == null)
            {
                set = false;
                Destroy(gameObject, 3);
            }
            if(target_.GetComponent<Control>() == null || target_.GetComponent<Control>().getHP() <= 0)
            {
                set = false;
                Destroy(gameObject, 5);
            }
            transform.position += transform.forward * speed_ * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target_.transform.position - transform.position), 0.7f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 && other.GetComponent<TurretController>() == null)
            return;
        if (!other.transform.parent.parent.gameObject.Equals(target_))
            return;
        set = false;

        int dmg = damage_ / 10;
        int op = damage_ % 10;
        if (Random.Range(0, 1) < crit_)
            dmg = (int)(dmg * crit_damage_);
        damage_ = dmg * 10 + op;
        target_.GetComponent<Control>().SetHP(damage_, ignore_);
        Destroy(gameObject, 3f);
    }
}
