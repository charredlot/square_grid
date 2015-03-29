using UnityEngine;
using System.Collections;

public class UUIManager : MonoBehaviour {
	/* for inspector */
	public GameObject unit_action_panel;
	public UUIButton unit_action_move;
	public UUIButton unit_action_attack;
	public UUIButton unit_action_skills;
	public UUIButton unit_action_end_turn;

	/* for inspector */
	public UUIPanelInfo init_active_unit_panel;
	public UUIPanelInfo init_selected_unit_panel;
	public UUIPanelInfo init_selected_square_panel;

	private UUIPanelInfo active_unit_panel;
	private UUIPanelInfo selected_unit_panel;
	private UUIPanelInfo selected_square_panel;

	void Start() {
		this.active_unit_panel = this.init_active_unit_panel;
		this.selected_unit_panel = this.init_selected_unit_panel;
		this.selected_square_panel = this.init_selected_square_panel;

		/*
		 * we want the fields to be private, but need to hook it together in unity inspector 
		 * nulling these out is overkill, but yah
		 */
		this.init_active_unit_panel = null;
		this.init_selected_unit_panel = null;
		this.init_selected_square_panel = null;


		if (this.unit_action_panel) {
			this.unit_action_panel.SetActive(false);
		}
		if (this.active_unit_panel != null) {
			this.active_unit_panel.Deactivate ();
		}
		if (this.selected_unit_panel != null) {
			this.selected_unit_panel.Deactivate();
		}
		if (this.selected_square_panel != null) {
			this.selected_square_panel.Deactivate();
		}
	}

	public void SelectedActiveUnit(USquareGridUnit unit) {
		if (unit == null) {
			this.active_unit_panel.Deactivate();
		} else {
			this.active_unit_panel.Activate (
			"Active Unit:\n" + unit.id.ToString());
		}
	}

	public void SelectedUnit(USquareGridUnit unit) {
		if (unit == null) {
			this.selected_unit_panel.Deactivate();
		} else {
			this.selected_unit_panel.Activate("Selected:\n" + unit.id.ToString ());
		}
	}

	public void SelectedSquare(USquareGridSquare us) {
		if (us != null) {
			this.selected_square_panel.Activate(
				"Row: " + us.Row + 
				"\nCol: " + us.Col + 
				"\nHeight: " + us.Height);
		}
	}
}
