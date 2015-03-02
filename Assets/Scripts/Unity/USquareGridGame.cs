using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USquareGridGame : SquareGridGame {
	public USquareGrid UGrid { get { return (USquareGrid)this.grid; } }
	public PrefabCache Prefabs { get { return this.cache; } }
	
	public static List<UUnitInit> START_PIECES = new List<UUnitInit>(){
		new UUnitInit(new UnitID("p1_cube"), typeof(UUnitCube), 0, 0),
		new UUnitInit(new UnitID("p1_sphere"), typeof(UUnitSphere), 2, 4),
	};

	private GameObject unit_panel;
	private PrefabCache cache;
	private USquareGridSelector selector;

	public USquareGridGame(int num_rows, int num_cols, float square_len) {
		this.unit_panel = (GameObject)GameObject.Find ("UnitPanel");
		this.unit_panel.SetActive (false); // TODO: argggggg need a manager
		this.selector = new USquareGridSelector();

		this.grid = new USquareGrid(num_rows, num_cols, square_len);
		this.cache = new PrefabCache();
		this.SetupPieces(USquareGridGame.START_PIECES);

		this.unit_scheduler.Begin();
	}

	public static System.Type[] UNIT_CONSTRUCTOR_ARGS = 
		new System.Type[2] { typeof(UnitID), typeof(PrefabCache) };
	public void SetupPieces(IList<UUnitInit> pieces)
	{
		foreach (UUnitInit i in pieces) {
			USquareGridUnit piece;
			System.Reflection.ConstructorInfo info;
			
			
			info = i.unit_class.GetConstructor(USquareGridGame.UNIT_CONSTRUCTOR_ARGS);
			piece = (USquareGridUnit)info.Invoke (new object[2] { i.id, this.cache });
			
			this.MoveUnit(piece, i.row, i.col);
			this.unit_scheduler.AddUnit(piece);
		}
	}

	private void CurrPieceSelected()
	{
		this.unit_panel.SetActive (true);
	}

	public void LeftSelectedSquare(USquareGridSquare us) {
		USquareGridUnit unit;

		if (us == null) {
			return;
		}

		this.selector.SelectUnit(us);

		unit = (USquareGridUnit)us.GetPiece();
		if ((unit != null) && 
		    ((USquareGridUnit)this.unit_scheduler.CurrUnit == unit)) {
			Debug.Log (unit);
			this.CurrPieceSelected();
		}
	}

	public void RightSelectedSquare(USquareGridSquare us) {
	}
}
