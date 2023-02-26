using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveCh(List<Transformer> transformers)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "./Assets/Files/Transformers.moba";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, transformers);
        stream.Close();
    }

    public static List<Transformer> LoadCh()
    {
        string path = "./Assets/Files/Transformers.moba";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<Transformer> result = formatter.Deserialize(stream) as List<Transformer>;
            stream.Close();
            return result;
        } 
        else
        {
            Debug.Log("Error! Transformers file NOT FOUND!");
            return null;
        }
    }

    public static void SaveDevices(List<Device> devices)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "./Assets/Files/Devices.moba";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, devices);
        stream.Close();
    }

    public static List<Device> LoadDevices()
    {
        string path = "./Assets/Files/Devices.moba";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<Device> result = formatter.Deserialize(stream) as List<Device>;
            stream.Close();
            return result;
        }
        else
        {
            Debug.Log("Error! Transformers file NOT FOUND!");
            return null;
        }
    }

    public static void EncryptDictionary(Dictionary<string, int> dict, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "./Assets/Files/" + name;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, dict);
        stream.Close();
    }

    public static Dictionary<string, int> DecryptDictionary(string name)
    {
        string path = "./Assets/Files/" + name;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Dictionary<string, int> dict = formatter.Deserialize(stream) as Dictionary<string, int>;
            stream.Close();
            return dict;
        }
        else
        {
            Debug.Log(name + "dictionary file NOT FOUND!");
            return null;
        }
    }
}
