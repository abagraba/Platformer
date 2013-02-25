using UnityEngine;
using System.Collections;



public class platformScript : MonoBehaviour {
	
	public bool platform = true;
	public bool R = false;
	public bool G = false;
	public bool B = false;
	
	GameObject PC;

	
	// Use this for initialization
	void Start () {
		
		if (!PC){
			PC = GameObject.FindGameObjectWithTag("Player");
		} else {
			Debug.LogError("Player not found");
		}
	
		if(!platform){
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
						
		} else {
			
			this.renderer.material.color = Color.white;
			
		}
			
			
	}
	
	// Update is called once per frame
	void Update () {
		
		Player player = PC.GetComponent<Player>();
		
		Color c = player.c;
		
		float red = c.r;
		float green = c.g;
		float blue = c.b;
		
		Color transparency = this.renderer.material.color;
		
		if (!platform){
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
		}
		
		this.renderer.material.color = transparency;
	}
}
