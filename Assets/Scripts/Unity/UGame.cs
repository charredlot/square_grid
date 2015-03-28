using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UGame : MonoBehaviour {
	/* For inspector */
	public int NUM_ROWS = 8;
	public int NUM_COLS = 10;
	public GameObject UNIT_SIZE_CUBE;
	public GameObject TERRAIN;


	private UMouseGrid mgrid;
	private UUIManager ui_mgr;
	private PrefabCache prefab_cache;

	private USquareGridGame game;
	private USquareGridSelector selector;
	private USquareGridSquare prev_square;

	private enum SelectContext {
		ACTIVE_UNIT,
		MOVE,
		ATTACK,
	};
	private delegate void SelectAction(USquareGridSquare us);
	private UGame.SelectContext select_context;	
	private UGame.SelectAction curr_select_action;
	private SquareGridArea curr_move_area;
	private bool input_enabled;

	// Use this for initialization
	void Start () {
		GameObject g;

		this.prefab_cache = new PrefabCache();

		g = (GameObject)GameObject.Find ("level_globals");
		this.ui_mgr = (UUIManager)g.GetComponent("UUIManager");

		/* iterating through transform doesn't work well with linq */
		var terrain = new HashSet<GameObject>();
		foreach (Transform t in this.TERRAIN.transform) {
			terrain.Add(t.gameObject);
		}

		this.game = new USquareGridGame(this.prefab_cache, this.NUM_ROWS, this.NUM_COLS, 
		                                this.UNIT_SIZE_CUBE.renderer.bounds.size.x,
		                                terrain);

		this.mgrid = new UMouseGrid(this.game.UGrid, terrain);

		this.InitSelector();
		this.input_enabled = true;
	}

	void InitSelector() {	
		this.selector = new USquareGridSelector(this.prefab_cache);
		this.select_context = UGame.SelectContext.ACTIVE_UNIT;
		this.curr_select_action = this.SelectActiveUnit;
	}
	
	// Update is called once per frame
	void Update () {
		USquareGridSquare us;

		if (this.input_enabled) {
			if (Input.GetMouseButtonDown(UKeyBinds.LEFT_CLICK)) {
				us = this.mgrid.FindSquare(Input.mousePosition);
				this.curr_select_action(us);
			} else if (Input.GetMouseButtonDown(UKeyBinds.RIGHT_CLICK)) {
				this.RightSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
			}
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
		if ((unit != null) && (this.game.UActiveUnit == unit)) {			
			this.ui_mgr.unit_action_panel.SetActive (true);
		}
		
		this.selector.SelectActiveUnit(us);
	}

	private IEnumerator MoveUnit(USquareGridUnit unit, USquareGridSquare us) {
		/* FIXME: since our distances are so small, doesn't matter but we can cache this result earlier */
		var moveable = unit.GetMoveableArea(this.game.Grid);

		moveable.ResetForSearch();
		Debug.Log ("boop---");
		foreach (var tmp in moveable.GetPath(us)) {
			//Debug.Log (tmp);
			this.game.MoveUnit(unit, tmp);
			yield return new WaitForSeconds(0.5f);
		}
		Debug.Log ("done---");
		
		this.ui_mgr.unit_action_move.button.interactable = false;
		this.ui_mgr.unit_action_move.text.text = "Move";
		this.selector.HighlightMoveableSquares(null);
		this.select_context = UGame.SelectContext.ACTIVE_UNIT;
		this.curr_select_action = this.SelectActiveUnit;

		this.input_enabled = true;
	}

	private void SelectMoveTarget(USquareGridSquare us) {
		if (us == null) {
			return;
		}

		this.UISquareSelected(us);

		if (this.curr_move_area == null) {
			Debug.Log ("arrrrrgh");
			return;
		}

		if (!this.curr_move_area.Squares.Contains(us)) {
			return;
		}

		if (this.prev_square == us) {
			/* disable input while waiting for move animation and cleanup */
			this.input_enabled = false;
			this.StartCoroutine(this.MoveUnit(this.game.UActiveUnit, us));
		} else {
			this.selector.SelectMoveTarget(us);
			this.prev_square = us;
		}
	}
	
	public void RightSelectedSquare(USquareGridSquare us) {
	}
	
	public void UIMoveButton() {
		if (!this.input_enabled) {
			return;
		}

		if (this.select_context == UGame.SelectContext.MOVE) {
			this.ui_mgr.unit_action_move.text.text = "Move";
			this.selector.HighlightMoveableSquares(null);
			
			this.select_context = UGame.SelectContext.ACTIVE_UNIT;
			this.curr_select_action = this.SelectActiveUnit;
		} else if (this.select_context == UGame.SelectContext.ACTIVE_UNIT) {
			this.ui_mgr.unit_action_move.text.text = "Cancel";

			this.curr_move_area = this.game.ActiveUnit.GetMoveableArea(this.game.Grid);

			this.selector.HighlightMoveableSquares(
				from s in this.curr_move_area.GetEnumerable() select (USquareGridSquare)s);

			this.select_context = UGame.SelectContext.MOVE;
			this.curr_select_action = this.SelectMoveTarget;
		}
	}

	public void UIEndTurnButton() {
		if (!this.input_enabled) {
			return;
		}
	}
}
