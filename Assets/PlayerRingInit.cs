using UnityEngine;
using System.Collections;

public class PlayerRingInit : MonoBehaviour {
	
	public Color c;
	
	void Start () {
		renderer.material.color = c;
	}
}
