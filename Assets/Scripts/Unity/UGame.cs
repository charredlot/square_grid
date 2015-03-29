using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct SelectContext {
	public enum States {
		ACTIVE_UNIT,
		MOVE,
		ATTACK,
	};
	public delegate void SelectAction(USquareGridSquare us);
	
	public SelectContext.States State { get { return this.state; } }
	public SelectContext.SelectAction Action { get { return this.action; } }
	public SquareGridArea move_area;
	public bool input_enabled;
	public USquareGridSquare prev_square;

	private SelectContext.States state;
	private SelectContext.SelectAction action;
	private USquareGridSelector selector;

	public SelectContext(PrefabCache prefab_cache, 
	                     SelectContext.States state, 
	                     SelectContext.SelectAction action) {
		this.selector = new USquareGridSelector(prefab_cache);
		this.move_area = null;
		this.prev_square = null;
		this.input_enabled = true;


		this.state = state;
		this.action = action;
	}

	public void Transition(SelectContext.States state, SelectContext.SelectAction action) {
		this.state = state;
		this.action = action;

		if ((this.move_area != null) && (SelectContext.States.MOVE != state)) {
			/* TODO: probably pass selector the prev move_area to cleanup */
			this.selector.HighlightMoveableSquares(null);
			this.move_area = null;
		}
	}

	public void SelectActiveUnit(USquareGridSquare us) {
		this.selector.SelectActiveUnit(us);
	}

	public void HighlightMoveableArea(SquareGridArea area) {
		if (this.move_area != null) {
			/* TODO: clean up old area? */
		}
		this.move_area = area;
		this.selector.HighlightMoveableSquares(
			from s in area.GetEnumerable() select (USquareGridSquare)s);
	}

	public void SelectMoveTarget(USquareGridSquare us) {
		this.selector.SelectMoveTarget(us);
		this.prev_square = us;
	}
};	

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
	
	private SelectContext select_context;	


	// Use this for initialization
	void Start () {
		GameObject g;

		this.prefab_cache = new PrefabCache();

		g = (GameObject)GameObject.Find ("level_globals");
		this.ui_mgr = (UUIManager)g.GetComponent("UUIManager");

		/* iterating through transform doesn't work well with linq */
		var terrain = new HashSet<GameObject>();
		this.GetAllChildrenWithColliders(terrain, this.TERRAIN);

		this.game = new USquareGridGame(this.prefab_cache, this.NUM_ROWS, this.NUM_COLS, 
		                                this.UNIT_SIZE_CUBE.renderer.bounds.size.x,
		                                terrain);

		this.mgrid = new UMouseGrid(this.game.UGrid, terrain);

	
		this.select_context = new SelectContext(this.prefab_cache,
		                                        SelectContext.States.ACTIVE_UNIT, 
		                                        this.SelectActiveUnit);
	}

	void GetAllChildrenWithColliders(HashSet<GameObject> terrain, GameObject o) {
		foreach (Transform t in o.transform) {
			if (t.gameObject.collider != null) {
				terrain.Add(t.gameObject);
			} else {
				this.GetAllChildrenWithColliders(terrain, t.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		USquareGridSquare us;

		if (this.select_context.input_enabled) {
			if (Input.GetMouseButtonDown(UKeyBinds.LEFT_CLICK)) {
				us = this.mgrid.FindSquare(Input.mousePosition);
				this.select_context.Action(us);
			} else if (Input.GetMouseButtonDown(UKeyBinds.RIGHT_CLICK)) {
				this.RightSelectedSquare(this.mgrid.FindSquare(Input.mousePosition));
			}
		}
	}

	private void UISquareSelected(USquareGridSquare us) {
		this.ui_mgr.SelectedSquare(us);
		this.ui_mgr.SelectedUnit ((USquareGridUnit)us.GetUnit ());
	}

	private void SelectActiveUnit(USquareGridSquare us) {
		USquareGridUnit unit;
		
		if (us == null) {
			return;
		}

		this.UISquareSelected(us);
		
		unit = (USquareGridUnit)us.GetUnit();
		if ((unit != null) && (this.game.UActiveUnit == unit)) {
			this.ui_mgr.unit_action_panel.SetActive (true);
			this.ui_mgr.SelectedActiveUnit(unit);
		}
		
		this.select_context.SelectActiveUnit(us);
	}

	private IEnumerator MoveUnit(USquareGridUnit unit, USquareGridSquare us) {
		/* FIXME: since our distances are so small, doesn't matter but we can cache this result earlier */
		var moveable = unit.GetMoveableArea(this.game.Grid);

		moveable.ResetForSearch();
		foreach (var tmp in moveable.GetPath(us)) {
			//Debug.Log (tmp);
			this.game.MoveUnit(unit, tmp);
			yield return new WaitForSeconds(0.5f);
		}
		Debug.Log ("---move animation done---");
		
		this.ui_mgr.unit_action_move.button.interactable = false;
		this.ui_mgr.unit_action_move.text.text = "Move";

		this.select_context.Transition (SelectContext.States.ACTIVE_UNIT, this.SelectActiveUnit);

		/**
		 * move cursor to unit's new position
		 * TODO: since we're wiping the moveable in transition,
		 * we have to do the select after. need to do something more elegant
		 */
		this.SelectActiveUnit(us);

		this.select_context.input_enabled = true;
	}

	private void SelectMoveTarget(USquareGridSquare us) {
		if (us == null) {
			return;
		}

		this.UISquareSelected(us);

		if (this.select_context.move_area == null) {
			Debug.Log ("arrrrrgh");
			return;
		}

		if (!this.select_context.move_area.Squares.Contains(us)) {
			return;
		}

		if (this.select_context.prev_square == us) {
			/* disable input while waiting for move animation and cleanup */
			this.select_context.input_enabled = false;
			this.StartCoroutine(this.MoveUnit(this.game.UActiveUnit, us));
		} else {
			this.select_context.SelectMoveTarget(us);
		}
	}
	
	public void RightSelectedSquare(USquareGridSquare us) {
	}
	
	public void UIMoveButton() {
		if (!this.select_context.input_enabled) {
			return;
		}

		if (this.select_context.State == SelectContext.States.MOVE) {
			this.ui_mgr.unit_action_move.text.text = "Move";
			this.select_context.Transition(SelectContext.States.ACTIVE_UNIT, this.SelectActiveUnit);
		} else if (this.select_context.State == SelectContext.States.ACTIVE_UNIT) {
			this.ui_mgr.unit_action_move.text.text = "Cancel";

			this.select_context.HighlightMoveableArea(
				this.game.ActiveUnit.GetMoveableArea(this.game.Grid));

			this.select_context.Transition (SelectContext.States.MOVE, this.SelectMoveTarget);
		}
	}

	public void UIEndTurnButton() {
		if (!this.select_context.input_enabled) {
			return;
		}

		this.select_context.Transition (SelectContext.States.ACTIVE_UNIT, this.SelectActiveUnit);

		/* clean up should happen here */
		this.game.ActiveUnitTurnEnded ();
		/* after this, active unit is updated */

		this.select_context.SelectActiveUnit((USquareGridSquare)this.game.ActiveUnit.Square);

		this.ui_mgr.SelectedActiveUnit(this.game.UActiveUnit);
		this.ui_mgr.SelectedUnit (this.game.UActiveUnit);
		this.ui_mgr.unit_action_panel.SetActive (true);
		this.ui_mgr.unit_action_move.button.interactable = true;
		this.ui_mgr.unit_action_move.text.text = "Move";
	}
}
