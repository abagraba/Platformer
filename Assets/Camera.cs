using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	
	public Player PC;
	
	/*
	 * Camera locks on to the Player's position and rotates such that gravity points down.
	 */
	void Update () {
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, PC.gravityAngle * 180.0f / Mathf.PI));
		transform.position = new Vector3(PC.transform.position.x, PC.transform.position.y, -50);
	}
}
