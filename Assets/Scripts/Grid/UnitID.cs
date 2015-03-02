using UnityEngine;
using System.Collections;

public struct UnitID {
	public SquareGridPlayer player;
	public readonly string unit_instance;

	public UnitID(string unit_instance, SquareGridPlayer player=null) {
		this.unit_instance = unit_instance;
		this.player = player;
	}
};
