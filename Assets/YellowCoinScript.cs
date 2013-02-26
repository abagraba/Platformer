using UnityEngine;
using System.Collections;

public class YellowCoinScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	renderer.material.SetColor("_Color", Color.yellow);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){
	
		if(other.tag== "Player"){
			
		}
		
	}
}
