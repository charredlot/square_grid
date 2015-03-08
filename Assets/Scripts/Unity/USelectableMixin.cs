using UnityEngine;
using System.Collections;

/* TODO: change struct */
public class USelectableMixin {
	public GameObject g;
	public USelectableCache cache;
	
	public USelectableMixin(GameObject g, USelectableCache cache) {
		this.g = g;
		this.cache = cache;
	}

	public bool BelongsTo(USelectableCache cache) {
		return this.cache == cache;
	}

	public void Cleanup() {
		this.cache.ClearSelectable(this);
	}
}
