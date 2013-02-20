using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public int[] parameters;
	public int action = 0;
	public float drainRate = 0.0f;
	protected float intensity = 1.0f;
	protected const float maxIntensity = 1.0f;
	protected const float minIntensity = 0.5f;
	protected Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.gray, Color.black, new Color(0.43f, 0.27f, 0)};
	

	public virtual Color getColor(){
		return colors[gameObject.layer-10] * intensity;	
	}
	
	void Update () {
		renderer.material.color = getColor();
	}
	
}
