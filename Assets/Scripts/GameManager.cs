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

    [SerializeField] GameObject MinionRallyPoint;
    [SerializeField] GameObject[] MinionTarget1;
    [SerializeField] GameObject[] MinionTarget2;
    [SerializeField] GameObject SpawnPoint1;
    [SerializeField] GameObject SpawnPoint2;
    [SerializeField] StoreButton Store;
    [SerializeField] StoreButton Fuse;
    [SerializeField] GameObject fm;
    [SerializeField] StoreButton bag;
    [SerializeField] GameObject BackPack;
    [SerializeField] GameObject canvas;

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
    public static readonly string[] DevicePos = { "CPU", "Software", "Power-Related Device", "Head", "Body", "Hand", "Foot" };

    public static Vector3 PlayerMiddlePosition = new Vector3(10, 0, 16);

    public const int DEVICE_NUM = 7;
    public const int BACKPACK_SIZE = 7;
    public const int INITIAL_CURRENCY = 100;

    public const int CURRENCY_PER_MINION_MELEE = 20;
    public const int CURRENCY_PER_MINION_RANGE = 45;
    public const int CURRENCY_PER_MINION_CANNON = 135;
    public const int CURRENCY_PER_CANNON = 250;
    public const int CURRENCY_PER_HERO_LEVEL = 50;

    Dictionary<string, int> transformers_dict_;
    List<Transformer> transformers_;
    Dictionary<string, Device> devices_;

    public Dictionary<string, Device> Devices {
        get { return devices_; }
    }

    public GameObject FuseMenu {
        get { return fm; }
    }

    public GameObject Canvas {
        get { return canvas; }
    }

    public GameObject Bag {
        get { return BackPack; }
    }

    bool player_middle_;
    float Spawn_;
    [SerializeField] float SpawnGap = 80;
    int spawn_time_;
    int spawn_num;
    float time;

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
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
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

    public float GetSpawn()
    {
        return Spawn_;
    }

    public float getSpawnGap()
    {
        return SpawnGap;
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

    public GameObject getSpawnPoint(int i)
    {
        return i == 1 ? SpawnPoint1 : SpawnPoint2;
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

    public int getCurrency(string name, int level = 0)
    {
        if (name == "MinionMelee")
            return CURRENCY_PER_MINION_MELEE;
        if (name == "MinionPistol")
            return CURRENCY_PER_MINION_RANGE;
        if (name == "MinionCannon")
            return CURRENCY_PER_MINION_CANNON;
        if (name == "Turret")
            return CURRENCY_PER_CANNON;
        return level * CURRENCY_PER_HERO_LEVEL;
    }

    public int getRespawnTime()
    {
        int game_time = (int)(Time.time - time);
        return game_time / 180 + 10;
    }

    public void getBag()
    {
        if (Store != null && Store.isActive)
            Store.Click();
        if (Fuse != null && Fuse.isActive)
            Fuse.Click();
        if (bag != null && !bag.isActive)
            bag.Click();
    }

    public void getStore()
    {
        if (Store != null && !Store.isActive)
            Store.Click();
        if (Fuse != null && Fuse.isActive)
            Fuse.Click();
        if (bag != null && bag.isActive)
            bag.Click();
    }

    public void getFuse()
    {
        if (Store != null && Store.isActive)
            Store.Click();
        if (Fuse != null && !Fuse.isActive)
            Fuse.Click();
        if (bag != null && bag.isActive)
            bag.Click();
    }

    public void ResetSFB()
    {
        if (Store != null && Store.isActive)
            Store.Click();
        if (Fuse != null && Fuse.isActive)
            Fuse.OnClick();
        if (bag != null && bag.isActive)
            bag.Click();
    }
}
