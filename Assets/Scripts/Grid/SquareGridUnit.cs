
public abstract class SquareGridUnit : UnitSchedulable {
	public readonly UnitID id;
	public SquareGridSquare Square { get { return this.square; } }
	
	protected SquareGridSquare square;
	
	public SquareGridUnit(UnitID id, SquareGridSquare square = null) {
		this.id = id;
		this.square = square;
	}

	public virtual bool CanMoveTo(SquareGridSquare target) {
		return true;
	}

	public virtual void Moved(SquareGridSquare target) {
		this.square = target;
	}

	public virtual void BeginTurn()
	{
	}

	public abstract int GetSpeed();
}
