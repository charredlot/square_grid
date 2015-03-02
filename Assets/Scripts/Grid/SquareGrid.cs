
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
	
	public bool MoveUnit(SquareGridUnit piece, int row, int col) {
		SquareGridSquare s = this.GetSquare(row, col);
		
		if (s == null) {
			return false;
		}

		if (piece.CanMoveTo(s)) {
			s.AddPiece(piece);
			piece.Moved(s);
			return true;
		} else {
			return false;
		}
	}

	public delegate void DebugPrint(object msg);
	public void Dump(DebugPrint debug_print) {
	}
}
