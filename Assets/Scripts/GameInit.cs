﻿using UnityEngine;
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

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject mine;
                //string s = mine.name + "/Offset/Hidden";
                int value = Random.Range(0, 4);
                //Debug.Log(value);
                switch (value)
                {
                    case 0:
                        mine = Instantiate(Resources.Load("MineCell")) as GameObject;
                        mine.name = "MineCell_";
                        break;
                    case 1:
                        mine = Instantiate(Resources.Load("NumberCell")) as GameObject;
                        mine.name = "NumberCell_";
                        break;
                    case 2:
                        mine = Instantiate(Resources.Load("EmptyCell")) as GameObject;
                        mine.name = "EmptyCell_";
                        break;
                    default:
                        mine = Instantiate(Resources.Load("EmptyCell")) as GameObject;
                        mine.name = "EmptyCell_";
                        break;
                }

                mine.name += i + "_" + j;
                mine.transform.position = new Vector2(i * mineDim, j * mineDim);
                mine.transform.parent = this.transform;
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
