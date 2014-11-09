using UnityEngine;
using System.Collections;

public class GameInit : MonoBehaviour {
    public int width = 128;
    public int height = 128;
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

        Vector2[] points = { 
                               new Vector2(1, 1), 
                               new Vector2(2, 2), 
                               new Vector2(10, 10), 
                               new Vector2(11, 10), 
                           };

        GenerateUsingMines(points);

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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int surroundingMines = 0;
                bool exitLoop = false;
                for (int k = 0; k < mines.Length && !exitLoop; k++)
                {
                    Vector2 currentMine = mines[k];
                    int dx = (int)Mathf.Abs(i - currentMine.x);
                    int dy = (int)Mathf.Abs(j - currentMine.y);
                    if (dx == 0 && dy == 0)
                    {
                        GameObject mine = Instantiate(Resources.Load("MineCellS")) as GameObject;
                        mine.name = "cell_" + i + "_" + j;
                        mine.transform.position = new Vector2(i * mineDim, j * mineDim);
                        mine.transform.parent = this.transform;
                        exitLoop = true;
                        //k = mines.Length;
                    }
                    else if (dx <= 1 && dy <= 1)
                    {
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
                        mine.transform.parent = this.transform;
                    }
                    else
                    {
                        GameObject mine = Instantiate(Resources.Load("Number" + surroundingMines + "CellS")) as GameObject;
                        mine.name = "cell_" + i + "_" + j;
                        mine.transform.position = new Vector2(i * mineDim, j * mineDim);
                        mine.transform.parent = this.transform;
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
