using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static int team {
        set { PlayerPrefs.SetInt("Team", value); }
        get { return PlayerPrefs.GetInt("Team", 0); }
    }

    public static string LastUsedCharacter {
        set { PlayerPrefs.SetString("LUC", value); }
        get { return PlayerPrefs.GetString("LUC", "OptimusPrime"); }
    }

    public static int Language {
        set { PlayerPrefs.SetInt("language", value);
        }
        get { return PlayerPrefs.GetInt("language", -1); }
    }
}
