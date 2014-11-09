using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour, ICell {
    public Vector2 pos;
    public bool isDeselected = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    virtual public void OnSelected()
    {
        Debug.Log("Cell - OnSelected");
    }

    public void Unselect()
    {
        GameObject unselect = GameObject.Find(this.name + "/Offset/Unselected");
        SpriteRenderer sr = unselect.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 0;
        isDeselected = true;
    }

    void OnMouseDown()
    {
        Debug.Log("Cell - OnMouseDown");
        this.OnSelected();
    }
}
