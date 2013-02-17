using UnityEngine;
using System.Collections;

public class ColorInit: MonoBehaviour {
	
	public Color c;
	
	void Start () {
		renderer.material.color = c;
	}
	
}
