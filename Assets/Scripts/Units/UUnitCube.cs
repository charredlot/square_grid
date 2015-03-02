using UnityEngine;
using System.Collections;

public class UUnitCube : USquareGridUnit {
	public const string PREFAB_FILE = "cube_unit.prefab";

	public UUnitCube(UnitID id, PrefabCache cache) : 
		base(id, cache, UUnitCube.PREFAB_FILE) {
		
	}
	
	public override int GetSpeed() {
		return 10;
	}
}
