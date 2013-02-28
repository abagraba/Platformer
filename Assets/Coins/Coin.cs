//Dan Yirinec dry9

using UnityEngine;
using System.Collections;

public class Coin : Environment {
	
	public float[] x =new float[2]; //First place denotes coin color, second denotes coin size
	// Use this for initialization
	void Start () {
		minIntensity = 0.7f;
		maxIntensity = 0.9f;
		drainRate *= -50; // The Coin gives between 0.0 to 1.0 of a color to the Player.
	}
		
	void OnTriggerEnter(Collider other) //Activated when trigger zone is entered
	{
		//if(other.tag==Player) //Makes sure collision object is the Player
		{
			if(color == Color.red) //Checks if Red
			{
				x[0] = 1f; //Red is 1
			}
			if(color == Color.green) //Checks if Green
			{
				x[0] = 2f; //Green is 2
			}
			if(color == Color.blue) //Checks if Blue
			{
				x[0] = 3f; //Blue is 3
			}
			x[1] = gameObject.renderer.bounds.size.x; //Small=1, Medium=2, Large=3
			//other.SendMessage("giveColor"); //Tells Player what they ran into
			ParticleSystem particlesystem = (ParticleSystem)gameObject.GetComponent("ParticleSystem"); //Finds particle system for Coin
			particlesystem.enableEmission = true; //Turns On particle system
			AudioSource audiosource = (AudioSource)gameObject.GetComponent("AudioSource"); //Finds audio system for Coin
			audiosource.Play(); //Turns On audio system
			Destroy(gameObject); //Destroys the coin
		}
	}
}