using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CellBrain : MonoBehaviour
{
    public enum CellType
    {
        tower,
        path,
        inactive
    }
    //Gizmos Parameters
    public float sizeX = 1.0f;
    public float sizeY = 0.25f;
    public float sizeZ = 1.0f;
    private Vector3 center;

    //Renderer for visuals
    private Renderer rend;

    //Possible Colors
    public Color towerColor    = Color.green;
    public Color pathColor     = Color.blue;
    public Color inactiveColor = Color.black;

    private Color currentColor = Color.white;

    //Set cell type
    public CellType currentCellType = CellType.inactive;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get reference to renderer
        if(rend != null )
        {
            Debug.Log("Renderer here");
        }
        else
        {
            Debug.Log("NO renderer");
        }
        //Get ref to transform position
        center = transform.position;

        //Change color based on cell type
        switch (currentCellType)
        {
            case CellType.tower:
                currentColor = towerColor;
                break;
            case CellType.path:
                currentColor = pathColor;
                break;
            case CellType.inactive:
                currentColor = inactiveColor;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        center = transform.position;
    }

    public void RandomType()
    {
        int num = Random.Range(0, 2);
        Debug.Log("Random num = " + num);
        switch (num)
        {
            case 0:
                currentCellType = CellType.tower;
                currentColor = towerColor;
                break;
            case 1:
                currentCellType = CellType.path;
                currentColor = pathColor;
                break;
            case 2:
                currentCellType = CellType.inactive;
                currentColor = inactiveColor;
                break; 
        }
        rend.material.color = currentColor;
    }

    private void OnDrawGizmos()
    {
        //Set Gizmo properties
        Gizmos.color = currentColor;
        Vector3 size = new Vector3(sizeX, sizeY, sizeZ);

        //Draw Gizmo
        Gizmos.DrawCube(center, size);   
    }
}
