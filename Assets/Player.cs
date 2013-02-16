using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	float r = 1;
	float g = 1;
	float b = 1;
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
				r = r < threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.X)){
				g = g < threshold ? 1 : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.C)){
				b = b < threshold ? 1 : minColor;
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
		
		Color color = new Color(r, g, b, 1.0f);
		foreach (Transform child in transform){
			foreach (Transform child2 in child.transform){
				child2.renderer.material.color = mask(child2.renderer.material.color) * color;
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
		
		
	}
	
	private Color[] colors = new Color[]{Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white, Color.black};

	public Color getColor(){
		return colors[gameObject.layer-10];	
	}
	
	public void collisionMask(){
		Physics.IgnoreLayerCollision(8, 10, r < threshold);	
		Physics.IgnoreLayerCollision(8, 11, g < threshold);	
		Physics.IgnoreLayerCollision(8, 12, b < threshold);
		Physics.IgnoreLayerCollision(8, 13, g < threshold || b < threshold);	
		Physics.IgnoreLayerCollision(8, 14, r < threshold || b < threshold);	
		Physics.IgnoreLayerCollision(8, 15, r < threshold || g < threshold);
		
	}
	
	Color mask(Color c){
		c.r = c.r > 0 ? 1 : 0;
		c.g = c.g > 0 ? 1 : 0;
		c.b = c.b > 0 ? 1 : 0;
		return c;
	}

	
	
	void OnCollisionEnter(Collision c){
		foreach(ContactPoint x in c.contacts)
			if (x.normal.y > Mathf.Abs(x.normal.x))
				onGround = true;
	}
	
}
