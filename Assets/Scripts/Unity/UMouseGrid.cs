using UnityEngine;
using System.Collections;

public class UMouseGrid {
	public float RAY_DISTANCE_MAX = 20.0f;
	
	private Collider ground;
	private USquareGrid grid;
	
	public UMouseGrid(USquareGrid grid, GameObject ground_obj) {
		this.grid = grid;
		this.ground = ground_obj.collider;
	}

	public USquareGridSquare FindSquare(Vector3 mouse_pos) {
		var ray = Camera.main.ScreenPointToRay(mouse_pos);
		RaycastHit hit_info;
		
		if (this.ground.Raycast(ray, out hit_info, this.RAY_DISTANCE_MAX)) {
			Vector3 p;

			//Debug.Log (p);
			//Debug.Log (this.grid.GetSquare(p));
			p = ray.GetPoint (hit_info.distance);
			return this.grid.GetSquare(p);
		} else {
			return null;
		}
	}
}
