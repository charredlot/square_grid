using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareGridSquare {
	public readonly Vector2 GridCoords;
	public readonly Vector3 WorldCenterCoords;
	
	private IList<SquareGridPiece> pieces;
	
	public SquareGridSquare(Vector2 grid_coords, Vector3 world_center_coords) {
		this.GridCoords = grid_coords;
		this.WorldCenterCoords = world_center_coords;
		this.pieces = new List<SquareGridPiece>();
	}
	
	public void AddPiece(SquareGridPiece piece)
	{
		this.pieces.Add(piece);
	}
	
	public IEnumerator<SquareGridPiece> GetPieces()
	{
		return this.pieces.GetEnumerator();
	}
	
	public override string ToString ()
	{
		return string.Format ("[Square row={0} col={1}, x={2}, y={3}, z={4}]",
		                      this.GridCoords.x, this.GridCoords.y,
		                      this.WorldCenterCoords.x, this.WorldCenterCoords.y, this.WorldCenterCoords.z);
	}
};
