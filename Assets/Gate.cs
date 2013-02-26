using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	Player PC;
	
	public float drainRate = 0.0f;
	protected Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black, new Color(0.43f, 0.27f, 0)};
	
	void Start () {
		
		PC = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		renderer.material.color = colors[gameObject.layer - 10];			
					
	}

	public virtual Color getColor(){
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
	
	void Update () {
		renderer.material.color = getColor();
		
	}
	
}
