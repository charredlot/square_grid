using UnityEngine;
using System.Collections;

public class UUnitT : USquareGridUnit {
	public const string PREFAB_FILE = "t_unit.prefab";
	
	public UUnitT(UnitID id, PrefabCache cache) : 
		base(id, cache, UUnitT.PREFAB_FILE) {
		this.move_range = 4;
		this.jump_range = 1;
	}
	
	public override int GetSpeed() {
		return 10;
	}
}