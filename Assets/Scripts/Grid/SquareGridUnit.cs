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

	protected bool CanMoveAdjacent(SquareGridSquare curr, SquareGridSquare adj) {
		return (this.jump_range >= System.Math.Abs(adj.Height - curr.Height));
	}

	public virtual SquareGridArea GetMoveableArea(SquareGrid grid) {
		HashSet<SquareGridSquare> in_range;
		Queue<SquareGridSquare> q;
		int depth;
		int max_depth;
		int squares_in_next_depth;
		int squares_in_curr_depth;

		/* meh improve later */
		in_range = new HashSet<SquareGridSquare>();
		foreach (var s in grid.GetAdjacentRadius(this.square, this.move_range)) {
			s.TmpVertex.Reset();
		}

		/* TODO: try to abstract the bfs into GraphableMixin */

			
		depth = 0;
		max_depth = this.move_range;
		squares_in_curr_depth = 1;
		squares_in_next_depth = 0;

		this.square.TmpVertex.Reset();
		this.square.TmpVertex.score = 1; // discovered
		q = new Queue<SquareGridSquare>();
		q.Enqueue(this.square);

		while (q.Count > 0) {
			var s = q.Dequeue();

			foreach (var adj in s.Vertex.Adjacent) {
				/* see ResetForSearch for what the undiscovered score is */
				if ((adj.TmpVertex.score != 1) &&
					this.CanMoveAdjacent(s, adj)) {

					s.TmpVertex.AddAdjacent(adj.TmpVertex);
					adj.TmpVertex.score = 1; // discovered

					q.Enqueue (adj);
					in_range.Add (adj);
					squares_in_next_depth += 1;
				}
			}

			squares_in_curr_depth -= 1;
			if (squares_in_curr_depth == 0) {
				squares_in_curr_depth = squares_in_next_depth;
				squares_in_next_depth = 0;
				depth += 1;
				if (depth >= max_depth) {
					break;
				}
			}
		}

		return new SquareGridArea(
			in_range, this.square);
	}

	public abstract int GetSpeed();
}
