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
    [SerializeField] GameObject controlPanel;
    [SerializeField] StoreButton Store;
    [SerializeField] StoreButton Fuse;
    [SerializeField] GameObject fm;
    [SerializeField] StoreButton bag;
    [SerializeField] GameObject BackPack;
    [SerializeField] GameObject canvas;

    [SerializeField] LayerMask areaSkillLayer;
    [SerializeField] LayerMask targetSkillLayer;
    [SerializeField] TextMeshProUGUI Debug_PointOver;

    [SerializeField] GameObject result;
    [SerializeField] GameObject pause;
    [SerializeField] LayerMask SkillIncludeLayer;

    [SerializeField] Map map;

    [SerializeField] GameObject RespawnCD;

    public GameObject RespawnCDObj {
        get { return RespawnCD; }
    }

    public LayerMask AreaSkillLayer {
        get { return areaSkillLayer; }
    }

    public LayerMask TargetSkillLayer {
        get { return targetSkillLayer; }
    }

    GameObject Base1, Base2;

    public static readonly int[] LevelUpBonus = { 1000, 30 };

    public const int Initial_LevelUp_Exp = 500;

    public const float LevelUpExp_Increment = 1.3f;
    public const float Assistant_Kill_Proportion = 0.7f;

    public const int Minion_Exp = 100;
    public const int Hero_Exp_Per_Level = 200;
    public const int Turret_Exp = 1000;
    public const int HP_Per_Block = 1000;
    public const int ENERGY_PER_BLOCK = 1000;
    
    public static readonly int[] DefendRate = { 200, 800, 1600, 2700, 4100, 6000, 8400, 10700, 14000 };
    public static readonly float[] ResistanceRate = { 0.02f, 0.04f, 0.05f, 0.06f, 0.06f, 0.07f, 0.12f, 0.14f, 0.14f };

    public static readonly string[] Minions = { "MinionMelee", "MinionMelee", "MinionPistol", "MinionPistol", "MinionCannon", "MinionCannon" };
    public static readonly string[] DevicePos = { "CPU", "Software", "Power-Related Device", "Head", "Body", "Hand", "Foot" };

    public static Vector3 PlayerMiddlePosition = new Vector3(-17.29f, 0, 15.38f);

    public const int DEVICE_NUM = 7;
    public const int BACKPACK_SIZE = 7;
    public const int INITIAL_CURRENCY = 100;

    public const int CURRENCY_PER_MINION_MELEE = 20;
    public const int CURRENCY_PER_MINION_RANGE = 45;
    public const int CURRENCY_PER_MINION_CANNON = 135;
    public const int CURRENCY_PER_CANNON = 250;
    public const int CURRENCY_PER_HERO_LEVEL = 50;

    public const float HEAL_DELAY = 0.5f;
    public const float HEAL_PROPORTION = 0.2f;

    public const int HP_REGEN = 10;
    public const float HP_REGEN_DELAY = 0.5f;

    public const float ENERGY_REGEN = 10;
    public const float ENERGY_REGEN_DELAY = 3;

    Dictionary<string, int> transformers_dict_;
    List<Transformer> transformers_;
    Dictionary<string, Device> devices_;
    bool isPause;

    public bool isPaused {
        get { return isPause; }
    }
    

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

    public GameObject ControlPanel {
        get { return controlPanel; }
    }

    public LayerMask SkillLayer {
        get { return SkillIncludeLayer; }
    }

    public bool MenuChildComponent;

    public bool SwapMode;

    public int ToSwap1 = -1;
    public int ToSwap2 = -1;

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

        MenuChildComponent = false;
        SwapMode = false;
        isPause = false;

        float degree = 90 - camera_obj_.transform.eulerAngles.x;
        float x = Mathf.Tan(degree * Mathf.Deg2Rad) * camera_obj_.transform.position.y;
        Vector3 camera_pos = new Vector3(-x, camera_obj_.transform.position.y - Player.transform.position.y, 0) + Player.transform.position;
        camera_pos = VectorRotate(camera_pos, Player.transform.position, 45);
        PlayerMiddlePosition = camera_pos - Player.transform.position;
        PlayerMiddlePosition.y = 90;

    }

    // Update is called once per frame
    void Update()
    {
        if(!MenuChildComponent && !Store.isActive && !Fuse.isActive && !bag.isActive) {
            if (Input.GetKeyUp(KeyCode.Escape))
                if (isPause) Resume(); else Pause();
        }

        if(Input.GetKeyUp(KeyCode.F)) {
            PlayerCamera.transform.position = new Vector3(Player.transform.position.x + PlayerMiddlePosition.x, 90, Player.transform.position.z + PlayerMiddlePosition.z);
            player_middle_ = true;
        }

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

        /*if(EventSystem.current.IsPointerOverGameObject())
            Debug_PointOver.text = EventSystem.current.currentSelectedGameObject.name;*/
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
        GameObject point = i == 1 ? SpawnPoint1 : SpawnPoint2;
        Debug.Log(point.transform.position);
        return i == 1 ? SpawnPoint1 : SpawnPoint2;
    }

    public GameObject getMinionRallyPoint()
    {
        return MinionRallyPoint;
    }

    public void win(int i)
    {
        Debug.Log("win");

        Debug.Log("Team " + i + " win!");

        result.SetActive(true);
        if (i == Player.GetComponent<Control>().getTeam()) {
            result.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You Win!";
        } else {
            result.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You Lose";
        }
        isPause = true;
        pause.SetActive(false);
        Time.timeScale = 0;
    }

    public int getCurrency(string name, int level = 1)
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
        if (MenuChildComponent)
            return;
        if (Store != null && Store.isActive)
            Store.Click();
        if (Fuse != null && Fuse.isActive)
            Fuse.Click();
        if (bag != null && !bag.isActive)
            bag.Click();
    }

    public void getStore()
    {
        if (MenuChildComponent)
            return;
        if (Store != null && !Store.isActive)
            Store.Click();
        if (Fuse != null && Fuse.isActive)
            Fuse.Click();
        if (bag != null && bag.isActive)
            bag.Click();
    }

    public void getFuse()
    {
        if (MenuChildComponent)
            return;
        if (Store != null && Store.isActive)
            Store.Click();
        if (Fuse != null && !Fuse.isActive)
            Fuse.Click();
        if (bag != null && bag.isActive)
            bag.Click();
    }

    public void ResetSFB()
    {
        if (MenuChildComponent)
            return;
        if (Store != null && Store.isActive)
            Store.Click();
        if (Fuse != null && Fuse.isActive)
            Fuse.Click();
        if (bag != null && bag.isActive)
            bag.Click();
    }

    public void Pause()
    {
        if (isPause)
            return;
        pause.SetActive(true);
        isPause = true;
    }

    public void Resume()
    {
        pause.SetActive(false);
        isPause = false;
    }

    public static Vector3 VectorRotate(Vector3 point, Vector3 center, float angle)
    {
        float rad = Mathf.Deg2Rad * angle;
        float x = (point.x - center.x) * Mathf.Cos(rad) + (point.z - center.z) * Mathf.Sin(rad) + center.x;
        float y = (point.z - center.z) * Mathf.Cos(rad) - (point.x - center.x) * Mathf.Sin(rad) + center.z;

        return new Vector3(x, point.y, y);
    }
}
