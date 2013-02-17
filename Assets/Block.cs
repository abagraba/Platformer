using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public float drainRate = 0.0f;
	private float intensity = 1.0f;
	private const float maxIntensity = 1.0f;
	private const float minIntensity = 0.5f;
	private Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black};
	

	public Color getColor(){
		return colors[gameObject.layer-10] * intensity;	
	}
	
	void Update () {
		renderer.material.color = getColor();
	}
	
}
