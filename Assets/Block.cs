using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public Color c;
	
	void Start () {
		renderer.material.color = c;
	}
	void Update () {
		renderer.material.color = c;
	}
	
}
