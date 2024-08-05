using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnCommand
{
   IEnumerator Execute();
}
