using UnityEngine;
using System.Collections;

public struct PieceID {
	public readonly string unit_type;
	public readonly string unit_instance;
	public PieceID(string unit_type, string unit_instance) {
		this.unit_type = unit_type;
		this.unit_instance = unit_instance;
	}
}
