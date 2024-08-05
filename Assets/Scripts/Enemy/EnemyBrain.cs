using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBrain : MonoBehaviour
{
    public SO_Enemy enemy;
    private Renderer rend;
    private int health;
    private float speed;
    private MoveAlongPath pathFinder;

    private void Awake()
    {
        rend = GetComponent<Renderer>();  
        pathFinder = GetComponent<MoveAlongPath>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = enemy.health;
        speed = enemy.speed;
        rend.material.color = enemy.color;
        pathFinder.pathDuration = speed;
        pathFinder.Move();
    }

    
}
