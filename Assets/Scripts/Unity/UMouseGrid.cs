using UnityEngine;
using System.Collections;

public class UMouseGrid : MonoBehaviour, InitCheckable {
	public string GROUND_OBJ_NAME = "ground";
	public string UNIT_SQUARE_NAME = "UNIT_CUBE";
	public string SELECTOR_PREFAB_PATH = "Assets/Prefabs/SquareProjector.prefab";

	public int NUM_ROWS = 8;
	public int NUM_COLS = 10;
	public float RAY_DISTANCE_MAX = 20.0f;
	public SquareGrid grid;

	public delegate void SquareClicked(SquareGridSquare square);
	
	private UMouseGrid.SquareClicked clicked_delegate;	
	private float square_len;
	private Collider ground;
	private bool ready = false;

	// Use this for initialization
	void Start () {
		GameObject ground_obj = (GameObject)GameObject.Find (this.GROUND_OBJ_NAME);
		this.ground = ground_obj.collider;

		GameObject unit_square = (GameObject)GameObject.Find (this.UNIT_SQUARE_NAME);
		this.square_len = unit_square.renderer.bounds.size.x;

		/* x-z plane */
		this.grid = new SquareGrid(this.NUM_ROWS,
		                           this.NUM_COLS, this.square_len);

		this.SetClickedDelegate();
		this.ready = true;
	}

	public bool IsReady() { return this.ready; }

	protected void SetClickedDelegate() {
		/* erg this is painful, just manual for now, let people inherit */
		SquareGridSelector selector =
			new SquareGridSelector(this.SELECTOR_PREFAB_PATH);

		this.clicked_delegate = selector.SelectSquare;
	}

	SquareGridSquare FindSquare(Vector3 mouse_pos) {
		var ray = Camera.main.ScreenPointToRay(mouse_pos);
		RaycastHit hit_info;
		
		if (this.ground.Raycast(ray, out hit_info, this.RAY_DISTANCE_MAX)) {
			Vector3 p;

			//Debug.Log (p);
			//Debug.Log (this.grid.GetSquare(p));
			p = ray.GetPoint (hit_info.distance);
			return this.grid.GetSquare(p);
		} else {
			return null;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(Hacks.MouseButton.LEFT)) {
			SquareGridSquare s = this.FindSquare(Input.mousePosition);

			if (s != null) {
				this.clicked_delegate(s);
			}
		}
	}
}
