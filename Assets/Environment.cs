using UnityEngine;
using System.Collections;

/*
 * Environment Blocks are background objects that oscillate in color.
 * They exist in the static Environment Layer which collides only with Blocks.
 */
public class Environment : Block {
	
	public float fluxRate = 0.05f;
	public Color color;
	private bool flux = false;
	private float intensity = 1.0f;
	public float minIntensity = 0.5f;
	public float maxIntensity = 1.0f;
	
	/**
	 * Adjusts the color such that it fades in and out.
	 */
	void FixedUpdate () {
		if (intensity < minIntensity)
			flux = true;
		if (intensity > maxIntensity)
			flux = false;
		intensity += fluxRate * (flux ? 1 : -1);
	}
	
	/*
	 * Returns the oscillating color of the Environment Block.
	 */
	public override Color getColor(){
		return color * intensity;	
	}
	
}
