using UnityEngine;
using System.Collections;

public class UUIManager : MonoBehaviour {
	/* For inspector */
	public GameObject unit_action_panel;
	public GameObject active_unit_panel;
	public GameObject selected_unit_panel;
	public GameObject selected_square_panel;

	void Start() {
		if (this.unit_action_panel) {
			this.unit_action_panel.SetActive(false);
		}
		if (this.active_unit_panel != null) {
			this.active_unit_panel.SetActive(false);
		}
		if (this.selected_unit_panel != null) {
			this.selected_unit_panel.SetActive(false);
		}
		if (this.selected_square_panel != null) {
			this.selected_square_panel.SetActive(false);
		}
	}
}
