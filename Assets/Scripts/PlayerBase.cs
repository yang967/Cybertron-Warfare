using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] HealthBar health_bar_;
    [SerializeField] int team = 0;
    float HP_;
    [SerializeField] float max_HP_ = 50000;
    [SerializeField] float defend_;
    [SerializeField] Transform out_;
    [SerializeField] GameObject StreamObj;
    [SerializeField] bool SPAWN_MINION = true;
    Material Stream;
    bool spawn_;
    int spawn_indx_;
    int spawn_num_;
    float spawn_time_;
    Animator animator_;
    int DEBUG_COUNTER;

    GameObject icon;

    // Start is called before the first frame update
    void Start()
    {
        HP_ = max_HP_;
        health_bar_.SetValue(max_HP_, HP_, 0);
        spawn_ = false;
        animator_ = GetComponent<Animator>();
        StreamObj.GetComponent<MeshRenderer>().material = Instantiate(StreamObj.GetComponent<MeshRenderer>().material);
        Stream = StreamObj.GetComponent<MeshRenderer>().material;

        if (team != GameManager.instance.Player.GetComponent<Control>().getTeam())
            transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        Stream.SetFloat("_Alpha", 0);
        DEBUG_COUNTER = 0;

        string path = "MapObjs/" +
                (team == GameManager.instance.Player.GetComponent<Control>().getTeam() ? "FriendlyBuilding" : "EnemyBuilding");
        icon = Instantiate(Resources.Load(path) as GameObject, Map.instance.transform);
        Map.SetPosition(gameObject, icon);
    }

    // Update is called once per frame
    void Update()
    {
        if (HP_ <= 0)
            animator_.SetTrigger("Destroy");
        if (GameManager.instance.GetSpawn() - Time.time <= 10)
            Stream.SetFloat("_Alpha", Mathf.Min(Stream.GetFloat("_Alpha") + 1 * Time.deltaTime, 1));
        else
            Stream.SetFloat("_Alpha", Mathf.Max(Stream.GetFloat("_Alpha") - 1 * Time.deltaTime, 0));

    }

    private void FixedUpdate()
    {
        if (!SPAWN_MINION)
            return;
        if(spawn_)
        {
            if(spawn_indx_ >= spawn_num_)
            {
                spawn_ = false;
                return;
            }
            if(Time.time > spawn_time_)
            {
                GameObject minion = Instantiate(Resources.Load(team == 0 ? GameManager.Minions[spawn_indx_] + "Autobot" : GameManager.Minions[spawn_indx_]) as GameObject, out_.transform.position, Quaternion.identity);
                minion.name = minion.name.Replace("(Clone)", " " + DEBUG_COUNTER++);
                minion.GetComponent<MinionControl>().SetTeam(team);
                if(team != GameManager.instance.Player.GetComponent<Control>().getTeam())
                    minion.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
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
        health_bar_.SetValue(max_HP_, HP_, 0);
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
        Destroy(icon);
        Destroy(gameObject);
        health_bar_.enabled = false;
        spawn_ = false;
        GameManager.instance.win(team == 0 ? 1 : 0);
    }

    public int getTeam()
    {
        return team;
    }
}
