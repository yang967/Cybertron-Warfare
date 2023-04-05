using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private void Awake()
    {
        Dictionary<string, int> TransformerDictionary = new Dictionary<string, int>();
        TransformerDictionary.Add("MinionPistol", 0);
        TransformerDictionary.Add("MinionCannon", 1);
        TransformerDictionary.Add("MinionMelee", 2);
        TransformerDictionary.Add("OptimusPrime", 3);
        TransformerDictionary.Add("Megatron", 4);
        SaveSystem.EncryptDictionary(TransformerDictionary, "TransformerDictionary.moba");

        List<Transformer> Transformers = new List<Transformer>();
        Transformers.Add(new Transformer("Minion Pistol", 2000, 0, 50, 0, 1, 200, 1.4f, 50, 70, 150, 30, 30, true, 0, new Ability[]
        {
            new Ability(), 
            new Ability(), 
            new Ability(), 
            new Ability(), 
            new Ability()
        }));
        Transformers.Add(new Transformer("Minion Cannon", 2000, 0, 100, 0, 1, 600, 0.3f, 50, 70, 200, 30, 30, true, 0, new Ability[]
        {
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability()
        }));
        Transformers.Add(new Transformer("Minion Melee", 3000, 0, 15, 0, 1, 100, 1, 8, 70, 600, 30, 30, true, 0, new Ability[]
        {
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability()
        }));
        Transformers.Add(new Transformer("Optimus Prime", 5000, 3000, 200, 0.2f, 1.3f, 200, 1, 50, 70, 400, 30, 50, true, 1, new Ability[]
        {
            new Ability("", "Optimus Prime jump forward and shoot 3 shots to the front area, each shot deal 79.3% damage", 0, 0.793f, 40, 7, false, 0, 1, 15),
            new Ability("", "Optimus Prime spin attack with his axe, deal 235% damage", 0, 2.35f, 20, 20, true, 0, 1, 20, new List<Effect>()
            {
                new Effect(0, "SlowDown", 4, 0.4f)
            }),
            new Ability("Prime Protection", "Inspire the surrounding teammate. Teammate near Optimus Prime gain shield capacity equals to 66.5% Optimus Prime's max HP, last for 10 seconds", 2, 0.65f, 30, 30, true, 10, 1, 60),
            new Ability("Leader's Inspiration", "Teammate near Optimus Prime can have extra 12% attack", 3, 0.12f, 30, 30, true, 0, 0, 0), 
            new Ability("", "Boost speed by 10%. Deal 350% damage when running into enemy", 0, 3.5f, 0, 0, false, 8, 1, 13)
        }));
        Transformers.Add(new Transformer("Megatron", 3000, 4000, 300, 0.25f, 1.3f, 350, 1.7f, 50, 70, 325, 30, 25, true, 2, new Ability[]
        {
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability()
        }));
        SaveSystem.SaveCh(Transformers);

        Dictionary<string, Device> Devices = new Dictionary<string, Device>();
        Devices.Add("Usable CPU", new Device("Usable CPU", 0, 0, 0, 0, 0, 0, 1.1f, 1, 1, 0, 0, 1, 0, 70));
        Devices.Add("Scrapped Armor Pieces", new Device("Scrapped Armor Pieces", 4, 50, 70));
        Devices.Add("Half-Drained Spark Energy Core", new Device("Half-Drained Spark Energy Core", 2, 0, 0, 0, 500, 0, 1, 1, 1, 0, 0, 1, 0, 70));
        Devices.Add("Penetration Module", new Device("Penetration Module", 5, 0, 30, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0, 70));
        Devices.Add("Electromagnetic Coil", new Device("Electromagnetic Coil", 5, 10, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0, 70));
        Devices.Add("Analyzing model", new Device("Analyzing model", 0, 0, 0, 0, 0, 0, 1.2f, 1.1f, 1.3f, 0, 0, 1, 0, 200, new List<string>() { "Usable CPU", "Usable CPU" }));
        Devices.Add("Weakpoint Analyzer", new Device("Weakpoint Analyzer", 0, 0, 100, 0, 0, 0, 1.5f, 1.6f, 1.5f, 0, 0, 1, 0, 300, new List<string> { "Analyzing model", "Penetration Module" }));
        Devices.Add("Half-Drained Power Core", new Device("Half-Drained Power Core", 2, 0, 0, 0, 0, 300, 1, 1, 1, 0, 0, 1, 0, 90));
        Devices.Add("Combined Power Core", new Device("Combined Power Core", 2, 0, 0, 0, 1000, 600, 1, 1, 1, 0, 0, 1, 0, 200, new List<string> { "Half-Drained Spark Energy Core", "Half-Drained Power Core" }));
        Devices.Add("Power Core Imbedded Armor", new Device("Power Core Imbedded Armor", 4, 0, 0, 170, 0, 500, 1, 1, 1, 0, 0, 1, 0, 250, new List<string> { "Scapped Armor Pieces", "Half-Drained Power Core" }));
        Devices.Add("Spark Energy Core Imbedded Armor", new Device("Spark Energy Core Imbedded Armor", 4, 0, 0, 250, 700, 0, 1, 1, 1, 0, 0, 1, 0, 375, new List<string> { "Scapped Armmor Pieces", "Half-Drained Spark Energy Core" }));
        Devices.Add("Combined Armor", new Device("Combined Armor", 4, 0, 0, 750, 1250, 830, 1, 1, 1, 0, 0, 1, 0, 1350, new List<string> { "Power Core Imbedded Armor", "Spark Energy Core Imbedded Armor" }));
        Devices.Add("Weapon Overloader", new Device("Weapon Overloader", 5, 0, 0, 0, 0, 0, 1, 1, 1.1f, 0, 0, 1, 0, 60));
        Devices.Add("Smart Overloader", new Device("Smart Overloader", 5, 0, 0, 0, 0, 0, 1.3f, 1.1f, 1.2f, 0, 0, 1, 0, 240, new List<string> { "Usable CPU", "Weapon Overloader" }));
        Devices.Add("Overload Core", new Device("Overload Core", 2, 0, 0, 0, 0, 3000, 1, 1, 1, 0, 0, 1.1f, 0, 2000, new List<string> { "Combined Power Core", "Smart Overloader" }));
        SaveSystem.SaveDevices(Devices);
    }
}
