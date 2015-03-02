using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabCache {
	static readonly string PREFAB_DIR = "Assets/Prefabs/";
	public IDictionary<string, Object> prefabs;

	public PrefabCache() {
		this.prefabs = new Dictionary<string, Object>();
	}

	public Object GetUnitPrefab(string prefab_file) {
		Object value;

		if (this.prefabs.TryGetValue(prefab_file, out value)) {
			return value;
		}

		value = Hacks.LoadPrefab(PrefabCache.PREFAB_DIR + prefab_file);
		if (value == null) {
			return null;
		}

		this.prefabs.Add (prefab_file, value);
		return value;
	}
}
