using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;
using UnityEngine.Events;

public class MoveAlongPath : MonoBehaviour
{
    public float pathDuration;
    private PathCreator path;

    private void Awake()
    {
        path = FindObjectOfType<PathCreator>();
    }
   
    public void Move()
    { 
        if (path != null)
        {
            transform.DOPath
            (
                path.points.ToArray(),
                pathDuration,
                PathType.CatmullRom).SetEase(Ease.Linear
            );
        }
        else
        {
            Debug.Log("No points..");
        }
        
    }


}
