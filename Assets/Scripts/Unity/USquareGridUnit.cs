﻿using UnityEngine;
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
	private bool g_is_active;

	public USquareGridUnit(UnitID id, PrefabCache cache, string prefab_file) : base(id) {
		Object prefab;

		prefab = cache.GetUnitPrefab(prefab_file);
		this.g = (GameObject)GameObject.Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
		this.g.SetActive (false);
		this.g_is_active = false;
	}

	public override void Moved(SquareGridSquare s) {
		Vector3 new_pos;
		USquareGridSquare us = (USquareGridSquare)s;

		this.square = us;
	
		/* need to put the piece on the top of the background (TODO: gravity?) */
		new_pos = us.WorldCenterCoords;
		new_pos.y = 2.0f;
		this.g.transform.position = new_pos;

		if (!this.g_is_active) {
			/* not sure how expensive setactive is */
			this.g.SetActive(true);
			this.g_is_active = true;
		}
	}
}