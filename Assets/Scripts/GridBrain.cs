using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBrain : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;

    public float paddingX = 1.0f;
    public float paddingZ = 1.0f;

    private List<GameObject> cellList = new List<GameObject>();

    public GameObject cell;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        { 
            RefreshGrid();
        }
    }

    void GenerateGrid()
    {
        Vector3 starting = transform.position;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject newCell = GameObject.Instantiate(cell, transform);
                newCell.transform.position = new Vector3((starting.x + paddingX) * i, starting.y, (starting.z + paddingZ) * j);
                newCell.GetComponent<CellBrain>().RandomType();
                cellList.Add(newCell);
            }
        }
    }

    void ClearGrid()
    {
        //Needs a check of list
        foreach(GameObject cell in cellList)
        {
            Destroy(cell);
        }
        cellList.Clear();
    }

    void RefreshGrid()
    {
        ClearGrid();
        GenerateGrid();
        Debug.Log("Grid Refresh");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(.5f, .5f, .5f);
        Gizmos.DrawCube(transform.position,size);
    }
}
