using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USelectableCache {
	private Object prefab;
	private Stack<GameObject> objs;

	public USelectableCache(Object prefab) {
		this.objs = new Stack<GameObject>();
		this.prefab = prefab;
	}

	private GameObject AllocGameObject(USquareGridSquare us) {
		GameObject g;

		if (this.objs.Count == 0) {
			g = (GameObject)GameObject.Instantiate(this.prefab,
			                                   new Vector3(0,0,0), Quaternion.identity);
		} else {
			g = this.objs.Pop();
		}

		g.transform.position = USelectableCache.PositionFromSquare(us);
		g.SetActive(true);
		return g;
	}

	private void FreeGameObject(GameObject g) {
		if (g != null) {
			g.SetActive(false);
			this.objs.Push (g);
		}
	}

	private USelectableMixin SelectableFactory(USquareGridSquare us) {
		return new USelectableMixin(this.AllocGameObject(us), this);
	}

	private void ChangeCache(USquareGridSquare us, USelectableMixin sel) {
		this.ClearSelectable(sel);
		sel.cache = this;
		sel.g = this.AllocGameObject(us);
	}

	/* TODO: make part of the selectable interface */
	private static Vector3 PositionFromSquare(USquareGridSquare us)	{
		/* x-z plane, hardcode height for now */
		return new Vector3(us.WorldCenterCoords.x, 4.0f, us.WorldCenterCoords.z);
	}
	
	public void SelectSquare(USquareGridSquare us) {
		var sel = us.Selectable;
		
		if (sel == null) {
			us.Selectable = this.SelectableFactory(us);
		} else {
			this.ChangeCache(us, sel);
		}
	}

	public void ClearSelectable(USelectableMixin sel) {
		if (sel.g != null) {
			sel.cache.FreeGameObject(sel.g);
			sel.g = null;
		}	
	}
};

public class USquareGridSelector {
	/* for inspector */
	public readonly string ACTIVE_UNIT_PREFAB = "SelectActiveUnit.prefab";
	public readonly string MOVEABLE_PREFAB = "HighlightMoveableSquare.prefab";
	public readonly string MOVE_TARGET_PREFAB = "HighlightMoveTarget.prefab";

	public enum SelectionType {
		NONE,
		ACTIVE_UNIT,
		MOVEABLE,
		MOVE_TARGET,
	};

	private PrefabCache prefab_cache;

	private USelectableCache active_unit_cache;
	private USelectableMixin active_unit_selected;

	private USelectableCache moveable_cache;
	private IList<USelectableMixin> moveable;

	private USelectableCache move_target_cache;
	private USelectableMixin move_target_selected;

	// Use this for initialization
	public USquareGridSelector(PrefabCache prefab_cache) {
		this.prefab_cache = prefab_cache;

		this.active_unit_cache =
			new USelectableCache(this.prefab_cache.GetPrefab(this.ACTIVE_UNIT_PREFAB));
		this.moveable_cache =
			new USelectableCache(this.prefab_cache.GetPrefab(this.MOVEABLE_PREFAB));
		this.move_target_cache = 
			new USelectableCache(this.prefab_cache.GetPrefab(this.MOVE_TARGET_PREFAB));

		this.moveable = new List<USelectableMixin>();
	}

	public USelectableMixin SelectSquare(USquareGridSquare us, USelectableCache cache) {
		cache.SelectSquare(us);
		return us.Selectable;
	}

	public void SelectActiveUnitSquare(USquareGridSquare us, USquareGridUnit unit) {
		if (this.active_unit_selected != null) {
			this.active_unit_selected.Cleanup();
			this.active_unit_selected = null;
		}
		this.active_unit_selected = this.SelectSquare (us, this.active_unit_cache);
	}

	public void SelectMoveTarget(USquareGridSquare us) {
		if (this.move_target_selected != null) {
			this.move_target_selected.Cleanup();
			this.move_target_selected = null;
		}
		this.move_target_selected = this.SelectSquare (us, this.move_target_cache);
	}

	public void HighlightMoveableSquares(IEnumerable<USquareGridSquare> squares) {
		if (squares == null) {
			return;
		}

		/* TODO: we can reuse the projectors, we just need to store them */
		foreach (USelectableMixin sel in this.moveable) {
			sel.Cleanup();
		}
		this.moveable.Clear();

		foreach (USquareGridSquare us in squares) {
			this.SelectSquare (us, this.moveable_cache);
			this.moveable.Add (us.Selectable);
		}
	}
}
