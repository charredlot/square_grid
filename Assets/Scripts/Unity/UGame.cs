using UnityEngine;
using System.Collections;

public class UGame : MonoBehaviour {
	public int NUM_ROWS = 8;
	public int NUM_COLS = 10;
	public string UNIT_SQUARE_NAME = "UNIT_CUBE";
	public string GROUND_OBJ_NAME = "ground";

	private UMouseGrid mgrid;
	private USquareGridGame game;

	// Use this for initialization
	void Start () {
		GameObject g;

		g = (GameObject)GameObject.Find (this.UNIT_SQUARE_NAME);
		this.game = new USquareGridGame(this.NUM_ROWS, this.NUM_COLS, 
		                                g.renderer.bounds.size.x);

		g = (GameObject)GameObject.Find (this.GROUND_OBJ_NAME);
		this.mgrid = new UMouseGrid(this.game.UGrid, g);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(UKeyBinds.LEFT_CLICK)) {	
			this.game.LeftSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
		} else if (Input.GetMouseButtonDown(UKeyBinds.RIGHT_CLICK)) {
			this.game.RightSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
		}
	}
}
