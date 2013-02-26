using UnityEngine;
using System.Collections;

public class Environment : Block {
	
	bool flux = false;
	public float fluxRate = 0.05f;
	public Color color;
	private float intensity = 1.0f;
	private float minIntensity = 0.5f;
	private float maxIntensity = 1.0f;
	
	void Start () {
	}
	
	void FixedUpdate () {
		if (intensity < minIntensity)
			flux = true;
		if (intensity > maxIntensity)
			flux = false;
		intensity += fluxRate * (flux ? 1 : -1);
	}
	
	public override Color getColor(){
		return color * intensity;	
	}
	
}
