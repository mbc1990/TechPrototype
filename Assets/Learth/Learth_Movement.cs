using UnityEngine;
using System.Collections;

public class Learth_Movement : MonoBehaviour {
	
	
	public static Vector3 velocity = new Vector3(1f, 1f, 0f);
	//public static Vector3 lastPos;
	public static Transform lastPos;
	public static bool isTangent = false;
	
	//explosion prefab
	//public GameObject explosion, e;
	
	//current and previous 3 positions at which the learth entered
	public static Vector3[] last_stars = new Vector3[4];
	//current and previous 3 velocity vectors of entrance points
	public static Vector3[] last_stars_velocity = new Vector3[4];
	//current and previous 3 star game objects
	public static GameObject[] last_star_gos = new GameObject[4];
	//current and previous 3 clockwise rotations
	public static bool[] last_star_rots = new bool[4];
	//current and previous 3 last energy values at previous 3 stars
	public static float[] last_energies = new float[4];
	
	void Start () {
		//initialize last stars array
		for(int i=0;i<3;i++)
			last_star_gos[i] = null;
		
		//initialize lastPos
		lastPos = new GameObject().transform;
		
		lastPos.position = this.transform.position - new Vector3(Manager.speed, Manager.speed, 0f);	
	}	
	
	void Update () {
		//calculate velocity every frame
		velocity = this.transform.position - lastPos.position;
		lastPos.position = this.transform.position;
		//regular non-orbiting movement
		if (!isTangent) {
			this.transform.position += velocity.normalized*Manager.speed;	
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if(!Input.GetKey(KeyCode.E))
		{
			//if learth collides with a space rip, die
			if(collision.gameObject.name == "Space_Rip(Clone)") {
				Manager.Die();
			}
			//if learth collides with a star, die
			if(collision.gameObject.name == "Star(Clone)") {
				Manager.Die();	
			}
			if(collision.gameObject.name == "coin(Clone)") {
				Manager.currency++;
				Destroy(collision.gameObject);
			}
			
		}
	}	
	
	
	
}