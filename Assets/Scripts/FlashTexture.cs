using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashTexture : MonoBehaviour {
	public Texture2D[] textures;
	MeshRenderer r;
	
	// Use this for initialization
	void Start () {	
		this.r = (MeshRenderer)this.GetComponent ("MeshRenderer");
		StartCoroutine("RotateTexture");
	}

	IEnumerator RotateTexture()
	{
		/* if textures are not setup it'll hard crash */
		while (this.textures.Length > 0) {
			int t;

			for (t=0; t<this.textures.Length; t++) {
				this.r.material.mainTexture = this.textures[t];
				yield return new WaitForSeconds(1);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
