using UnityEngine;
using System.Collections;

public struct UnitID {
	public SquareGridPlayer player;
	public readonly string unit_instance_name;

	public UnitID(string unit_instance, SquareGridPlayer player=null) {
		this.unit_instance_name = unit_instance;
		this.player = player;
	}

	public override string ToString ()
	{
		return string.Format ("[UnitID] " + 
		       this.unit_instance_name + "(" + this.player + ")");
	}
};
