using UnityEngine;
using System.Collections;

/*
 * A Gate collides with the Player if the Player has less than the maximum Threshold of any color the Block posesses.
 * It exists in the Gate Layers (18 - 25)
 */
public class Gate : MonoBehaviour {

	protected Player PC;
	protected Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.white * 0.3f};
	
	/*
	 * Initializes a reference to the Player and sets the Block's initial color.
	 */
	void Start () {
		PC = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		renderer.material.color = colors[gameObject.layer - 18];				
	}
	
	/*
	 * Returns the color of this object. 
	 * 
	 * The color is determined by retreiving the color from the lookup table based on the level of the Block.
	 * This color's alpha value is then set to the (1 - lowest checked RGB value) of the Player.
	 * Only RGB values in the base color are checked.
	 * 
	 * This means that a red Block will only check the Player's red value and use that to generate the alpha value.
	 * 
	 * Black blocks have a color value of dark grey to prevent conflicts with background. Black blocks also do not vary with color.
 	 */
	public virtual Color getColor(){
		Color c = colors[gameObject.layer - 18];			
		if (gameObject.layer == 25){
			float ax = 0.0f;
			if(c.r > 0)
				ax = Mathf.Max(ax, PC.c.r);
			if(c.g > 0)
				ax = Mathf.Max(ax, PC.c.g);
			if(c.b > 0)
				ax = Mathf.Max(ax, PC.c.b);
			c.a = ax;
		}
		else{
			float ax = 1.0f;
			if(c.r > 0)
				ax = Mathf.Min(ax, PC.c.r);
			if(c.g > 0)
				ax = Mathf.Min(ax, PC.c.g);
			if(c.b > 0)
				ax = Mathf.Min(ax, PC.c.b);
			c.a = 1.0f + PC.minColor - ax;
		}
		return c;
	}
	
	/*
	 * Updates the color of the Block. 
	 */
	void Update () {
		renderer.material.color = getColor();
	}
	
}
