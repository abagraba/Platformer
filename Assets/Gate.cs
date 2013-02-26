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
		Color c = colors[gameObject.layer - 10];			
		if (gameObject.layer == 16)
			return c;
		float a = 1.0f;
		if(c.r > 0)
			a = Mathf.Min(a, 1-PC.c.r);
		if(c.g > 0)
			a = Mathf.Min(a, 1-PC.c.g);
		if(c.b > 0)
			a = Mathf.Min(a, 1-PC.c.b);
		c.a = a;
		return c;	
	}
	
	void Update () {
		renderer.material.color = getColor();
		
	}
	
}
