using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Spawner_Enemy : Spawner
{
    public List<GameObject> enemyList = new List<GameObject>();
    public List<int> enemyCount = new List<int>();

    private SpawnInvoker invoker;
    private Dictionary<GameObject, int> spawnMap = new Dictionary<GameObject, int>();
    public override void ResetSpawnTime(float spawnTime)
    {
        spawnInterval = spawnTime;
    }

    public override void Spawn(GameObject obj)
    {
        Instantiate(obj);
    }

    public override void ToggleSpawner(bool canSpawn)
    {
        canSpawn = !canSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        invoker = gameObject.AddComponent<SpawnInvoker>();
        CreateMap();
        StartCoroutine(HandleSpawn());


    }


    void CreateMap()
    {
        if (enemyList.Count > 0)
        {
            while(enemyCount.Count < enemyList.Count)
            {
                enemyCount.Add(1);
                Debug.Log("Added Enemy");
            }

            // Clear the spawnMap before adding new entries
            spawnMap.Clear();

            for(int i = 0; i < enemyList.Count; i++)
            {
                GameObject obj = enemyList[i];
                int count = enemyCount[i];
                spawnMap[obj] = count; // Add or update the entry in the spawnMap
            }
        }
        else
        {
            Debug.Log("No enemies listed.");
            return;
        }
    }

   

    IEnumerator HandleSpawn()
    {
        foreach (KeyValuePair<GameObject, int> entry in spawnMap)
        {
            GameObject obj = entry.Key;
            int lim = entry.Value;
            invoker.AddCommand(new SpawnCommand(this, obj, lim, spawnInterval));
        }

        yield return StartCoroutine(invoker.ExecuteCommands());
    }
    protected override void SpawnDelay(float delay, float limit, GameObject obj)
    {
        base.SpawnDelay(delay, limit, obj);
    }
     

}
