using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	Player PC;
	
	public int[] parameters;
	public int action = 0;
	public float drainRate = 0.0f;
	protected Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.white * 0.2f};
	
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
	
	public void wake(){
		if (rigidbody != null){
			rigidbody.AddForce(PC.up()*PC.gravAccel*rigidbody.mass);	
		}
			
	}
	
	void Update () {
		renderer.material.color = getColor();
		
	}
	
}
