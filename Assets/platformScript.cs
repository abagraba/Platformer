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
			PC = GameObject.FindGameObjectWithTag("player");
		} else {
			Debug.LogError("Player not found");
		}
	
		if(!platform){
			if (R){
				if(B) {				
					this.renderer.material.color = Color.magenta;
					
				} else if (G){
					this.renderer.material.color = Color.yellow;
					
				} else {
					this.renderer.material.color = Color.red;
				}
			
			} else if (G){
				if (B){
					this.renderer.material.color = Color.cyan;
				
				} else {
					this.renderer.material.color = Color.green;
					
				}	
			} else {
				this.renderer.material.color = Color.blue;
			
			}
						
		} else {
			
			this.renderer.material.color = Color.white;
			
		}
			
			
	}
	
	// Update is called once per frame
	void Update () {
		if(!platform){
			if (R && G){
				this.renderer.material.color.a = Mathf.Min(PC.renderer.material.color.r,PC.renderer.material.color.g);
			} else if (R && B){
				this.renderer.material.color.a = Mathf.Min(PC.renderer.material.color.r,PC.renderer.material.color.b);
			}else if (G && B){
				this.renderer.material.color.a = Mathf.Min(PC.renderer.material.color.g,PC.renderer.material.color.b);
			} else if(R){
				this.renderer.material.color.a = PC.renderer.material.color.r;
			} else if(G){
				this.renderer.material.color.a = PC.renderer.material.color.g;	
			} else if(B){
				this.renderer.material.color.a = PC.renderer.material.color.b;
			}
		}
	}
}
