using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USquareGrid : SquareGrid {
	public readonly Vector3 bottom_left;
	private float square_len;

	public USquareGrid(int num_rows, int num_cols, float square_len)
	{
		this.bottom_left = new Vector3(
			- (num_cols / 2) * square_len, 0,
			- (num_rows / 2) * square_len);
		this.square_len = square_len;

		this.InitSquares(num_rows, num_cols);

		/* -- debug bounds -- */
		USquareGridSquare us;
		Vector3 bot_left;
		Vector3 top_right;
		us = (USquareGridSquare)this.GetSquare (0,0);
		bot_left = us.WorldCenterCoords;
		us = (USquareGridSquare)this.GetSquare(this.Rows - 1, this.Cols - 1);
		top_right = us.WorldCenterCoords;
		Debug.Log ("Bounds: " + bot_left + " " + top_right);
	}

	protected override SquareGridSquare AllocSquare(int row, int col) {
		float half_len = this.square_len / 2.0f;

		/* TODO: figure out height */
		return new USquareGridSquare(row, col, 0, new Vector3(
			this.bottom_left.x + (col*this.square_len) + half_len,
			0,
			this.bottom_left.z + (row*this.square_len) + half_len)
		);
	}

	public USquareGridSquare GetSquare(Vector3 world_coords)
	{
		/* grid is x-z plane for now */
		int row;
		int col;

		row = (int)((world_coords.z - this.bottom_left.z) / this.square_len);
		col = (int)((world_coords.x - this.bottom_left.x) / this.square_len);

		return (USquareGridSquare)this.GetSquare (row, col);
	}
}
