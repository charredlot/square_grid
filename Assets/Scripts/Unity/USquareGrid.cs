using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USquareGrid : SquareGrid {
	public readonly Vector3 bottom_left;
	public const float MAX_HEIGHT = 30.0f;

	private float square_len;
	private HashSet<GameObject> terrain;

	public USquareGrid(int num_rows, int num_cols,
	                   float square_len,
	                   HashSet<GameObject> terrain)
	{
		this.bottom_left = new Vector3(
			- (num_cols / 2) * square_len, 0,
			- (num_rows / 2) * square_len);
		this.square_len = square_len;
		this.terrain = terrain;

		this.InitSquares(num_rows, num_cols);

		this.PrintBounds(); // debug
	}

	public bool IsRaycastHitTerrain(RaycastHit hit) {
		return this.terrain.Contains (hit.collider.gameObject);
	}

	protected float FindHeightBelow(Vector3 coords) {
		var ray = new Ray(coords, Vector3.down);


		var hit = Hacks.GetRaycastClosest(ray, this.IsRaycastHitTerrain);
		if (hit.Equals(default(RaycastHit))) {
			Debug.Log ("can't find terrain for square at " + coords);
			return 0.0f;
		} else {
			return hit.point.y;
		}
	}

	void PrintBounds() {
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
		float half_len;
		float x;
		float height;
		float z;

		half_len = this.square_len / 2.0f;
		x = this.bottom_left.x + (col*this.square_len) + half_len;
		z = this.bottom_left.z + (row*this.square_len) + half_len;

		height = this.FindHeightBelow(new Vector3(x, USquareGrid.MAX_HEIGHT, z));

		return new USquareGridSquare(row, col, height,
		                             new Vector3(x, height, z));
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
