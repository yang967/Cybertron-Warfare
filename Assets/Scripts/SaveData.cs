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
        SaveSystem.EncryptDictionary(TransformerDictionary, "TransformerDictionary.moba");

        List<Transformer> Transformers = new List<Transformer>();
        Transformers.Add(new Transformer("Minion Pistol", 2000, 50, 0, 1, 200, 1.4f, 50, 70, 150, 30, 30, true, 0, new Ability[]
        {
            new Ability(), 
            new Ability(), 
            new Ability(), 
            new Ability(), 
            new Ability()
        }));
        Transformers.Add(new Transformer("Minion Cannon", 2000, 100, 0, 1, 600, 0.3f, 50, 70, 200, 30, 30, true, 0, new Ability[]
        {
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability()
        }));
        Transformers.Add(new Transformer("Minion Melee", 3000, 15, 0, 1, 100, 1, 8, 70, 600, 30, 30, true, 0, new Ability[]
        {
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability(),
            new Ability()
        }));
        Transformers.Add(new Transformer("Optimus Prime", 5000, 200, 0.2f, 1.3f, 200, 1, 50, 70, 400, 30, 50, true, 1, new Ability[]
        {
            new Ability("", "Optimus Prime jump forward and shoot 3 shots to the front area, each shot deal 79.3% damage", 0, 0.793f, 40, 7, false, 0, 1, 15),
            new Ability("", "Optimus Prime spin attack with his axe, deal 235% damage", 0, 2.35f, 20, 20, true, 0, 1, 20, new List<Effect>()
            {
                new Effect(0, "SlowDown", 4, 0.4f)
            }),
            new Ability("", "Inspire the surrounding teammate. Teammate near Optimus Prime gain shield capacity equals to 66.5% Optimus Prime's max HP, last for 10 seconds", 2, 0.665f, 30, 30, true, 10, 1, 22),
            new Ability("", "Teammate near Optimus Prime can have extra 12% attack", 3, 0.12f, 30, 30, true, 0, 0, 0), 
            new Ability("", "Boost speed by 10%. Deal 350% damage when running into enemy", 0, 3.5f, 0, 0, false, 8, 1, 13)
        }));
        SaveSystem.SaveCh(Transformers);

        List<Device> Devices = new List<Device>();
        Devices.Add(new Device("Usable CPU", "Recycled CPU from dead Transformers. Its additional compute power can help calculate the bullet trail", 0, 0, 0, 0, 1, 1, 1.1f, 70));
        Devices.Add(new Device("Scrapped Armor pieces", "Armor pieces recycled from the corpses.", 50, 70));
        Devices.Add(new Device("Half-Drained Power Core", "Used power core found in the city. It can provide additional power at critical time to wait for medics", 0, 0, 0, 500, 1, 1, 1, 70));
        Devices.Add(new Device("Penetration Module", "Gun module that can improve the armor penetration ability", 0, 30, 0, 0, 1, 1, 1, 70));
        Devices.Add(new Device("Electromagnetic Coil", "Add Additional Electromagnetic Coil to the weapon. It can increase weapon damage without affect the fire rate", 10, 0, 0, 0, 1, 1, 1, 70));
        Devices.Add(new Device("Analyzing model", "Exploit additional computer power for data prediction", 0, 0, 0, 0, 1.2f, 1.1f, 1.3f, 200, new List<string>() { "Usable CPU", "Usable CPU" }));
        Devices.Add(new Device("Weakpoint Analyzer", "", 0, 100, 0, 0, 1.5f, 1.6f, 1.5f, 300, new List<string> { "Analyzing model", "Penetration Module" }));
    }
}
