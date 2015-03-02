using System.Collections;

public class SquareGridSelector {
	protected SquareGridGame game;
	protected SquareGridUnit curr_piece;
	protected SquareGridSquare curr_square;

	public SquareGridSelector(SquareGridGame game) {
		this.game = game;
		this.curr_piece = null;
		this.curr_square = null;
	}


}
