using UnityEngine;
using System.Collections;

/*
 * A gravity well will rotate the Player's gravity such that down is in the -y direction of the GravWell.
 * A GravWell also acts as a checkpoint, respawning a player with a specified color if the player hits a Death trigger.
 * A GravWell also maintains a list of rigidbodies to watch.
 *   When the Gravwell is considered the active checkpoint by the Player, the rigidbodies are activated.
 *   When the it is nop longer considered active, updates to the rigidbodies stop.
 */
public class GravWell : MonoBehaviour {
	
	//How fast gravity will rotate to match the orientation of the GravWell.
	public float rotateSpeed = Mathf.PI / 180.0f;
	//Respawn color. Player will be given this color upon respawn.
	public Color c = Color.white;
	//List of Rigidbodies to watch.
	public GameObject[] phys;
	
}
