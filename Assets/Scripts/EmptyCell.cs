using UnityEngine;
using System.Collections;

public class EmptyCell : Cell {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void OnSelected()
    {
        Debug.Log("EmptyCell - OnSelected");

        Deselect(this, new ArrayList());
    }

    private ArrayList Deselect(Cell c, ArrayList deselected)
    {
        Debug.Log("EmptyCell - Deselect: Cell - " + c.name);
        c.Unselect();
        deselected.Add(c);
        GameHub game = GameObject.Find("GameView").GetComponent<GameHub>();
        Debug.Log("Left: " + game.cellsLeft);
        game.cellsLeft--;

        if (c is NumberCell)
        {
            Debug.Log("Deselecting NumberCell");
            //Escape case
        }
        else
        {
            Debug.Log("Deselecting EmptyCell");

            Cell top = GetCellByDisplacement(c, 0, 1);
            if (top != null && !top.isDeselected)
            {
                deselected = Deselect(top, deselected);
            }
            Cell left = GetCellByDisplacement(c, -1, 0);
            if (left != null && !left.isDeselected)
            {
                deselected = Deselect(left, deselected);
            }
            Cell right = GetCellByDisplacement(c, 1, 0);
            if (right != null && !right.isDeselected)
            {
                deselected = Deselect(right, deselected);
            }
            Cell bottom = GetCellByDisplacement(c, 0, -1);
            if (bottom != null && !bottom.isDeselected)
            {
                deselected = Deselect(bottom, deselected);
            }
            Cell topRight = GetCellByDisplacement(c, 1, 1);
            if (topRight != null && !topRight.isDeselected)
            {
                deselected = Deselect(topRight, deselected);
            }
            Cell topLeft = GetCellByDisplacement(c, -1, 1);
            if (topLeft != null && !topLeft.isDeselected)
            {
                deselected = Deselect(topLeft, deselected);
            }
            Cell bottomRight = GetCellByDisplacement(c, 1, -1);
            if (bottomRight != null && !bottomRight.isDeselected)
            {
                deselected = Deselect(bottomRight, deselected);
            }
            Cell bottomLeft = GetCellByDisplacement(c, -1, -1);
            if (bottomLeft != null && !bottomLeft.isDeselected)
            {
                deselected = Deselect(bottomLeft, deselected);
            }
        }
        return deselected;
    }

    private Cell GetCellByDisplacement(Cell c, int dx, int dy)
    {
        int x = (int)c.pos.x + dx;
        int y = (int)c.pos.y + dy;
        if (InBounds(x, y))
            return GameObject.Find("Cell_" + x + "_" + y).GetComponent<Cell>();
        else
            return null;
    }

    private bool InBounds(int x, int y)
    {
        GameHub game = GameObject.Find("GameView").GetComponent<GameHub>();
        return !(x >= game.width || x < 0 || y >= game.height || y < 0);
    }
}
