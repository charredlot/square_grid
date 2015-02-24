using UnityEngine;
using System.Collections;

// TODO: make interface maybe, but for now, compose
public class SquareGridPiece {
	public readonly PieceID id;
	public SquareGridSquare Square { get { return this.square; } }

	private SquareGridSquare square;

	public SquareGridPiece(PieceID id) {
		this.id = id;
		this.square = null;
	}

	public virtual void MoveTo(SquareGridSquare target) {
		this.square = target;
	}
}
