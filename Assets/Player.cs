using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	Color c = Color.white;
	// Use this for initialization
	
	public float accelerationFactor;
	
	public float horizontalAcceleration;		
	public float jumpAccel;
	public float gravAccel;
	
	public float minColor;
	public float threshold;
	public int swapDelay;
	
	public GameObject core;
	
	private float health = 1.0f;
	private int frames;
	private bool onGround = false;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (frames > 0)
			frames--;
		
		if (frames == 0){
			if (Input.GetKey(KeyCode.Z)){
				c.r = c.r < threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.X)){
				c.g = c.g < threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.C)){
				c.b = c.b < threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.A)){
				health = 1.0f;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.S)){
				health = 0.3f;
				frames = swapDelay;
			}

		
		}

		
		collisionMask();
		
		core.transform.localScale = new Vector3(0.7f * health, 1.0f, 0.7f * health);
		
		foreach (Transform child in transform){
			foreach (Transform child2 in child.transform){
				child2.renderer.material.color = mask(child2.renderer.material.color) * c;
			}
		}
		
		Vector3 force = new Vector3(0, 0, 0);
	
		if (Input.GetKey(KeyCode.LeftArrow))
			force.x -= rigidbody.mass*horizontalAcceleration*accelerationFactor;
		if (Input.GetKey(KeyCode.RightArrow))
			force.x += rigidbody.mass*horizontalAcceleration*accelerationFactor;
		
		if (onGround && Input.GetKey(KeyCode.UpArrow)){
			onGround = false;
			force.y = rigidbody.mass*jumpAccel*accelerationFactor;
		}
		force.y -= rigidbody.mass*gravAccel*accelerationFactor;
		
		force *= Time.fixedDeltaTime;
		rigidbody.AddForce(force);
		
		Debug.Log(c);
		
	}
	
	private Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black};

	public Color getColor(){
		return colors[gameObject.layer-10];	
	}
	
	public void collisionMask(){
		Physics.IgnoreLayerCollision(8, 10, c.r <= threshold);	
		Physics.IgnoreLayerCollision(8, 11, c.g <= threshold);	
		Physics.IgnoreLayerCollision(8, 12, c.b <= threshold);
		Physics.IgnoreLayerCollision(8, 13, c.g <= threshold || c.b <= threshold);	
		Physics.IgnoreLayerCollision(8, 14, c.r <= threshold || c.b <= threshold);	
		Physics.IgnoreLayerCollision(8, 15, c.r <= threshold || c.g <= threshold);
		
	}
	
	Color mask(Color c){
		c.r = c.r > 0 ? 1 : 0;
		c.g = c.g > 0 ? 1 : 0;
		c.b = c.b > 0 ? 1 : 0;
		return c;
	}

	
	
	void OnCollisionEnter(Collision cx){
		foreach(ContactPoint x in cx.contacts){
			if (x.normal.y > Mathf.Abs(x.normal.x))
				onGround = true;
			drain(x.otherCollider);
		}
	}
	
	void OnCollisionStay(Collision cx){
		foreach(ContactPoint x in cx.contacts)
			drain(x.otherCollider);
	}
	
	void drain(Collider cx){
		Block b = (Block) cx.GetComponent(typeof(Block));
		if (b != null)
			decrementColor(b.drainRate * b.getColor());
	}
	
	void decrementColor (Color dec){
		dec *= 0.01f;
		dec.a = 0;		

		Color x = c - dec;
		if (x.r < minColor)
			dec.r = c.r - minColor;
		if (x.g < minColor)
			dec.g = c.g - minColor;
		if (x.b < minColor)
			dec.b = c.b - minColor;
		c -= dec;
	}
}
