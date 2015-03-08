using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UGame : MonoBehaviour {
	/* For inspector */
	public int NUM_ROWS = 8;
	public int NUM_COLS = 10;
	public string UNIT_SQUARE_NAME = "UNIT_CUBE";
	public string GROUND_OBJ_NAME = "ground";

	private UMouseGrid mgrid;
	private USquareGridGame game;
	private USquareGridSelector selector;
	private UUIManager ui_mgr;
	private PrefabCache prefab_cache;

	// Use this for initialization
	void Start () {
		GameObject g;

		this.prefab_cache = new PrefabCache();

		g = (GameObject)GameObject.Find ("level_globals");
		this.ui_mgr = (UUIManager)g.GetComponent("UUIManager");

		g = (GameObject)GameObject.Find (this.UNIT_SQUARE_NAME);
		this.game = new USquareGridGame(this.prefab_cache, this.NUM_ROWS, this.NUM_COLS, 
		                                g.renderer.bounds.size.x);

		g = (GameObject)GameObject.Find (this.GROUND_OBJ_NAME);
		this.mgrid = new UMouseGrid(this.game.UGrid, g);
				
		this.selector = new USquareGridSelector(this.prefab_cache);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(UKeyBinds.LEFT_CLICK)) {	
			this.LeftSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
		} else if (Input.GetMouseButtonDown(UKeyBinds.RIGHT_CLICK)) {
			this.RightSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
		}
	}

	public void LeftSelectedSquare(USquareGridSquare us) {
		USquareGridUnit unit;
		
		if (us == null) {
			return;
		}
		
		unit = (USquareGridUnit)us.GetPiece();
		if ((unit != null) && (this.game.ActiveUnit == unit)) {
			this.ActiveUnitSelected();
		}
		
		this.selector.SelectActiveUnitSquare(us, unit);
	}
	
	public void RightSelectedSquare(USquareGridSquare us) {
	}

	public void ActiveUnitSelected() {
		this.ui_mgr.unit_action_panel.SetActive (true);
	}

	public void UIMoveButton() {
		this.selector.HighlightMoveableSquares(this.game.GetMoveableArea());
	}
}
