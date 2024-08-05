using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour 
{
    public bool canSpawn;
    public float spawnInterval;
    public abstract void Spawn(GameObject obj);
    public abstract void ResetSpawnTime(float spawnTime);
    public abstract void ToggleSpawner(bool canSpawn);

    private IEnumerator Delay(float delay, float limit, GameObject obj)
    {
        Debug.Log("Spawner warming up...");

        while(limit > 0)
        {
            if (canSpawn)
            {
                Spawn(obj); // Or pass a specific GameObject
            }
            limit--;
            yield return new WaitForSeconds(delay);
        }
    }

    protected virtual void SpawnDelay(float delay, float limit, GameObject obj)
    {
        StartCoroutine(Delay(delay, limit, obj));
    }



}
