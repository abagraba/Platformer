using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	Color c = Color.white;
	// Use this for initialization
	
	public float accelerationFactor;
	
	public float horizontalAcceleration;		
	public float gravAccel;
	
	private float maxVerticalVelocity;
	public float normalJumpHeight;
	
	public float minColor;
	public float threshold;
	public float max;
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
				c.r = c.r <= threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.X)){
				c.g = c.g <= threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.C)){
				c.b = c.b <= threshold ? 1 : minColor;
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
		
		
		// Set Colors of Components
		// Uses the existing color to mask the color to the right components.
		foreach (Transform child in transform)
			foreach (Transform child2 in child.transform)
				child2.renderer.material.color = mask(child2.renderer.material.color) * c;
			
		
		Vector3 force = new Vector3(0, 0, 0);
		bool jump = false;
		if (Input.GetKey(KeyCode.LeftArrow))
			force.x -= rigidbody.mass*horizontalAcceleration*accelerationFactor;
		if (Input.GetKey(KeyCode.RightArrow))
			force.x += rigidbody.mass*horizontalAcceleration*accelerationFactor;
		
		if (onGround && Input.GetKey(KeyCode.UpArrow)){
			onGround = false;
			jump = true;
		}
		force.y -= rigidbody.mass*gravAccel*accelerationFactor;
		
		force *= Time.fixedDeltaTime;
		rigidbody.AddForce(force);
		if (jump){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxVerticalVelocity, rigidbody.velocity.z);
			jump = !jump;
		}
	}
	
	private Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black};

	public Color getColor(){
		return colors[gameObject.layer-10];	
	}
	
	public void collisionMask(){
		bool rThresh = c.r <= threshold;
		bool gThresh = c.g <= threshold;
		bool bThresh = c.b <= threshold;
		bool rMax = c.r >= max;
		bool gMax = c.g >= max;
		bool bMax = c.b >= max;
		Physics.IgnoreLayerCollision(8, 10, rThresh);	
		Physics.IgnoreLayerCollision(8, 11, gThresh);	
		Physics.IgnoreLayerCollision(8, 12, bThresh);
		Physics.IgnoreLayerCollision(8, 13, gThresh || bThresh);	
		Physics.IgnoreLayerCollision(8, 14, rThresh || bThresh);	
		Physics.IgnoreLayerCollision(8, 15, rThresh || gThresh);
		Physics.IgnoreLayerCollision(8, 17, !(rThresh || gThresh || bThresh));
		
	}
	
	Color mask(Color c){
		c.r = c.r > 0 ? 1 : 0;
		c.g = c.g > 0 ? 1 : 0;
		c.b = c.b > 0 ? 1 : 0;
		return c;
	}

	
	
	void OnCollisionEnter(Collision cx){
		foreach(ContactPoint x in cx.contacts){
		Block b = getBlock(x.otherCollider);
			if (x.normal.y > Mathf.Abs(x.normal.x)){
				onGround = true;
				maxVerticalVelocity = normalJumpHeight;
			}
			if (b.gameObject.layer == 19)
				maxVerticalVelocity = 0;
			if (b.action == 1){
				maxVerticalVelocity = b.parameters[0];
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxVerticalVelocity, rigidbody.velocity.z);
			}
			drain(b);	
		}
	}
	
	void OnCollisionStay(Collision cx){
		foreach(ContactPoint x in cx.contacts){
			if (x.normal.y > Mathf.Abs(x.normal.x))
				onGround = true;
			Block b = getBlock(x.otherCollider);
			drain(b);
		}
	}
	
	void OnTriggerEnter(Collider cx){
		drain(getBlock(cx));
	}
	
	void OnTriggerStay(Collider cx){
		drain(getBlock(cx));
	}

	private Block getBlock(Collider x){
		return (Block) x.GetComponent(typeof(Block));
	}
	
	void drain(Block b){
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
