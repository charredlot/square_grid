using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USquareGridSelector {
	/* for inspector */
	public readonly string ACTIVE_UNIT_PREFAB = "SelectActiveUnit.prefab";
	public readonly string MOVEABLE_PREFAB = "HighlightMoveableSquare.prefab";
	public readonly string MOVE_TARGET_PREFAB = "HighlightMoveTarget.prefab";

	private PrefabCache prefab_cache;

	private USelectableCache active_unit_cache;
	private ISelectable active_unit_selected;

	private USelectableCache moveable_cache;
	private HashSet<ISelectable> moveable;

	private USelectableCache move_target_cache;
	private ISelectable move_target_selected;

	// Use this for initialization
	public USquareGridSelector(PrefabCache prefab_cache) {
		this.prefab_cache = prefab_cache;

		this.active_unit_cache =
			new USelectableCache(this.prefab_cache.GetPrefab(this.ACTIVE_UNIT_PREFAB));
		this.moveable_cache =
			new USelectableCache(this.prefab_cache.GetPrefab(this.MOVEABLE_PREFAB));
		this.move_target_cache = 
			new USelectableCache(this.prefab_cache.GetPrefab(this.MOVE_TARGET_PREFAB));

		this.moveable = new HashSet<ISelectable>();
	}

	public void SelectActiveUnit(USquareGridSquare us) {
		if (this.active_unit_selected != null) {
			this.active_unit_selected.SelectableM.Cleanup();
			this.active_unit_selected = null;
		}
		this.active_unit_cache.Select(us);
		this.active_unit_selected = us;
	}

	public void SelectMoveTarget(USquareGridSquare us) {
		var old_move_target = this.move_target_selected;

		if (old_move_target != null) {
			old_move_target.SelectableM.Cleanup();		
			/* assume old_move_target was always a legit moveable */
			this.moveable_cache.Select (old_move_target);		
		}

		/**
		 * FIXME: should be responsibility of caller to check that square is moveable,
		 * but check it here for now
		 */
		if (this.moveable.Contains (us)) {
			this.move_target_cache.Select(us);
			this.move_target_selected = us;
		}
	}

	public void HighlightMoveableSquares(IEnumerable<USquareGridSquare> squares) {
		foreach (ISelectable sel in this.moveable) {
			sel.SelectableM.Cleanup();
		}
		this.moveable.Clear();
		this.move_target_selected = null;

		if (squares != null) {
			foreach (USquareGridSquare us in squares) {
				this.moveable_cache.Select(us);
				this.moveable.Add (us);
			}
		}
	}
}
