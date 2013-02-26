using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public Color c = Color.white;
	// Use this for initialization
	
	public float accelerationFactor;
	
	public float horizontalAcceleration;		
	public float gravAccel;

	public float angleDown = 0.0f;
	private float finalAngleDown = 0.0f;
	private float rotateSpeed = Mathf.PI/180.0f;
	private float jumpVelocity;
	public float normalJumpHeight;
	
	public float minColor;
	public float threshold;
	public float max;
	public int swapDelay;
	
	public GameObject core;
	
	private float health = 1.0f;
	private int frames;
	private bool onGround = false;
	private GravWell spawn;
	
	void Start () {
		
	}
	
	private void testing(){
		if (frames > 0)
			frames--;
		else{
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
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		testing();
	
		collisionMask();
		
		core.transform.localScale = new Vector3(0.7f * health, 1.0f, 0.7f * health);
		
		
		// Set Colors of Components
		// Uses the existing color to mask the color to the right components.
		foreach (Transform child in transform)
			foreach (Transform child2 in child.transform)
				child2.renderer.material.color = mask(child2.renderer.material.color) * c;
			
		float dAngle = finalAngleDown - angleDown;
		dAngle %= 2*Mathf.PI;
		float rotate = rotateSpeed * (dAngle>0?1:-1) * (Mathf.Abs(dAngle)>Mathf.PI?-1:1);
		angleDown += center(rotate, dAngle);
		
		Vector3 force = new Vector3(0, 0, 0);
		if (Input.GetKey(KeyCode.LeftArrow))
			force -= rigidbody.mass*horizontalAcceleration*accelerationFactor * right();
		if (Input.GetKey(KeyCode.RightArrow))
			force += rigidbody.mass*horizontalAcceleration*accelerationFactor * right();
		
		float fGrav = rigidbody.mass*gravAccel*accelerationFactor;
		force += up()*fGrav;
		
		force *= Time.fixedDeltaTime;
		rigidbody.AddForce(force);
		if (onGround && Input.GetKey(KeyCode.UpArrow)){
			float horizontal = Vector3.Dot(rigidbody.velocity, right());
			rigidbody.velocity =  horizontal * right() + jumpVelocity * up();
		}
		
		jumpVelocity = normalJumpHeight;
		onGround = false;
	}
	
	private Vector3 up(){
		return new Vector3(-Mathf.Sin(angleDown), Mathf.Cos(angleDown));	
	}
	
	private Vector3 right(){
		return new Vector3(Mathf.Cos(angleDown), Mathf.Sin(angleDown));	
	}
	
	private float center(float a, float b){
		if (Mathf.Abs(a)>Mathf.Abs(b))
			return b;
		return a;
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
			float dtheta = Vector3.Dot(x.normal, up());
			dtheta /= x.normal.magnitude;
			if (Mathf.Abs(dtheta) >= Mathf.Cos(Mathf.PI/4.0f))
				onGround = true;			
			if (b.gameObject.layer == 19)
				jumpVelocity = 0;
			if (b.action == 1){
				jumpVelocity = b.parameters[0];
				float horizontal = Vector3.Dot(rigidbody.velocity, right());
				rigidbody.velocity = horizontal * right() + jumpVelocity * up();
			}
			drain(b);	
		}
	}
	
	void OnCollisionStay(Collision cx){
		foreach(ContactPoint x in cx.contacts){
			float dtheta = Vector3.Dot(x.normal, up());
			dtheta /= x.normal.magnitude;
			if (Mathf.Abs(dtheta) >= Mathf.Cos(Mathf.PI/4.0f))
				onGround = true;			
			Block b = getBlock(x.otherCollider);
			drain(b);
		}
	}
	
	void OnTriggerEnter(Collider cx){
		if (!drain(getBlock(cx))){
			if (cx.tag.Equals("Death"))
				respawn();
			if (cx.tag.Equals("Respawn")){
				GravWell gravWell = (GravWell)cx.GetComponent(typeof(GravWell));
				spawn = gravWell;
				rotateSpeed = gravWell.rotateSpeed;
				finalAngleDown = cx.transform.eulerAngles.z*Mathf.PI/180.0f;
			}
		}
	}
	
	void OnTriggerStay(Collider cx){
		if (!drain(getBlock(cx))){
			GameObject gravWell = cx.gameObject;
		}
	}

	private Block getBlock(Collider x){
		return (Block) x.GetComponent(typeof(Block));
	}
	
	bool drain(Block b){
		if (b != null){
			decrementColor(b.drainRate * b.getColor());
			return true;
		}
		return false;
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
	
	Color getColorValues(){
	return c;	
	}
	
	public void respawn(){
		c = spawn.c;
		transform.position = spawn.transform.position;
	}
	
}
