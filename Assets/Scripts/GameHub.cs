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

<<<<<<< HEAD
    public enum GameState { gettingMines, gameOver, gameWon, inProgress, unknown, startGame };
=======
    public enum GameState { gettingMines, gameOver, gameWon, inProgress, unknown };

    public GameState currentState = GameState.unknown;
>>>>>>> origin/Zack

    public GameState currentState = GameState.unknown;

    public int cellsLeft = 0;
    public string locationData = "";

    public static int MAX_BOMBS = 90;
    public Vector2[] mines = new Vector2[MAX_BOMBS];
    public int minesIndex = 0;
	// Use this for initialization
	void Start () {
        GameObject camera = GameObject.Find("Camera");
        camera.name = "Camera";
        Vector3 pos = camera.transform.position;
        camera.transform.position = new Vector3(width * mineDim / 2, height * mineDim / 2, pos.z);
        camera.GetComponent<Camera>().orthographicSize = (width < height) ? (height * CAMERA_V_SCALE) : (width * CAMERA_H_SCALE);

        currentState = GameState.gettingMines;
	}

    private void startGame()
    {
<<<<<<< HEAD
        mines = mines.Distinct().ToArray<Vector2>();
=======
        int numPoints = 99;
        cellsLeft = 0;
        Vector2[] points = new Vector2[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector2(Random.Range(0, width), Random.Range(0, height));
        }
        Vector2[] distinct = points.Distinct().ToArray<Vector2>();
>>>>>>> origin/Zack

        GenerateUsingMines();
    }

    //Takes in an array of position data for mine locations
    //Generates number cells around the mines and the blank cell not next to mines. O(n*m), every cell checks every mine
    private void GenerateUsingMines()
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

<<<<<<< HEAD
        minesIndex = 0;
        mines = new Vector2[MAX_BOMBS];
=======
>>>>>>> origin/Zack
        currentState = GameState.inProgress;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentState == GameState.inProgress)
        {
            if (Input.GetKeyDown("q"))
            {
                Debug.Log("Restarting Game");
                currentState = GameState.gameOver;

                var children = new List<GameObject>();
                foreach (Transform child in transform) children.Add(child.gameObject);
                children.ForEach(child => Destroy(child));

                currentState = GameState.gettingMines;
            }
        }
        if (currentState == GameState.gettingMines)
        {
            if (Input.GetKeyDown("r"))
            {
                BuildMineFromRandom();
                
            }
            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                Debug.Log("Zero Entered");
                BuildMineFromTwitch(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                Debug.Log("One Entered");
                BuildMineFromTwitch(false);
            }
        }
        else if (currentState == GameState.startGame)
        {
<<<<<<< HEAD
            currentState = GameState.inProgress;
=======
            Debug.Log("Restarting Game");
            currentState = GameState.gameOver;

            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
>>>>>>> origin/Zack

            startGame();
        }
	}

    //May produces less than the max number of mines, if there are repeats
    private void BuildMineFromRandom()
    {
        AddMineToMines(new Vector2(Random.Range(0, width), Random.Range(0, height)));
    }

    public void BuildMineFromTwitch(bool zero)
    {
        locationData += ((zero) ? ("0") : ("1"));
        if (locationData.Length == 9)
        {
            AddMineToMines(BinaryToVector2d(locationData));
            locationData = "";
        }
    }

    public void AddMineToMines(Vector2 mine)
    {
        mines[minesIndex] = mine;
        minesIndex++;
        if (minesIndex == MAX_BOMBS)
        {
            currentState = GameState.startGame;//!!!
        }
    }

    public Vector2 BinaryToVector2d(string b)
    {
        int x = 0;
        int y = 0;
        for (int i = 0; i < locationData.Length; i++)
        {
            // we start with the least significant digit, and work our way to the left
            if (locationData[locationData.Length - i - 1] == '0') continue;
            if (i < 5)
            {
                x += (int)Mathf.Pow(2, i);
            }
            else
            {
                y += (int)Mathf.Pow(2, (i - 5));
            }
            
        }
        Debug.Log("X: " + x + ", Y: " + y);
        return new Vector2(x, y);
    }
}
