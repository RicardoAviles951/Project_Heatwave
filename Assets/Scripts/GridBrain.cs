using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;

public class GridBrain : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;

    public float paddingX = 1.0f;
    public float paddingZ = 1.0f;

    public GameObject cell;

    private Dictionary<GridID, GameObject> grid = new Dictionary<GridID, GameObject>();

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
                CellBrain cellBrain = newCell.GetComponent<CellBrain>();
                //cellBrain.RandomType();
                grid.Add(cellBrain.AssignPos(i, j), newCell);
            }
        }
        //ChooseAlgo();
        //AlgoRandomPath();
        //AlgoZigZag();
        AlgoGeneral();
        AssignRemaining();
    }

    void ClearGrid()
    {
        foreach(GameObject cell in grid.Values)
        {
            Destroy(cell);
        }
        grid.Clear();
    }

    void RefreshGrid()
    {
        ClearGrid();
        GenerateGrid();
        Debug.Log("Grid Refresh");

    }

    //For debugging in engine
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(.5f, .5f, .5f);
        Gizmos.DrawCube(transform.position,size);
    }

    //This algorithm starts in the top middle of the grid and then moves down rows, changing cell type to path along the way until it hits the last row.
    void AlgoDownMid()
    {
        //Find the top middle cell
        int col = columns / 2;
        //Change the cell types going straight down
        for (int i = 0; i < rows; i++)
        {
            GameObject Obj;
            GridID NextID = new GridID(i, col);
            grid.TryGetValue(NextID, out Obj);

            Obj.GetComponent<CellBrain>().SetPath();
        }
    }

    void AlgoDiagonalPath()
    {
        // The diagonal path will be as long as the shortest dimension of the grid
        int pathLength = Mathf.Min(rows, columns);

        for (int i = 0; i < pathLength; i++)
        {
            GameObject Obj;
            GridID NextID = new GridID(i, i); // Diagonal path means row number = column number
            grid.TryGetValue(NextID, out Obj);

            // Check if the object exists before trying to change its type
            if (Obj != null)
            {
                Obj.GetComponent<CellBrain>().SetPath();
            }
        }
    }

    void AssignRemaining()
    {
        //Sets remaining cells to tower type. 
        foreach(GameObject cell in grid.Values)
        {
            cell.GetComponent<CellBrain>().SetTower();
        }
    }

    void ChooseAlgo()
    {
        int rand = Random.Range(0, 3);

        switch(rand)
        {
            case 0:
                AlgoDownMid(); break;
            case 1:
                AlgoDiagonalPath(); break;
            case 2:
                AlgoRandomPath(); break;
        }
    }

    void AlgoZigZag()
    {
        int startingPoint = Random.Range(0, columns);
        int endPoint = rows - 1;
        int currentRow = 0;
        int currentCol = 0;
        int edgeA = 0;
        int edgeB = 9;

        //Start somewhere
        GameObject Obj;
        GridID NextID = new GridID(0, startingPoint);
        currentCol = startingPoint;

        Debug.Log("Starting point: (" + currentRow + "," + currentCol + ")");
        //Set obj with ID to path. 
        grid.TryGetValue(NextID, out Obj);

        if (Obj != null)
        {
            Obj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
        }
        

       

        while(currentRow < endPoint)
        {
            int probs = Random.Range(0, 3); // 1/3 chance to go left right or down.
            float prob = Random.Range(0, 1f);
            Debug.Log("Prob: " + prob);
            //currentRow++;
            if (prob < .4f)//LEFT
            {
                int leftColumn = currentCol - 1; //Gets column of left cell
                if (leftColumn >= edgeA)//There is a cell next to us
                {
                    //Create temporary GridID 
                    GridID tempID = new GridID(currentRow, leftColumn);
                    GameObject tempObj;


                    //Search dictionary with matching grid ID
                    grid.TryGetValue(tempID, out tempObj);

                    //Get cell brain component
                    CellBrain cellBrain = tempObj.GetComponent<CellBrain>();

                    //Check if the left cell is already marked
                    if (cellBrain.currentCellType == CellBrain.CellType.path)
                    {
                        continue;
                    }
                    else //Blank cell 
                    {
                        //Set to path type
                        if (tempObj != null)
                        {
                            //Update the new column we are in
                            currentCol = leftColumn;
                            Debug.Log("Chosen: (" + currentRow + "," + currentCol + ")");
                            tempObj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
                        }
                    }

                }
                else
                {
                    Debug.Log("Cannot move LEFT over edge.");
                    continue;
                }
            }
            else if (prob > .40f & prob < .80f) //RIGHT
            {
                int rightColumn = currentCol + 1; //Gets column of right cell
                if (rightColumn <= edgeB)//There is a cell next to us
                {
                    //Create temporary GridID 
                    GridID tempID = new GridID(currentRow, rightColumn);
                    GameObject tempObj;


                    //Search dictionary with matching grid ID
                    grid.TryGetValue(tempID, out tempObj);

                    //Get cell brain component
                    CellBrain cellBrain = tempObj.GetComponent<CellBrain>();

                    //Check if the left cell is already marked
                    if (cellBrain.currentCellType == CellBrain.CellType.path)
                    {
                        continue;
                    }
                    else //Blank cell 
                    {
                        //Set to path type
                        if (tempObj != null)
                        {
                            //Update the new column we are in
                            currentCol = rightColumn;
                            Debug.Log("Chosen: (" + currentRow + "," + currentCol + ")");
                            tempObj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
                        }
                    }

                }
                else
                {
                    Debug.Log("Cannot move RIGHT over edge.");
                    continue;
                }
            }
            else if (prob > .80f & prob < 1)//DOWN
            {
                int tempRow = currentRow + 1;
                NextID = new GridID(tempRow, currentCol);
                currentRow = tempRow;

                grid.TryGetValue(NextID, out Obj);

                if (Obj != null)
                {
                    Debug.Log("Chosen: (" + currentRow + "," + currentCol + ")");
                    Obj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
                }
            }
            else
            {
                Debug.Log("Nothing");
                break;
            }
            //switch (probs)
            //{
            //    case 0://LEFT

            //        int leftColumn = currentCol - 1; //Gets column of left cell
            //        if(leftColumn >= edgeA)//There is a cell next to us
            //        {
            //            //Create temporary GridID 
            //            GridID tempID = new GridID(currentRow, leftColumn);
            //            GameObject tempObj;


            //            //Search dictionary with matching grid ID
            //            grid.TryGetValue(tempID, out tempObj);

            //            //Get cell brain component
            //            CellBrain cellBrain = tempObj.GetComponent<CellBrain>();

            //            //Check if the left cell is already marked
            //            if(cellBrain.currentCellType == CellBrain.CellType.path)
            //            {
            //                continue;
            //            }
            //            else //Blank cell 
            //            {
            //                //Set to path type
            //                if (tempObj != null)
            //                {
            //                    //Update the new column we are in
            //                    currentCol = leftColumn;
            //                    Debug.Log("Chosen: (" + currentRow + "," + currentCol + ")");
            //                    tempObj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
            //                }
            //            }

            //        }
            //        else
            //        {
            //            Debug.Log("Cannot move LEFT over edge.");
            //            continue;
            //        }
            //        break;
            //    case 1://RIGHT
            //        int rightColumn = currentCol + 1; //Gets column of right cell
            //        if (rightColumn <= edgeB)//There is a cell next to us
            //        {
            //            //Create temporary GridID 
            //            GridID tempID = new GridID(currentRow, rightColumn);
            //            GameObject tempObj;


            //            //Search dictionary with matching grid ID
            //            grid.TryGetValue(tempID, out tempObj);

            //            //Get cell brain component
            //            CellBrain cellBrain = tempObj.GetComponent<CellBrain>();

            //            //Check if the left cell is already marked
            //            if (cellBrain.currentCellType == CellBrain.CellType.path)
            //            {
            //                continue;
            //            }
            //            else //Blank cell 
            //            {
            //                //Set to path type
            //                if (tempObj != null)
            //                {
            //                    //Update the new column we are in
            //                    currentCol = rightColumn;
            //                    Debug.Log("Chosen: (" + currentRow + "," + currentCol + ")");
            //                    tempObj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
            //                }
            //            }

            //        }
            //        else
            //        {
            //            Debug.Log("Cannot move RIGHT over edge.");
            //            continue;
            //        }

            //        break;
            //    case 2://DOWN
            //        int tempRow = currentRow + 1;
            //        NextID = new GridID(tempRow, currentCol);
            //        currentRow = tempRow;

            //        grid.TryGetValue(NextID, out Obj);

            //        if (Obj != null)
            //        {
            //            Debug.Log("Chosen: (" + currentRow + "," + currentCol + ")");
            //            Obj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
            //        }
            //        break;
            //}

        }
        
    }

    void AlgoRandomPath()
    {
        int row = 0;
        int col = 0;

        // Start at the top left cell
        SetPath(row, col);

        // While we haven't reached the bottom edge
        while (row < rows - 1)
        {
            // Randomly choose to move right or down
            if (col < columns - 1 && Random.value < 0.5f)
            {
                col++; // Move right
            }
            else
            {
                row++; // Move down
            }

            // Set the current cell to a path
            SetPath(row, col);
        }

        // Continue to the bottom right corner
        while (col < columns - 1)
        {
            col++; // Move right
            SetPath(row, col);
        }
    }

    void SetPath(int row, int col)
    {
        GameObject Obj;
        GridID NextID = new GridID(row, col);
        grid.TryGetValue(NextID, out Obj);

        if (Obj != null)
        {
            Obj.GetComponent<CellBrain>().currentCellType = CellBrain.CellType.path;
        }
    }

    //Brute Force algo
    void AlgoGeneral()
    {
        int startRow = 0;
        int startCol = 0;
        int currentRow = 0;
        int currentCol = 0;

        
        SetVerticalDir(6, startRow, true);
        SetHorizontalDir(3, startCol, true);
        SetVerticalDir(4, currentRow, false);
        SetHorizontalDir(3, currentCol, true);
        SetVerticalDir(6, currentRow, true);
        SetHorizontalDir(4,currentCol, false);
        SetVerticalDir(3, currentRow, true);
        SetHorizontalDir(7,currentCol, true);
        SetVerticalDir(5, currentRow, false );
        SetHorizontalDir(2, currentCol, false );
        SetVerticalDir(5, currentRow, false);
        SetHorizontalDir(4,currentCol, true);
        SetVerticalDir(9, currentRow, true);
        //local method
        void SetVerticalDir(int maxSteps, int row, bool goDown)
        {
            switch (goDown)
            {
                case true://Going down
                    for (int i = 0; i < maxSteps; i++)
                    {
                        int calcRow = row + i;
                        SetPath(calcRow, currentCol);

                        if (i == (maxSteps - 1))
                        {
                            currentRow = calcRow;
                        }

                    }
                    
                    break;
                case false: //Going up
                    for (int i = 0; i < maxSteps; i++)
                    {
                        int calcRow = row - i;
                        SetPath(calcRow, currentCol); ;

                        if (i == (maxSteps - 1))
                        {
                            currentRow = calcRow;
                        }

                    }
                    break;
            }

            
            
        }
        //local method
        void SetHorizontalDir(int maxSteps, int col, bool goRight)
        {
            switch(goRight)
            {
                case true://Go right
                    for (int i = 0; i < maxSteps; i++)
                    {
                        int calcCol = col + i;
                        SetPath(currentRow, calcCol);

                        if (i == (maxSteps - 1))
                        {
                            currentCol = calcCol;
                        }

                    }
                    break;
                case false://Go left
                    for (int i = 0; i < maxSteps; i++)
                    {
                        int calcCol = col - i;
                        SetPath(currentRow, calcCol);

                        if (i == (maxSteps - 1))
                        {
                            currentCol = calcCol;
                        }

                    }
                    break;
            }


        }
    }

}
