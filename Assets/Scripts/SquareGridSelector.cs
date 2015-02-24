using UnityEngine;
using System.Collections;

public class SquareGridSelector {
	public readonly Quaternion PREFAB_ROTATION = Quaternion.AngleAxis(90, Vector3.right);

	private Object select_prefab;
	private GameObject curr_selector;

	// Use this for initialization
	public SquareGridSelector(string select_prefab_path) {
		this.select_prefab = Hacks.LoadPrefab(select_prefab_path);
		this.curr_selector = null;
	}

	Vector3 SquarePosition(SquareGridSquare s)	{
		/* x-z plane, hardcode height for now */
		return new Vector3(s.WorldCenterCoords.x, 4.0f, s.WorldCenterCoords.z);
	}

	public void SelectSquare(SquareGridSquare s) {
		GameObject selector;

		if (s == null) {
			return;
		}

		//Debug.Log ("selected " + s);

		selector = this.curr_selector;
		if (selector != null) {
			GameObject.Destroy(selector);
		}

		this.curr_selector = (GameObject)GameObject.Instantiate (this.select_prefab, 
			this.SquarePosition(s),
			this.PREFAB_ROTATION);

	}
}
