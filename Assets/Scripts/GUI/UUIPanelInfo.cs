using UnityEngine;
using System.Collections;

[System.Serializable]
public class UUIPanelInfo {
	public GameObject panel;
	public UnityEngine.UI.Text text;
	
	public void Activate(string info_text) {
		this.text.text = info_text;
		this.panel.SetActive(true);
	}

	public void Deactivate() {
		this.panel.SetActive(false);
	}
}
