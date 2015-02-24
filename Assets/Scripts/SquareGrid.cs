using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareGrid {
	public Vector3 bottom_left;
	private SquareGridSquare[,] squares;
	private int num_rows;
	private int num_cols;
	private float square_len;

	public SquareGrid(int num_rows, int num_cols, float square_len)
	{
		int row;
		int col;
		float half_len;

		this.bottom_left = new Vector3(
			- (num_cols / 2) * square_len,
			0,
			- (num_rows / 2) * square_len);

		this.num_rows = num_rows;
		this.num_cols = num_cols;

		this.squares = new SquareGridSquare[num_rows, num_cols];
		this.square_len = square_len;

		half_len = square_len / 2;
		for (row=0; row<num_rows; row++) {
			for (col=0; col<num_cols; col++) {
				/* grid is in x-z plane, we're in quadrant 1 so always add */
				this.squares[row, col] = new SquareGridSquare(
					new Vector2(row, col),
					new Vector3(this.bottom_left.x + (col*square_len) + half_len,
						0,
						this.bottom_left.z + (row*square_len) + half_len)
					);
			}
		}

		Vector3 bot_left = this.GetSquare(0, 0).WorldCenterCoords;
		Vector3 top_right = this.GetSquare(this.num_rows - 1, this.num_cols - 1).WorldCenterCoords;
		Debug.Log ("Bounds: " + bot_left + " " + top_right);
	}

	public SquareGridSquare GetSquare(int row, int col)
	{
		if ((row < 0) || (col < 0)) {
			return null;
		}
		if ((row >= this.num_rows) || (col >= this.num_cols)) {
			return null;
		}

		return this.squares[row, col];
	}

	public SquareGridSquare GetSquare(Vector3 world_coords)
	{
		/* grid is x-z plane for now */
		int row;
		int col;

		row = (int)((world_coords.z - this.bottom_left.z) / this.square_len);
		col = (int)((world_coords.x - this.bottom_left.x) / this.square_len);

		return this.GetSquare (row, col);
	}

	public void AddPiece(SquareGridPiece piece, int row, int col) {
		SquareGridSquare s = this.GetSquare(row, col);

		if (s != null) {
			s.AddPiece(piece);
			piece.MoveTo(s);
		}
	}
}
