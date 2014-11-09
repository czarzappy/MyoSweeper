using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameHub : MonoBehaviour {
    public int width;
    public int height;
    public float mineDim = 2.56f;
    private static float CAMERA_V_SCALE = 41f / 32f;
    private static float CAMERA_H_SCALE = 24f / 32f;

    private enum GameState { };

    public int cellsLeft;
	// Use this for initialization
	void Start () {
        GameObject camera = GameObject.Find("Camera");
        camera.name = "Camera";
        Vector3 pos = camera.transform.position;
        camera.transform.position = new Vector3(width * mineDim / 2, height * mineDim / 2, pos.z);
        camera.GetComponent<Camera>().orthographicSize = (width < height) ? (height * CAMERA_V_SCALE) : (width * CAMERA_H_SCALE);

        startGame();
	}

    private void startGame()
    {
        int numPoints = 99;
        Vector2[] points = new Vector2[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector2(Random.Range(0, width), Random.Range(0, height));
        }
        Vector2[] distinct = points.Distinct().ToArray<Vector2>();

        GenerateUsingMines(distinct);
    }

    //Takes in an array of position data for mine locations
    //Generates number cells around the mines and the blank cell not next to mines. O(n*m), every cell checks every mine
    private void GenerateUsingMines(Vector2[] mines)
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
                GameObject cell = null;
                bool exitLoop = false;
                for (int k = 0; k < mines.Length && !exitLoop; k++)
                {
                    Vector2 currentMine = mines[k];
                    int dx = (int)Mathf.Abs(i - currentMine.x);
                    int dy = (int)Mathf.Abs(j - currentMine.y);
                    if (dx == 0 && dy == 0)
                    {
                        cell = Instantiate(Resources.Load("MineCell")) as GameObject;
                        cell.name = "MineCell_";
                        cell.transform.parent = ms.transform;
                        cell.AddComponent<MineCell>();

                        exitLoop = true;
                    }
                    else if (dx <= 1 && dy <= 1)
                        surroundingMines++;
                }

                if (!exitLoop)
                {
                    if (surroundingMines == 0)
                    {
                        cell = Instantiate(Resources.Load("EmptyCell")) as GameObject;
                        cell.name = "Cell_";
                        cell.transform.parent = blanks.transform;
                        cell.AddComponent<EmptyCell>();
                    }
                    else
                    {
                        cell = Instantiate(Resources.Load("NumberCellS" + surroundingMines)) as GameObject;
                        cell.name = "Cell_";
                        
                        cell.AddComponent<NumberCell>();
                        cell.GetComponent<NumberCell>().surroundingMines = surroundingMines;
                        cell.transform.parent = numCells.transform;
                    }

                    cellsLeft++;
                }

                cell.name += i + "_" + j;
                cell.GetComponent<Cell>().pos = new Vector2(i, j);
                cell.transform.position = new Vector2(i * mineDim, j * mineDim);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("r"))
        {
            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));

            startGame();
        }
	}

    void OnGUI()
    {
    }
}
