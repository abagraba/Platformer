//Dan Yirinec dry9

using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	
	float[] x =new float[2]; //First place denotes coin color, second denotes coin size
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) //Activated when trigger zone is entered
	{
		//if(other.tag==Player) //Makes sure collision object is the Player
		{
			if(gameObject.renderer.material.GetColor("_Color") == Color.red) //Checks if Red
			{
				x[0] = 1f; //Red is 1
			}
			if(gameObject.renderer.material.GetColor("_Color") == Color.green) //Checks if Green
			{
				x[0] = 2f; //Green is 2
			}
			if(gameObject.renderer.material.GetColor("_Color") == Color.blue) //Checks if Blue
			{
				x[0] = 3f; //Green is 3
			}
			x[1] = gameObject.renderer.bounds.size.x; //Small=1, Medium=2, Large=3
			other.SendMessage("GiveColor", x); //Tells Player what they ran into
			ParticleSystem particlesystem = (ParticleSystem)gameObject.GetComponent("ParticleSystem"); //Finds particle system for Coin
			particlesystem.enableEmission = true; //Turns On particle system
			AudioSource audiosource = (AudioSource)gameObject.GetComponent("AudioSource"); //Finds audio system for Coin
			audiosource.Play(); //Turns On audio system
			Destroy(gameObject); //Destroys the coin
		}
	}
}