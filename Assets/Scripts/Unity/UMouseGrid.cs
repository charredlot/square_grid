using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UMouseGrid {
	public float RAY_DISTANCE_MAX = 20.0f;
	
	private HashSet<GameObject> terrain;
	private USquareGrid grid;
	
	public UMouseGrid(USquareGrid grid, HashSet<GameObject> terrain) {
		this.grid = grid;
		this.terrain = terrain;
	}

	public bool RaycastHitMatches(RaycastHit hit) {
		return this.terrain.Contains(hit.collider.gameObject);
	}

	public USquareGridSquare FindSquare(Vector3 mouse_pos) {
		var ray = Camera.main.ScreenPointToRay(mouse_pos);
		RaycastHit best_hit;
		best_hit = Hacks.GetRaycastClosest(ray, this.RaycastHitMatches); 
		if (best_hit.Equals(default(RaycastHit))) {
			return null;
		} else {
			return this.grid.GetSquare(best_hit.point);		
		}
	}
}
