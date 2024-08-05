using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInvoker : MonoBehaviour
{
    private Queue<ISpawnCommand> commandQueue = new Queue<ISpawnCommand>();
    // Start is called before the first frame update
    public void AddCommand(ISpawnCommand command)
    {
        commandQueue.Enqueue(command);
    }

    public IEnumerator ExecuteCommands()
    {
        while (commandQueue.Count > 0)
        {
            ISpawnCommand command = commandQueue.Dequeue();
            yield return StartCoroutine(command.Execute());
        }
    }
}
