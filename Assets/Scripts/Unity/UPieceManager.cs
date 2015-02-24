using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UPieceManager : MonoBehaviour {
	static string PREFAB_DIR = "Assets/Prefabs/";
	static Dictionary<string, string> NAME_TO_PREFAB = 
		new Dictionary<string, string> {
		{ "sphere", "sphere_unit.prefab" },
		{ "cube", "cube_unit.prefab" },
	};
	public IDictionary<string, Object> prefabs;

	private SquareGrid grid;

	void Start() {
		this.LoadPrefabs ();

		StartCoroutine("SetupPieces");
	}

	void LoadPrefabs() {
		this.prefabs = new Dictionary<string, Object>();
		
		foreach (KeyValuePair<string, string> kv in UPieceManager.NAME_TO_PREFAB) {
			Object o;
			o = Hacks.LoadPrefab(UPieceManager.PREFAB_DIR + kv.Value);
			if (o == null) {
				Debug.Log ("failed to load prefab " + kv.Key + " at " + kv.Value);
			} else {
				this.prefabs.Add(kv.Key, o);
			}
		}
	}

	protected IEnumerator SetupPieces() {
		UMouseGrid mg;

		mg = (UMouseGrid)GameObject.Find(Hacks.LEVEL_GLOBALS).GetComponent("UMouseGrid");
		while (!mg.IsReady()) {
			yield return new WaitForSeconds(1.0f);
		}
		
		this.grid = mg.grid;
		
		Debug.Log ("boop");
		SquareGridPiece piece;

		piece = new UPiece(this, new PieceID("sphere", "my_sphere"));
		this.grid.AddPiece(piece, 0, 0);
	}

	public Object GetPrefab(PieceID id) {
		Object value;

		if (this.prefabs.TryGetValue(id.unit_type, out value)) {
			return value;
		} else {
			return null;
		}
	}
}
