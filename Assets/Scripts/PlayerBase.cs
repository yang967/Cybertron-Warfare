using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] HealthBar health_bar_;
    [SerializeField] int team = 0;
    float HP_;
    [SerializeField] float max_HP_ = 50000;
    [SerializeField] float defend_;
    [SerializeField] Transform out_;
    bool spawn_;
    int spawn_indx_;
    int spawn_num_;
    float spawn_time_;
    Animator animator_;

    // Start is called before the first frame update
    void Start()
    {
        HP_ = max_HP_;
        health_bar_.SetValue(max_HP_, HP_);
        spawn_ = false;
        animator_ = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP_ <= 0)
            animator_.SetTrigger("Destroy");
    }

    private void FixedUpdate()
    {
        if(spawn_)
        {
            if(spawn_indx_ >= spawn_num_)
            {
                spawn_ = false;
                return;
            }
            if(Time.time > spawn_time_)
            {
                GameObject minion = Instantiate(Resources.Load(GameManager.Minions[spawn_indx_]) as GameObject, out_.transform.position, Quaternion.identity);
                minion.GetComponent<MinionControl>().SetTeam(team);
                spawn_indx_++;
                spawn_time_ = Time.time + 1;
            }
        }
    }

    public float getHP()
    {
        return HP_;
    }

    public void setHP(float damage, float ignore)
    {
        HP_ -= damage / 10 * (1 - Transformer.getResistanceRate(defend_, ignore));
        health_bar_.SetValue(max_HP_, HP_);
    }

    public void Spawn(int number)
    {
        spawn_ = true;
        spawn_time_ = Time.time;
        spawn_num_ = number;
        spawn_indx_ = 0;
    }

    public void Destroy()
    {
        health_bar_.enabled = false;
        spawn_ = false;
        GameManager.instance.win(team);
    }

    public int getTeam()
    {
        return team;
    }
}
