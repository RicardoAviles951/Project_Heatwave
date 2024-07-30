using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;

public class MoveAlongPath : MonoBehaviour
{
    public float pathDuration;

    public void Move(PathCreator point)
    {
        transform.DOPath
            (
                point.points.ToArray(),
                pathDuration,
                PathType.CatmullRom).SetEase(Ease.Linear
            ); 
    }


}
