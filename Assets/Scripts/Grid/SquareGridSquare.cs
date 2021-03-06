﻿using System.Collections.Generic;

public class SquareGridSquare {
	public readonly int Row;
	public readonly int Col;
	public readonly float Height;
	public readonly GraphableMixin<SquareGridSquare> Vertex;
	public readonly GraphableMixin<SquareGridSquare> TmpVertex;

	private List<SquareGridUnit> units;
	
	public SquareGridSquare(int row, int col, float height=0) {
		this.Row = row;
		this.Col = col;
		this.Height = SquareGridSquare.RoundHeight(height);
		this.units = new List<SquareGridUnit>();

		this.Vertex = new GraphableMixin<SquareGridSquare>(this);
		this.TmpVertex = new GraphableMixin<SquareGridSquare>(this);
	}
	
	public void AddUnit(SquareGridUnit piece)
	{
		this.units.Add(piece);
	}

	public SquareGridUnit GetUnit()
	{
		/* handle multiple units later */
		if (this.units.Count > 0) {
			return this.units[0];
		} else {
			return null;
		}
	}
	
	public IEnumerator<SquareGridUnit> GetUnits()
	{
		return this.units.GetEnumerator();
	}
	
	public int NumPieces()
	{
		return this.units.Count;
	}
	
	public override string ToString ()
	{
		return string.Format ("[Square row={0} col={1}]",
		                      this.Row, this.Col);
	}

	
	public static float RoundHeight(float h) {
		/* round to 0.5 values */
		float fractional = h - (int)h;
		if (fractional < 0.5f) {
			fractional = 0;
		} else {
			fractional = 0.5f;
		}
		
		return (int)h + fractional;
	}
};
