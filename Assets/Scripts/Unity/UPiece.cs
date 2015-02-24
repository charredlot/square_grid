using UnityEngine;
using System.Collections;

public class UPiece : SquareGridPiece {
	private UPieceManager mgr;
	private GameObject g;

	public UPiece(UPieceManager mgr, PieceID id) : base(id) {
		Object prefab;

		this.mgr = mgr;
		prefab = mgr.GetPrefab(id);
		this.g = (GameObject)GameObject.Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
	}

	public override void MoveTo(SquareGridSquare s) {
		this.g.transform.position = s.WorldCenterCoords;
	}
}
