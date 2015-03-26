using System.Collections.Generic;

public class SquareGrid {
	private SquareGridSquare[,] squares;
	public int Rows { get { return this.num_rows; } }
	public int Cols { get { return this.num_cols; }  }
	private int num_rows;
	private int num_cols;

	protected SquareGrid() { }

	public SquareGrid(int num_rows, int num_cols)
	{
		this.InitSquares (num_rows, num_cols);
	}

	protected void InitSquares(int num_rows, int num_cols) {
		int row;
		int col;

		this.num_rows = num_rows;
		this.num_cols = num_cols;		
		this.squares = new SquareGridSquare[num_rows, num_cols];

		for (row=0; row<this.num_rows; row++) {
			for (col=0; col<this.num_cols; col++) {
				/* grid is in x-z plane, we're in quadrant 1 so always add */
				this.squares[row, col] = this.AllocSquare(row, col);
			}
		}

		for (row=0; row<this.num_rows; row++) {
			for (col=0; col<this.num_cols; col++) {
				SquareGridSquare curr;
				SquareGridSquare adj;

				curr = this.squares[row, col];

				/* ugh just ugly for now */
				adj = this.GetSquare(row - 1, col);
				if (adj != null) {
					curr.Vertex.AddAdjacent(adj.Vertex);
				}

				adj = this.GetSquare(row + 1, col);
				if (adj != null) {
					curr.Vertex.AddAdjacent(adj.Vertex);
				}

				adj = this.GetSquare(row, col - 1);
				if (adj != null) {
					curr.Vertex.AddAdjacent(adj.Vertex);
				}

				adj = this.GetSquare(row, col + 1);
				if (adj != null) {
					curr.Vertex.AddAdjacent(adj.Vertex);
				}
			}
		}
	}

	protected virtual SquareGridSquare AllocSquare(int row, int col) {
		return new SquareGridSquare(row, col);
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

	public bool MoveUnit(SquareGridUnit unit, SquareGridSquare s) {
		if (s == null) {
			return false;
		}

		s.AddUnit(unit);
		unit.Moved (s);
		return true;
	}
	
	public bool MoveUnit(SquareGridUnit piece, int row, int col) {
		SquareGridSquare s = this.GetSquare(row, col);
		
		if (s == null) {
			return false;
		}

		return this.MoveUnit (piece, s);
	}

	public IEnumerable<SquareGridSquare> GetAdjacentRadius(SquareGridSquare square, int radius) {
		int row;
		int col;

		if (radius == 0) {
			yield break;
		}

		/**
		 * starting at x,y with radius 3
		 * x - 3, y + [0, 0]
		 * x - 2, y + [-1, 1] 
		 * x - 1, y + [-2, 2] 
		 * x, y + [-3, 3]
		 * x + 1, [-2, 2]
		 */
		for (row = square.Row - radius; row <= square.Row + radius; row++) {
			int dist = row - square.Row;
			if (dist < 0) {
				dist = radius + dist;
			} else {
				dist = radius - dist;
			}
			for (col = square.Col - dist; col <= square.Col + dist; col++) {
				SquareGridSquare adj;

				adj = this.GetSquare(row, col);
				if ((adj != null) && (adj != square)) {
					yield return adj;
				}
			}
		}

		yield break;
	}

	public delegate void DebugPrint(object msg);
	public void Dump(DebugPrint debug_print) {
	}
}
