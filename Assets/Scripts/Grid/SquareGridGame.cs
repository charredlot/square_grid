using UnityEngine;
using System.Collections.Generic;

public class SquareGridGame {
	public SquareGrid Grid { get { return this.grid; } }
	public SquareGridUnit ActiveUnit { get { return (SquareGridUnit)this.unit_scheduler.ActiveUnit; } }

	protected UnitScheduler unit_scheduler = null;
	protected SquareGrid grid = null;

	public SquareGridGame() {
		this.unit_scheduler = new UnitScheduler();
	}

	public virtual bool MoveUnit(SquareGridUnit piece, SquareGridSquare s) {
		return this.grid.MoveUnit(piece, s);
	}

	public virtual bool MoveUnit(SquareGridUnit piece, int row, int col) {
		return this.grid.MoveUnit(piece, row, col);
	}

	public bool CanMoveTo(SquareGridUnit unit, SquareGridSquare square) {
		return true;
	}

	public void ActiveUnitTurnEnded() {
		this.unit_scheduler.TurnEnded (this.ActiveUnit);
	}
}
