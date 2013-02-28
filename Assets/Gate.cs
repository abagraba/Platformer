using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	Player PC;
	
	protected Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black};
	
	void Start () {
		
		PC = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		renderer.material.color = colors[gameObject.layer - 18];			
					
	}

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
			c.a = 1.2f - ax;
		}
		return c;
	}
	
	void Update () {
		renderer.material.color = getColor();
		
	}
	
}
