using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USquareGridSquare : SquareGridSquare {
	public readonly Vector3 WorldCenterCoords;
	public USelectableMixin Selectable;

	public USquareGridSquare(int row, int col, int height, Vector3 world_center_coords) : 
		base(row, col, height) {
		this.WorldCenterCoords = world_center_coords;
	}
	
	public override string ToString ()
	{
		return string.Format ("[Square row={0} col={1}, x={2}, y={3}, z={4}]",
		                      this.Row, this.Col,
		                      this.WorldCenterCoords.x, this.WorldCenterCoords.y, this.WorldCenterCoords.z);
	}
};
