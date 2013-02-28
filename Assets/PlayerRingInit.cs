using UnityEngine;
using System.Collections;

public class PlayerRingInit : MonoBehaviour {
	
	public Color c;
	
	/*
	 * Initializes the Player's sub rings to proper colors necessary for masking. 
	 */
	void Start () {
		renderer.material.color = c;
	}
}
