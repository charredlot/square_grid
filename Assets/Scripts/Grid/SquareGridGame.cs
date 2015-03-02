using UnityEngine;
using System.Collections;

public class SquareGridGame {
	public SquareGrid Grid { get { return this.grid; } }

	protected UnitScheduler unit_scheduler = null;
	protected SquareGrid grid = null;

	public SquareGridGame() {
		this.unit_scheduler = new UnitScheduler();
	}

	public bool MoveUnit(SquareGridUnit piece, int row, int col) {
		return this.grid.MoveUnit(piece, row, col);
	}
}
