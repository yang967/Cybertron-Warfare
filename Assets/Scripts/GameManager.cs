using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Player;
    public static Camera PlayerCamera;
    public const int MaxLevel = 15;

    [SerializeField] Camera camera_;
    [SerializeField] GameObject camera_obj_;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] GameObject MinionRallyPoint;
    [SerializeField] GameObject[] MinionTarget1;
    [SerializeField] GameObject[] MinionTarget2;

    GameObject Base1, Base2;

    public static readonly int[] LevelUpBonus = { 1000, 30 };

    public const int Initial_LevelUp_Exp = 500;

    public const float LevelUpExp_Increment = 1.3f;
    public const float Assistant_Kill_Proportion = 0.7f;

    public const int Minion_Exp = 100;
    public const int Hero_Exp_Per_Level = 200;
    public const int Turret_Exp = 1000;
    public const int HP_Per_Block = 1000;
    
    public static readonly int[] DefendRate = { 200, 800, 1600, 2700, 4100, 6000, 8400, 10700, 14000 };
    public static readonly float[] ResistanceRate = { 0.02f, 0.04f, 0.05f, 0.06f, 0.06f, 0.07f, 0.12f, 0.14f, 0.14f };

    public static readonly string[] Minions = { "MinionMelee", "MinionMelee", "MinionPistol", "MinionPistol", "MinionTank", "MinionTank" };

    public static Vector3 PlayerMiddlePosition = new Vector3(10, 0, 16);

    public const int DEVICE_NUM = 7;
    public const int BACKPACK_SIZE = 7;
    public const int INITIAL_CURRENCY = 100;

    Dictionary<string, int> transformers_dict_;
    List<Transformer> transformers_;
    Dictionary<string, Device> devices_;

    bool player_middle_;
    float Spawn_;
    [SerializeField] float SpawnGap = 60;
    int spawn_time_;
    int spawn_num;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        transformers_dict_ = SaveSystem.DecryptDictionary("TransformerDictionary.moba");
        transformers_ = SaveSystem.LoadCh();
        devices_ = SaveSystem.LoadDevices();
        PlayerCamera = camera_;
        player_middle_ = false;
        Spawn_ = Time.time + 60;
        Base1 = MinionTarget2[MinionTarget2.Length - 1];
        Base2 = MinionTarget1[MinionTarget1.Length - 1];
        spawn_num = 3;
        spawn_time_ = 0;
        Spawn_ = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (Spawn_ - Time.time) + "";
        if(Time.time > Spawn_)
        {
            Base1.GetComponent<PlayerBase>().Spawn(spawn_num);
            Base2.GetComponent<PlayerBase>().Spawn(spawn_num);
            spawn_time_++;
            if (spawn_time_ % 5 == 0 && spawn_num < Minions.Length - 1)
                spawn_num++;
            Spawn_ = Time.time + SpawnGap;
        }
        if(player_middle_)
        {
            camera_obj_.transform.position = Vector3.MoveTowards(camera_obj_.transform.position, new Vector3(Player.transform.position.x + PlayerMiddlePosition.x, 90,
                Player.transform.position.z + PlayerMiddlePosition.z), 300 * Time.deltaTime);
        }
    }

    public Transformer getTransformer(string name)
    {
        return transformers_[transformers_dict_[name]];
    }

    public Device getDevice(string name)
    {
        return devices_[name];
    }

    public void PlayerMiddle()
    {
        player_middle_ = true;
    }

    public void CancelPlayerMiddle()
    {
        player_middle_ = false;
    }

    public GameObject[] getMinionTarget(int i)
    {
        if (i == 1)
            return MinionTarget1;
        else
            return MinionTarget2;
    }

    public GameObject getMinionRallyPoint()
    {
        return MinionRallyPoint;
    }

    public void win(int i)
    {
        if (i == 0 && Base1 != null)
            return;
        if (i == 1 && Base2 != null)
            return;

        Debug.Log("Team " + i + " win!");
    }
}
