using System.Collections.Generic;

public abstract class SquareGridUnit : UnitSchedulable {
	public readonly UnitID id;
	public SquareGridSquare Square { get { return this.square; } }
	public int move_range;
	public int jump_range;

	protected SquareGridSquare square;
	
	public SquareGridUnit(UnitID id, SquareGridSquare square = null) {
		this.id = id;
		this.square = square;
		this.move_range = 1;
		this.jump_range = 1;
	}

	public virtual void Moved(SquareGridSquare target) {
		this.square = target;
	}

	public virtual void BeginTurn()
	{
	}

	public virtual SquareGridArea GetMoveableArea(SquareGrid grid) {
		HashSet<SquareGridSquare> in_range;

		/* meh improve later */
		in_range = new HashSet<SquareGridSquare>();

		/* clear all the tmp vertex and build the hashset */
		foreach (var s in grid.GetAdjacentRadius(this.square, this.move_range)) {
			in_range.Add(s);
			s.TmpVertex.Reset();
		}

		/* connect everything together for the graph */
		foreach (var s in in_range) {
			foreach (var adj in s.Vertex.Adjacent) {
				if (in_range.Contains(adj)) {
					/* TODO: jump check */
					s.TmpVertex.AddAdjacent(adj.TmpVertex);
				}
			}
		}

		/* add everything from unit's current position as special case */
		foreach (var adj in this.square.Vertex.Adjacent) {
			if (in_range.Contains(adj)) {
				this.square.TmpVertex.AddAdjacent(adj.TmpVertex);
				adj.TmpVertex.AddAdjacent(this.square.TmpVertex);
			}
		}

		return new SquareGridArea(
			in_range, this.square);
	}

	public abstract int GetSpeed();
}
