using UnityEngine;
using System.Collections;
using System.Linq;

public class GameInit : MonoBehaviour {
    public int width;
    public int height;
    public float mineDim = 2.56f;
    private static float CAMERA_V_SCALE = 41f / 32f;
    private static float CAMERA_H_SCALE = 24f / 32f;
	// Use this for initialization
	void Start () {
        GameObject camera = GameObject.Find("Camera");
        camera.name = "Camera";
        Vector3 pos = camera.transform.position;
        camera.transform.position = new Vector3(width * mineDim / 2, height * mineDim / 2, pos.z);
        camera.GetComponent<Camera>().orthographicSize = (width < height) ? (height * CAMERA_V_SCALE) : (width * CAMERA_H_SCALE);

        //Vector2[] points = { 
        //                       new Vector2(1, 1), 
        //                       new Vector2(2, 2), 
        //                       new Vector2(10, 10), 
        //                       new Vector2(11, 10), 
        //                       new Vector2(11, 11), 
        //                       new Vector2(12, 12), 
        //                   };

        int numPoints = 99;
        Vector2[] points = new Vector2[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector2(Random.Range(0, width), Random.Range(0, height));
        }
        Vector2[] distinct = points.Distinct().ToArray<Vector2>();

        GenerateUsingMines(distinct);

        //for (int i = 0; i < width; i++)
        //{
        //    for (int j = 0; j < height; j++)
        //    {
        //        GameObject mine;
        //        //string s = mine.name + "/Offset/Hidden";
        //        int value = Random.Range(0, 4);
        //        //Debug.Log(value);
        //        switch (value)
        //        {
        //            case 0:
        //                mine = Instantiate(Resources.Load("MineCellS")) as GameObject;
        //                mine.name = "MineCell_";
        //                break;
        //            case 1:
        //                mine = Instantiate(Resources.Load("Number1CellS")) as GameObject;
        //                mine.name = "NumberCell_";
        //                break;
        //            case 2:
        //                mine = Instantiate(Resources.Load("EmptyCellS")) as GameObject;
        //                mine.name = "EmptyCell_";
        //                break;
        //            default:
        //                mine = Instantiate(Resources.Load("EmptyCellS")) as GameObject;
        //                mine.name = "EmptyCell_";
        //                break;
        //        }

        //        mine.name += i + "_" + j;
        //        mine.transform.position = new Vector2(i * mineDim, j * mineDim);
        //        mine.transform.parent = this.transform;
        //    }
        //}
	}

    //Takes in an array of position data for mine locations
    //Generates number cells around the mines and the blank cell not next to mines. O(n*m), every cell checks every mine
    void GenerateUsingMines(Vector2[] mines)
    {
        GameObject ms = new GameObject();
        ms.name = "Mines";
        ms.transform.parent = this.transform;

        GameObject blanks = new GameObject();
        blanks.name = "Blanks";
        blanks.transform.parent = this.transform;

        GameObject numCells = new GameObject();
        numCells.name = "NumCells";
        numCells.transform.parent = this.transform;

        GameObject num1Cells = new GameObject();
        num1Cells.name = "Num1Cells";
        num1Cells.transform.parent = this.transform;

        GameObject num2Cells = new GameObject();
        num2Cells.name = "Num2Cells";
        num2Cells.transform.parent = this.transform;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int surroundingMines = 0;
                Vector2[] surrounding = new Vector2[8];
                bool exitLoop = false;
                for (int k = 0; k < mines.Length && !exitLoop; k++)
                {
                    
                    Vector2 currentMine = mines[k];
                    int dx = (int)Mathf.Abs(i - currentMine.x);
                    int dy = (int)Mathf.Abs(j - currentMine.y);
                    if (dx == 0 && dy == 0)
                    {
                        GameObject mine = Instantiate(Resources.Load("MineCellS")) as GameObject;
                        mine.name = "MineCellS_" + i + "_" + j;
                        mine.transform.position = new Vector2(i * mineDim, j * mineDim);
                        mine.transform.parent = ms.transform;
                        exitLoop = true;
                        //k = mines.Length;
                    }
                    else if (dx <= 1 && dy <= 1)
                    {
                        if (surroundingMines == 5)
                        {
                            Debug.Log(surroundingMines);
                            Debug.Log("x1: " + i + ", x2: " + currentMine.x + ", dx: " + dx);
                            Debug.Log("y1: " + j + ", y2: " + currentMine.y + ", dy: " + dy);
                        }
                        surrounding[surroundingMines] = currentMine;
                        surroundingMines++;
                    }
                }

                if (!exitLoop)
                {
                    if (surroundingMines == 0)
                    {
                        GameObject mine = Instantiate(Resources.Load("EmptyCellS")) as GameObject;
                        mine.name = "cell_" + i + "_" + j;
                        mine.transform.position = new Vector2(i * mineDim, j * mineDim);
                        mine.transform.parent = blanks.transform;
                    }
                    else
                    {
                        
                        GameObject mine = Instantiate(Resources.Load("NumberCellS" + surroundingMines)) as GameObject;
                        mine.name = "NumberCellS" + surroundingMines + "_" + i + "_" + j;
                        mine.transform.position = new Vector2(i * mineDim, j * mineDim);
                        mine.AddComponent<NumberCell>();
                        mine.GetComponent<NumberCell>().surroundingMines = surroundingMines;
                        mine.GetComponent<NumberCell>().surrounding = surrounding;
                        if (surroundingMines == 2)
                        {
                            mine.transform.parent = num2Cells.transform;
                        }
                        else
                        {
                            mine.transform.parent = numCells.transform;
                        }
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
