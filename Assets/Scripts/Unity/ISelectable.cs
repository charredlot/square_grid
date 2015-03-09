using UnityEngine;
using System.Collections.Generic;

public interface ISelectable {
	USelectableMixin SelectableM { get; set; }
	Vector3 GetSelectorPosition();
}

/* TODO: change struct */
public class USelectableMixin {
	public ISelectable selectable;
	public GameObject g;
	public USelectableCache cache;
	
	public USelectableMixin(ISelectable selectable, GameObject g, USelectableCache cache) {
		this.selectable = selectable;
		this.g = g;
		this.cache = cache;
	}

	public bool BelongsTo(USelectableCache cache) {
		return this.cache == cache;
	}

	public void Cleanup() {
		this.cache.ClearSelectable(this);
	}
};

public class USelectableCache {
	private Object prefab;
	private Stack<GameObject> objs;
	
	public USelectableCache(Object prefab) {
		this.objs = new Stack<GameObject>();
		this.prefab = prefab;
	}
	
	private GameObject AllocGameObject(ISelectable selectable) {
		GameObject g;
		
		if (this.objs.Count == 0) {
			g = (GameObject)GameObject.Instantiate(this.prefab,
			                                       new Vector3(0,0,0), Quaternion.identity);
		} else {
			g = this.objs.Pop();
		}
		
		g.transform.position = selectable.GetSelectorPosition();
		g.SetActive(true);
		return g;
	}
	
	private void FreeGameObject(GameObject g) {
		if (g != null) {
			g.SetActive(false);
			this.objs.Push (g);
		}
	}
	
	private USelectableMixin SelectableMFactory(ISelectable selectable) {
		return new USelectableMixin(selectable, this.AllocGameObject(selectable), this);
	}
	
	private void ChangeCache(USelectableMixin selm) {
		this.ClearSelectable(selm);
		selm.cache = this;
		selm.g = this.AllocGameObject(selm.selectable);
	}
	
	public USelectableMixin Select(ISelectable selectable) {
		var selm = selectable.SelectableM;
		
		if (selm == null) {
			selectable.SelectableM = this.SelectableMFactory(selectable);
		} else {
			this.ChangeCache(selm);
		}

		return selectable.SelectableM;
	}
	
	public void ClearSelectable(USelectableMixin sel) {
		if (sel.g != null) {
			sel.cache.FreeGameObject(sel.g);
			sel.g = null;
		}	
	}
};
