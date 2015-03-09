using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UGame : MonoBehaviour {
	/* For inspector */
	public int NUM_ROWS = 8;
	public int NUM_COLS = 10;
	public string UNIT_SQUARE_NAME = "UNIT_CUBE";
	public string GROUND_OBJ_NAME = "ground";
	public GameObject ENVIRONMENT_OBJ;


	private UMouseGrid mgrid;
	private USquareGridGame game;
	private USquareGridSelector selector;
	private UUIManager ui_mgr;
	private PrefabCache prefab_cache;

	private enum SelectContext {
		ACTIVE_UNIT,
		MOVE,
		ATTACK,
	};
	private delegate void SelectAction(USquareGridSquare us);
	private UGame.SelectContext select_context;	
	private UGame.SelectAction curr_select_action;

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

		/* iterating through transform doesn't work well with linq */
		var environment_objs = new List<GameObject>();
		foreach (Transform t in this.ENVIRONMENT_OBJ.transform) {
			environment_objs.Add(t.gameObject);
		}
		this.mgrid = new UMouseGrid(this.game.UGrid, environment_objs);

		this.InitSelector();
	}

	void InitSelector() {	
		this.selector = new USquareGridSelector(this.prefab_cache);
		this.select_context = UGame.SelectContext.ACTIVE_UNIT;
		this.curr_select_action = this.SelectActiveUnit;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(UKeyBinds.LEFT_CLICK)) {
			this.curr_select_action(this.mgrid.FindSquare(Input.mousePosition));
		} else if (Input.GetMouseButtonDown(UKeyBinds.RIGHT_CLICK)) {
			this.RightSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
		}
	}

	private void UISquareSelected(USquareGridSquare us) {
		this.ui_mgr.selected_square_panel.Activate(
			"Row: " + us.Row + 
			"\nCol: " + us.Col + 
			"\nHeight: " + us.Height);
	}

	private void SelectActiveUnit(USquareGridSquare us) {
		USquareGridUnit unit;
		
		if (us == null) {
			return;
		}

		this.UISquareSelected(us);
		
		unit = (USquareGridUnit)us.GetPiece();
		if ((unit != null) && (this.game.ActiveUnit == unit)) {			
			this.ui_mgr.unit_action_panel.SetActive (true);
		}
		
		this.selector.SelectActiveUnit(us);
	}

	private void SelectMoveTarget(USquareGridSquare us) {
		if (us == null) {
			return;
		}

		this.UISquareSelected(us);

		/* FIXME: check if legal move */
		this.selector.SelectMoveTarget(us);
	}
	
	public void RightSelectedSquare(USquareGridSquare us) {
	}

	/* FIXME: ugly, clean up later */
	private bool is_move_active = false;
	public void UIMoveButton() {
		this.is_move_active = !this.is_move_active;
		if (this.is_move_active) {
			this.ui_mgr.unit_action_move.text.text = "Cancel";
			this.selector.HighlightMoveableSquares(this.game.GetMoveableArea());

			this.select_context = UGame.SelectContext.MOVE;
			this.curr_select_action = this.SelectMoveTarget;
		} else {
			this.ui_mgr.unit_action_move.text.text = "Move";
			this.selector.HighlightMoveableSquares(null);

			this.select_context = UGame.SelectContext.ACTIVE_UNIT;
			this.curr_select_action = this.SelectActiveUnit;
		}
	}
}
