using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBrain : MonoBehaviour
{
    public bool canSpawn;
    public List<GameObject> spawnList;
    public GameObject spawn;
    public Spawner spawner;
    public float spawnDelay;
    public int spawnCount;
    public int maxSpawn;
    public Dictionary<GameObject,int> spawnMap;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
