using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	
	public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Player PC = (Player)target.GetComponent(typeof(Player));
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, PC.angleDown * 180.0f / Mathf.PI));
		transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -50);
	}
}
