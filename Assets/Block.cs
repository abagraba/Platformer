using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public bool R = false;
	public bool G = false;
	public bool B = false;
	
	public bool lightGate = false;
	public bool darkGate = false;
	public bool brown = false;
	
	GameObject PC;
	
	public int[] parameters;
	public int action = 0;
	public float drainRate = 0.0f;
	protected float intensity = 1.0f;
	protected const float maxIntensity = 1.0f;
	protected const float minIntensity = 0.5f;
	protected Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.gray, Color.black, new Color(0.43f, 0.27f, 0)};
	
	void Start () {
		
		if (!PC){
			PC = GameObject.FindGameObjectWithTag("Player");
		} else {
			Debug.LogError("Player not found");
		}
	
		if(R || G || B){
			if (R){
				if(B) {				
					this.renderer.material.color = Color.magenta;
					this.gameObject.layer = 14;
				} else if (G){
					this.renderer.material.color = Color.yellow;
					this.gameObject.layer = 15;
				} else {
					this.renderer.material.color = Color.red;
					this.gameObject.layer = 10;
				}
			
			} else if (G){
				if (B){
					this.renderer.material.color = Color.cyan;
					this.gameObject.layer = 13;
				
				} else {
					this.renderer.material.color = Color.green;
					this.gameObject.layer = 11;
				}	
			} else {
				this.renderer.material.color = Color.blue;
				this.gameObject.layer = 12;
			}
			
			drainRate = 1.0f;
					
		} else if (darkGate){
			this.renderer.material.color = Color.black;
			this.gameObject.layer = 18;
		} else if (lightGate){
			this.renderer.material.color = Color.gray ;
			this.gameObject.layer = 17;
		} else if (brown) {
			this.renderer.material.color = new Color(0.43f, 0.27f, 0);
			this.gameObject.layer = 19;
		} else {
			this.renderer.material.color = Color.white;
			this.gameObject.layer = 16;
			drainRate = 0.0f;
		}			
	}

	public virtual Color getColor(){
		return colors[gameObject.layer-10] * intensity;	
	}
	
	void Update () {
		renderer.material.color = getColor();
		
		Player player = PC.GetComponent<Player>();
		
		Color c = player.c;
		
		float red = c.r;
		float green = c.g;
		float blue = c.b;
		
		Color transparency = this.renderer.material.color;
		
		
			if (R && G){
				transparency.a = Mathf.Min(red, green);
			} else if (R && B){
				transparency.a = Mathf.Min(red, blue);
			}else if (G && B){
				transparency.a = Mathf.Min(green, blue);
			} else if(R){
				transparency.a = red;
			} else if(G){
				transparency.a = green;	
			} else if(B){
				transparency.a = blue;
			}
		
		
		this.renderer.material.color = transparency;
	}
	
}
