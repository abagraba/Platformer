using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	void Start () {

		renderer.material.color = getColor();
	}
	
	private Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black};

	public Color getColor(){
		Debug.Log (gameObject.layer-10);
		return colors[gameObject.layer-10];	
	}
	
	void Update () {
	}
	
}
