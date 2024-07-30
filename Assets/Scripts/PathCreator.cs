using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();


    public void AddPoints(Vector3 point)    
    {
        points.Add(point);
    }

    public void ClearAllPoints()
    {
        points.Clear();
    }
}
