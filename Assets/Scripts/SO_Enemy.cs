using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy")]
public class SO_Enemy : ScriptableObject
{
    public string name;
    public int health;
    public float speed;
    public Color color;
}
