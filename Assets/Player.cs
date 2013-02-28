using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	//Stores the color value of the Player.
	public Color c = Color.white;
	
	//Overall acceleration strength multiplier.
	public float accelerationFactor;
	
	//Strength of movement (Left/Right Keys)
	public float horizontalAcceleration;		
	//Strength of Gravity
	public float gravAccel;
	
	//Direction of Gravity
	public float gravityAngle = 0.0f;
	//Final direction of gravity. Used to interpolate gravitational rotations.
	private float finalGravityAngle = 0.0f;
	//Rotation speed during rotation of gravity.
	private float rotateSpeed = Mathf.PI/180.0f;
	
	private float jumpVelocity;
	public float normalJumpHeight;
	
	//Minimum amount of color the Player can have.
	public float minColor;
	//Maximum amount of color the Player can have.
	public float maxColor;
	
	//Lower Threshold of color. Used for Block collisions to determine when Player stops colliding with it.
	public float minThresh;
	//Upper Threshold of color. Used for Gate collisions to determine when Player stops colliding with it.
	public float maxThresh;
	
	public int swapDelay;
	private int frames;
	
	//Center of the Player. Used for health updates.
	public GameObject core;
	//Player health
	private float health = 1.0f;
	//boolean that determines if the player can jump.
	private bool onGround = false;
	//Determines what angle the collision needs to be at to allow a jump.
	private float jumpReset = Mathf.PI/3;
	
	//Respawn point
	public GravWell spawn;
	public GameObject[] phys;
	
	//Spawns the Player
	void Start () {
		respawn();
	}
	
	private void debug(){
		if (frames > 0)
			frames--;
		else{
			if (Input.GetKey(KeyCode.Z)){
				c.r = c.r <= minThresh ? maxColor : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.X)){
				c.g = c.g <= minThresh ? maxColor : minColor;
				frames = swapDelay;
			}
			if (Input.GetKey(KeyCode.C)){
				c.b = c.b <= minThresh ? maxColor : minColor;
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
        //Wake objects.
		wake();
		ignoreCollisions();
		updatePlayer();
		physics();
			
		debug();
		
		//Reset Variables.
		jumpVelocity = normalJumpHeight;
		onGround = false;
	}
	
	/*
	 * Does Physics
	 */
	private void physics(){
		interpolateGravity();
		Vector3 force = new Vector3(0, 0, 0);
		
		if (Input.GetKey(KeyCode.LeftArrow))
			force -= rigidbody.mass*horizontalAcceleration*accelerationFactor * right();
		if (Input.GetKey(KeyCode.RightArrow))
			force += rigidbody.mass*horizontalAcceleration*accelerationFactor * right();
		float fGrav = rigidbody.mass*gravAccel*accelerationFactor;
		force += up()*fGrav; // fGrav = -9.8
		
		rigidbody.AddForce(force);
		
		if(Input.GetKey(KeyCode.UpArrow))
			jump();
	}
	
	/*
	 * Takes the required change in angle and clamps it down to +-rotateSpeed.
	 * Then adds this value to gravity.
	 */
	private void interpolateGravity(){
		float dAngle = finalGravityAngle - gravityAngle;
		dAngle %= 2*Mathf.PI;
		if (dAngle < -Mathf.PI)
			dAngle += 2*Mathf.PI;
		if (dAngle > Mathf.PI)
			dAngle -= 2*Mathf.PI;
		float rotate = rotateSpeed * (dAngle>0?1:-1);
		gravityAngle += center(rotate, dAngle);
		gravityAngle %= 2 * Mathf.PI;
	}
	
	/*
	 * Makes the Player jump with velocity jumpVelocity.
	 */
	private void jump(){
		if (onGround){
			float horizontal = Vector3.Dot(rigidbody.velocity, right());
			rigidbody.velocity =  horizontal * right() + jumpVelocity * up();
		}
	}
	
	//Returns the direction up in relation to gravity
	public Vector3 up(){
		return new Vector3(-Mathf.Sin(gravityAngle), Mathf.Cos(gravityAngle));	
	}
	//Returns the direction right in relation to gravity
	public Vector3 right(){
		return new Vector3(Mathf.Cos(gravityAngle), Mathf.Sin(gravityAngle));	
	}
	//Returns the value closest to zero.
	private float center(float a, float b){
		if (Mathf.Abs(a)>Mathf.Abs(b))
			return b;
		return a;
	}
		
	/*
	 * Checks the player colors and uses minThresh and maxThresh to determine which Physics layers the Player may collide with.
	 */
	private void ignoreCollisions(){
		bool rThresh = c.r <= minThresh;
		bool gThresh = c.g <= minThresh;
		bool bThresh = c.b <= minThresh;
		bool rMax = c.r >= maxThresh;
		bool gMax = c.g >= maxThresh;
		bool bMax = c.b >= maxThresh;
		Physics.IgnoreLayerCollision(8, 10, rThresh);	
		Physics.IgnoreLayerCollision(8, 11, gThresh);	
		Physics.IgnoreLayerCollision(8, 12, bThresh);
		Physics.IgnoreLayerCollision(8, 13, gThresh || bThresh);	
		Physics.IgnoreLayerCollision(8, 14, rThresh || bThresh);	
		Physics.IgnoreLayerCollision(8, 15, rThresh || gThresh);
		Physics.IgnoreLayerCollision(8, 18, rMax);	
		Physics.IgnoreLayerCollision(8, 19, gMax);	
		Physics.IgnoreLayerCollision(8, 20, bMax);
		Physics.IgnoreLayerCollision(8, 21, gMax && bMax);	
		Physics.IgnoreLayerCollision(8, 22, rMax && bMax);	
		Physics.IgnoreLayerCollision(8, 23, rMax && gMax);
		Physics.IgnoreLayerCollision(8, 24, rMax && gMax && bMax);
		Physics.IgnoreLayerCollision(8, 25, rThresh && gThresh && bThresh);
	}
	
	/*
	 * Determines what components in the color have a value.
	 */
	Color mask(Color c){
		c.r = c.r > 0 ? 1 : 0;
		c.g = c.g > 0 ? 1 : 0;
		c.b = c.b > 0 ? 1 : 0;
		return c;
	}
	
	/*
	 * Updates the Player state.
	 */
	private void updatePlayer(){
		// Set Colors of Components
		// Uses the existing color to mask the color to the right components.
		foreach (Transform child in transform)
			foreach (Transform child2 in child.transform)
				child2.renderer.material.color = mask(child2.renderer.material.color) * c;
		///Rescale core to indicate health.
		core.transform.localScale = new Vector3(0.7f * health, 1.0f, 0.7f * health);
	}
	
	/*
 	 * Allow Jumps and drain color while Player collides with object.
	 */
	void OnCollisionEnter(Collision cx){
		foreach(ContactPoint x in cx.contacts){
			resetJump(x);
			drain(x.otherCollider);	
			
			//Action 1 : Jump Pad
			//Force the player to jump with initial velocity of arg0.
			Block b = getBlock(x.otherCollider);
			if (b!= null && b.action == 1){
				jumpVelocity = b.parameters[0];
				jump();
			}
		}
	}
	
	/*
	 * Allow Jumps and drain color while Player collides with object.
	 */
	void OnCollisionStay(Collision cx){
		foreach(ContactPoint x in cx.contacts){
			resetJump(x);
			drain(x.otherCollider);
		}
	}
	
	/*
	 * Handles Trigger Events.
	 	 * Tag: Death - Forces Player to respawn.
	 	 * Tag: Respawn - Sets the Players respawn point and rotates gravity.
		 * Tag: Coin - Collects Coin.
	 */
	void OnTriggerEnter(Collider cx){
		if (!drain(cx)){
			if (cx.tag.Equals("Death"))
				respawn();
			if (cx.tag.Equals("Respawn")){
				spawn = (GravWell)cx.GetComponent(typeof(GravWell));
	            phys = spawn.phys;
				finalGravityAngle = cx.transform.eulerAngles.z*Mathf.PI/180.0f;
				rotateSpeed = spawn.rotateSpeed;
			}
		}
	}

	void OnTriggerStay(Collider cx){
		drain(cx);
	}
	/*
	 * Sets onGround to true if Player hits a surface within 60 degrees of gravity's direction.
	 */
	private void resetJump(ContactPoint x){
		float cosTheta = Vector3.Dot(x.normal, up()) / x.normal.magnitude;
		if (cosTheta >= Mathf.Cos(jumpReset))
			onGround = true;			
	}
	
	/*
	 * Activates all Rigidbodies defined in spawn's phys array.
	 */
    private void wake(){
        for (int i = 0; i < phys.Length; i++){
            Block b = (Block)phys[i].GetComponent<Block>();
            if (b != null)
                b.wake();
        }
    }
	
	/*
	 * Casts a collider to a Block
	 */
	private Block getBlock(Collider x){
		return (Block) x.GetComponent(typeof(Block));
	}
		
	/*
	 * Drains the Player's color by a certain amount specified by a collider.
	 */
	private bool drain(Collider x){
		Block b = getBlock(x);
		if (b != null){
			decrementColor(b.drainRate * b.getColor());
			return true;
		}
		return false;
	}

	/*
	 * Decreases a color by 1% of dec.
	 */
	private void decrementColor (Color dec){
		dec *= 0.01f;
		dec.a = 0;		

		Color x = c - dec;
		x.r = Mathf.Max(minColor, x.r);
		x.g = Mathf.Max(minColor, x.g);
		x.b = Mathf.Max(minColor, x.b);
		c = x;
	}
	
	void giveColor(float[] x){
		
		if(x[0] == 1f){//red
			c.r += x[1]/3.0f;
			//getColor();
		    //renderer.material.color = Color.red;
		}
		
		else if(x[0] == 2f){//green
            c.g += x[1] / 3.0f;
        //    getColor();
		    
		}
		else if(x[0] == 3f){// blue
            c.b += x[1] / 3.0f;
          //  getColor();
		    
        }
		
	}
	
	/*
	 * Respawns the Player at spawn.
	 */
	private void respawn(){
		c = spawn.c;
	 	transform.rotation = spawn.transform.rotation;
		transform.position = spawn.transform.position;
	}
}