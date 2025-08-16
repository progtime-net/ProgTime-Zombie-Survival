using System;
using UnityEngine;

[Serializable]
public class ZombieSpawnSetting
{
    public int ZombieSpawnFactor { get; set; }
    public int ZombieSpawnDelay { get; set; }
    public GameObject ZombiePrefab { get; set; }

}
