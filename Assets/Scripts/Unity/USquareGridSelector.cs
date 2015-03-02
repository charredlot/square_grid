using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USquareGridSelector {
	public readonly Quaternion PREFAB_ROTATION = Quaternion.AngleAxis(90, Vector3.right);
	public readonly string SELECTOR_PREFAB_PATH = "Assets/Prefabs/SquareProjector.prefab";

	public enum Mode {
		UNIT, MOVE, ATTACK, HELP, ATTACK_AREA, HELP_AREA
	};

	private Object select_prefab;
	private USquareGridSquare curr_square;
	private GameObject selector;

	// Use this for initialization
	public USquareGridSelector() {
		this.select_prefab = Hacks.LoadPrefab(this.SELECTOR_PREFAB_PATH);
		/* TODO: figure out rotation thing */
		this.selector = (GameObject)GameObject.Instantiate(this.select_prefab,
			new Vector3(0,0,0), this.PREFAB_ROTATION);
		this.selector.SetActive(false);
	}

	Vector3 SelectorPosition(SquareGridSquare s)	{
		USquareGridSquare us = (USquareGridSquare)s;

		/* x-z plane, hardcode height for now */
		return new Vector3(us.WorldCenterCoords.x, 4.0f, us.WorldCenterCoords.z);
	}

	public void SelectUnit(USquareGridSquare us) {
		this.selector.transform.position = this.SelectorPosition (us);
		this.selector.SetActive(true);
	}
}
