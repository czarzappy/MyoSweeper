using UnityEngine;
using System.Collections;

public class NumberCell : Cell {
    public int surroundingMines;
    public Vector2[] surrounding;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void OnSelected()
    {
        this.Unselect();
    }
}
