using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UMouseGrid {
	public float RAY_DISTANCE_MAX = 20.0f;
	
	private HashSet<Collider> colliders;
	private USquareGrid grid;
	
	public UMouseGrid(USquareGrid grid, IEnumerable<GameObject> terrain) {
		this.grid = grid;
		this.colliders = new HashSet<Collider>(from g in terrain select g.collider);
	}

	public USquareGridSquare FindSquare(Vector3 mouse_pos) {
		var ray = Camera.main.ScreenPointToRay(mouse_pos);
		RaycastHit[] hits;
		RaycastHit best_hit = new RaycastHit();
		float min_distance;
		bool found;

		hits = Physics.RaycastAll(ray);
		if (hits.Length == 0) {
			return null;
		}

		found = false;
		min_distance = float.MaxValue;
		foreach (RaycastHit hit in hits) {
			if (!this.colliders.Contains (hit.collider)) {
				continue;
			}

			if (!found) {
				best_hit = hit;
				min_distance = hit.distance;
				found = true;
			} else if (hit.distance < min_distance) {
				best_hit = hit;
				min_distance = hit.distance;
			}
		}

		if (!found) {
			return null;
		}

		// Debug.Log (best_hit.collider.gameObject);
		return this.grid.GetSquare(best_hit.point);
	}
}
