using UnityEngine;
using System.Collections;

public struct UUnitInit {
	public readonly UnitID id;
	public readonly int row;
	public readonly int col;
	public readonly System.Type unit_class;
	
	public UUnitInit(UnitID id, System.Type unit_class, int row, int col) {
		this.id = id;
		this.unit_class = unit_class;
		this.row = row;
		this.col = col;
	}
};

public abstract class USquareGridUnit : SquareGridUnit {
	private GameObject g;
	private float half_height;

	public USquareGridUnit(UnitID id, PrefabCache cache, string prefab_file) : base(id) {
		Object prefab;

		prefab = cache.GetPrefab(prefab_file);
		this.g = (GameObject)GameObject.Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
		this.half_height = this.g.collider.bounds.extents.y;
		this.g.SetActive (false);
	}

	public override void Moved(SquareGridSquare s) {
		Vector3 new_pos;
		USquareGridSquare us = (USquareGridSquare)s;

		this.square = us;
	
		/* need to put the piece on the top of the background (TODO: gravity?) */
		new_pos = us.WorldCenterCoords;
		new_pos.y = new_pos.y + this.half_height;
		this.g.transform.position = new_pos;

		/* not sure how expensive setactive is */
		this.g.SetActive(true);
	}
}
