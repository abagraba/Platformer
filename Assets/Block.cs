using UnityEngine;
using System.Collections;

/*
 * A Block collides with the Player if the Player has more than the minimum Threshold of any color the Block posesses.
 * It exists in the Block Layers (10 - 17)
 * A block can also drain colors from the Player.
 */
public class Block : Gate {
		
	public float drainRate = 0.0f;
	public int action = 0;
	public int[] parameters;
	/*
	 * Initializes a reference to the Player and sets the Block's initial color.
	 */
	void Start() {
		PC = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.white * 0.3f};
	//	renderer.material.color = colors[gameObject.layer - 10];				
	}
	
	/*
	 * Returns the color of this object. 
	 * 
	 * The color is determined by retreiving the color from the lookup table based on the level of the Block.
	 * This color's alpha value is then set to the lowest checked RGB value of the Player.
	 * Only RGB values in the base color are checked.
	 * 
	 * This means that a red Block will only check the Player's red value and use that to generate the alpha value.
	 * 
	 * Black blocks have a color value of dark grey to prevent conflicts with background. Black blocks also do not vary with color.
 	 */ 
 	 public override Color getColor(){
		Color c = colors[gameObject.layer - 10];			
		if (gameObject.layer == 16)
			return c;
		float a = 1.0f;
		if(c.r > 0)
			a = Mathf.Min(a, PC.c.r);
		if(c.g > 0)
			a = Mathf.Min(a, PC.c.g);
		if(c.b > 0)
			a = Mathf.Min(a, PC.c.b);
		c.a = a;
		return c;	
	}
	
	/*
	 * Accessible method that should be run every FixedUpdate that ensures that the object is awake.
	 * Also applies the force of gravity as defined for the Player.
	 */
	public void wake(){
		if (rigidbody != null){
			rigidbody.WakeUp();
			rigidbody.AddForce(PC.up()*PC.gravAccel*rigidbody.mass);	
		}
	}
	
	
}
