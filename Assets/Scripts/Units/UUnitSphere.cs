
public class UUnitSphere : USquareGridUnit {
	public const string PREFAB_FILE = "sphere_unit.prefab";

	public UUnitSphere(UnitID id, PrefabCache cache) : 
		base(id, cache, UUnitSphere.PREFAB_FILE) {

	}

	public override int GetSpeed() {
		return 10;
	}
}
