using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCommand : ISpawnCommand
{
    private Spawner_Enemy spawner;
    private GameObject obj;
    private int limit;
    private float delay;
    public IEnumerator Execute()
    {
        Debug.Log("Spawner warming up...");

        while (limit > 0)
        {
            if (spawner.canSpawn)
            {
                spawner.Spawn(obj);
                limit--;
                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield return null; // Wait until spawning is enabled again
            }
        }
    
    }

    public SpawnCommand(Spawner_Enemy spawner, GameObject obj, int limit, float delay)
    {
        this.spawner = spawner;
        this.obj = obj;
        this.limit = limit;
        this.delay = delay;
    }
}
