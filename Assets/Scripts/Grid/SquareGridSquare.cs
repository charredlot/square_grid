using System.Collections.Generic;

public class SquareGridSquare {
	public readonly int Row;
	public readonly int Col;
	public readonly int Height;

	private List<SquareGridUnit> pieces;
	
	public SquareGridSquare(int row, int col, int height=0) {
		this.Row = row;
		this.Col = col;
		this.Height = height;
		this.pieces = new List<SquareGridUnit>();
	}
	
	public void AddPiece(SquareGridUnit piece)
	{
		this.pieces.Add(piece);
	}

	public SquareGridUnit GetPiece()
	{
		/* handle multiple units later */
		if (this.pieces.Count > 0) {
			return this.pieces[0];
		} else {
			return null;
		}
	}
	
	public IEnumerator<SquareGridUnit> GetPieces()
	{
		return this.pieces.GetEnumerator();
	}
	
	public int NumPieces()
	{
		return this.pieces.Count;
	}
	
	public override string ToString ()
	{
		return string.Format ("[Square row={0} col={1}]",
		                      this.Row, this.Col);
	}
};
