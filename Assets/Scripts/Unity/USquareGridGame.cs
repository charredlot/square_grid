using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class USquareGridGame : SquareGridGame {
	public USquareGrid UGrid { get { return (USquareGrid)this.grid; } }
	public PrefabCache Prefabs { get { return this.prefab_cache; } }
	
	public static List<UUnitInit> START_PIECES = new List<UUnitInit>(){
		new UUnitInit(new UnitID("p1_cube"), typeof(UUnitCube), 0, 0),
		new UUnitInit(new UnitID("p1_sphere"), typeof(UUnitSphere), 2, 4),
	};
	
	private PrefabCache prefab_cache;

	public USquareGridGame(PrefabCache cache, int num_rows, int num_cols, float square_len) {
		this.grid = new USquareGrid(num_rows, num_cols, square_len);
		this.prefab_cache = cache;
		this.SetupPieces(USquareGridGame.START_PIECES);

		this.unit_scheduler.Begin();
	}

	public static System.Type[] UNIT_CONSTRUCTOR_ARGS = 
		new System.Type[2] { typeof(UnitID), typeof(PrefabCache) };
	public void SetupPieces(IList<UUnitInit> pieces)
	{
		foreach (UUnitInit i in pieces) {
			USquareGridUnit unit;
			System.Reflection.ConstructorInfo info;
			
			
			info = i.unit_class.GetConstructor(USquareGridGame.UNIT_CONSTRUCTOR_ARGS);
			unit = (USquareGridUnit)info.Invoke (new object[2] { i.id, this.prefab_cache });
			
			this.MoveUnit(unit, i.row, i.col);
			this.unit_scheduler.AddUnit(unit);
		}
	}

	public USquareGridUnit ActiveUnit {
		get { return (USquareGridUnit)this.unit_scheduler.ActiveUnit; }
	}

	public IEnumerable<USquareGridSquare> GetMoveableArea() {
		var unit = this.ActiveUnit;
		var us = (USquareGridSquare)unit.Square;

		/* TODO */

		return from s in this.grid.GetAdjacentRadius(us, 3) select (USquareGridSquare)s;
	}
}
