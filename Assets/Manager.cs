using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	//Constants

	/*GAMEPLAY CONTROLS */
	//Larger the error, the wider legal orbit radius 
	private int RADIAL_ERROR = 10;
	//larger the tan error, the easier it is to enter a star at a legal radius
	private float TAN_ERROR = 8;
	//the larger this number is, the sharper bends are (est. useful range: 0 - 2)
	private float BEND_FACTOR = .065f;
	//larger the number, the faster the learth moves overall
	private float MOVEMENT_SPEED = 0.80f;
	//larger the number, the faster learth moves when orbiting (doesn't affect speed, but makes aiming easier)
	private float ORBIT_SPEED_FACTOR = .85f;
	
	/*CAMERA CONTROLS */
	//the larger this number is, the more closely the camera follows learth while in orbit
	private float ORBIT_LERP = .05f;
	//the larger this number is, the more closely the camera follows learth while not in orbit
	private float TRAVEL_LERP = 0.7F;
	//How far the player is allowed to move the camera
	private float CAM_MAX_DIST = 500;
	//How close the player is allowed to move the camera
	private float CAM_MIN_DIST = 50;
	//how fast the player can zoom in/out
	private float CAM_MOVE_SPEED = 4;
	//Camera orthographic size at start, higher = see more
	private float CAM_START_HEIGHT = 300;
	
	/*ENERGY CONTROLS */	
	//How much energy is reduced each frame while bending
	private float BEND_COST = .025f;
	//How much energy is reduced each frame while invincible
	private float INVINC_COST = .05f;
	//this much energy is subtracted each frame the learth is not in orbit
	private float FLYING_COST = .00025f;
	//this much energy is subtracted each frame the learth is in orbit
	private float ORBITING_COST = .000025f;
	//this much energy is subtracted when they player hits the space bar to launch from a star
	private float LEAVING_COST = 0;
	
	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	public GameObject coin;
	public static GameObject cur_star;
	
	//actual objects used in script
	public static GameObject l, s, e;
	public GameObject[] star_arr;
	public int numStars = 0;
	
	//level related variables, not sure how this works with different scenes. might need another class for these
	//positions past which learth will die. levels are always rectangles
	float LEVEL_X_MAX = 2000;
	float LEVEL_X_MIN = -1000;
	float LEVEL_Y_MAX = 1000;
	float LEVEL_Y_MIN = -1000;
	
	//learth-related variables
	public static float speed = 0;
	public static float energy = 5f;
	public GameObject lastStar;
	public static Vector3 tangent;
	public static bool clockwise = false;
	public static int num_deaths = 0;
	public int revisit = 0;
	
	public Color orange = new Color(1f, .6f, 0f, 1f);
	public Color dgray = new Color(.1f, .1f, .1f, 1f);
	public Texture tred;
	public Texture torange;
	public Texture tyellow;
	public Texture twhite;
	public Texture tgray;
	public Texture tblue;

	//currency
	public static int currency = 0;
	
	//current number of stars added
	private int arr_size = 0;
	
	void Start () {
		//instantiate learth
		l = Instantiate (learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
	/*	GameObject c1 = Instantiate(coin, new Vector3(5, 20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		GameObject c2 = Instantiate(coin, new Vector3(5, -20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		GameObject c3 = Instantiate(coin, new Vector3(25, 20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		GameObject c4 = Instantiate(coin, new Vector3(25, -20, 0), new Quaternion (0, 0, 0, 0)) as GameObject; */
		//instantiate stars and store them in array
		star_arr = new GameObject[0]; 
		
		//insantiate spacerips
/*		CreateSpaceRip(-400,230,25,400, 25);
		CreateSpaceRip(-400,-230,25,400, -25);
		
		CreateSpaceRip(500,-100,25,400);
		*/
		//instantiate stars
		CreateStar (0, 0, Color.white, twhite, 30f);
		/*
		//left side stars
		CreateStar (-500,75,Color.blue,tblue,70f);
		CreateStar (-500,-75,Color.blue,tblue,70f);
		
		CreateStar(-490,430,Color.blue,tblue,20f);
		CreateStar(-490,-430,Color.blue,tblue,20f);
	
		CreateStar(-900,200,Color.yellow, tyellow, 35f);
		CreateStar(-700,150,Color.white, twhite, 35f);
		CreateStar(-950,0,Color.yellow, tyellow, 35f);
		CreateStar(-1100,100,Color.white, twhite,35f);
		CreateStar(-1100,-200,Color.blue, tblue,35f);
		
		//right side stars
		CreateStar(500,200,Color.red,tred,65f);
		
		CreateStar(700, 0, Color.blue,tblue,35f);
		CreateStar (650, -100, Color.white,twhite,35f);
		CreateStar (800,-100, Color.yellow,tyellow,35f);
		CreateStar (950, -300, Color.white,twhite,45f);
		
		//middle stars
		CreateStar(-100,100,Color.blue,tblue,35f);
		CreateStar(-250, -400,Color.white,twhite,35f);
		CreateStar(200,-250,Color.yellow, tyellow,35f); */
		
		//new test level!
		CreateStar (100,100,Color.blue,tblue,35f);
		CreateStar (200,200,Color.blue,tblue,35f);
		CreateStar (350,300,Color.blue,tblue,35f);
		CreateStar (550, 400, Color.blue,tblue,35f);
		CreateStar (900, 500, Color.blue,tblue,35f);
		
		CreateStar (900, -200, Color.white,twhite,35f);
		CreateStar (850, -150, Color.blue,tblue,35f);
		CreateStar(800, 100, Color.white,twhite,35f);
		CreateStar(700, 0, Color.blue,tblue,35f);
		CreateStar (800, -250,Color.white,twhite,35f);
		CreateStar (950, 100,Color.blue,tblue,35f);
		
		CreateSpaceRip(900,0,30,30);
		CreateSpaceRip(800,-70,30,30);
		CreateSpaceRip (900,150, 30,30);
		CreateSpaceRip (725, 100,30,30);
		
		CreateSpaceRip (400,100,30,900,-55);
		
		CreateStar(-500,500,Color.blue,tblue,55f);
		CreateStar (-700, -200, Color.white,twhite,65f);
		
		//set camera height for beginning a game
		Camera.main.orthographicSize = CAM_START_HEIGHT;
	}
	
	//instantiates a space rip from prefab at given location and of given dimensions, with given rotation (default = 0), returns reference to that object
	GameObject CreateSpaceRip(float x, float y, float width, float height, float rotation = 0)
	{
		GameObject rip_actual = Instantiate (rip, new Vector3 (x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		rip_actual.transform.localScale += new Vector3(width,height,0);
		rip_actual.transform.Rotate(new Vector3(0,0,rotation));
		return rip_actual;
	}
	
	//instantiates star from prefab at given xy location and of given characteristics
	GameObject CreateStar(float x, float y, Color color, Texture texture, float size)
	{
		GameObject starE = Instantiate (star, new Vector3(x,y,0), new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = color;
		starscript.t = texture;
		starscript.starSize = size; 
		
		//expand and copy star_arr - if loading a level takes too long, this can be optimized
		GameObject[] temp_arr = new GameObject[arr_size+1];
		for(int i=0;i<arr_size;i++)
			temp_arr[i] = star_arr[i];
		star_arr = temp_arr;
		star_arr[arr_size] = starE;
		arr_size++;
		lastStar = starE;
		numStars++;
		return starE;
	}
	
	//puts Learth in orbit given an entrance point, an energy value, a velocity, a star, and a direction
	public static void MoveLearthToOrbit(Vector3 entrance_point, Vector3 entrance_velocity, float lastEnergy, GameObject star, bool cwise )
	{
		energy = lastEnergy;
		s = star;
		cur_star = s;
		Learth_Movement.isTangent = true;
		l.transform.position = Vector3.Lerp(l.transform.position,entrance_point,100.0F);
		Learth_Movement.velocity = entrance_velocity;
		clockwise = cwise;
	} 
	
	//call this anytime something kills the player
	public static void Die()
	{		
		//if you screw up too much at the beginning, or if you've died more than 3 times, just start the level over
		if(Learth_Movement.last_star_gos[num_deaths] == null || num_deaths > 2)
		{
			ResetLevel();
		}
		//otherwise, you move back 1, 2, or 3 stars
		else {
			//move learth to previous stars
			MoveLearthToOrbit(Learth_Movement.last_stars[num_deaths], 
				Learth_Movement.last_stars_velocity[num_deaths], Learth_Movement.last_energies[num_deaths],
				Learth_Movement.last_star_gos[num_deaths], Learth_Movement.last_star_rots[num_deaths]);
				
			//move with learth
			Camera.main.transform.position = new Vector3(l.transform.position.x, l.transform.position.y, Camera.main.transform.position.z);
			num_deaths++;
		}
		
	}
	
	//reloads the scene and modifies whatever we want to modify when the scene gets reloaded
	public static void ResetLevel() {
		Application.LoadLevel(Application.loadedLevel);	
		energy = 2;
	}
	
	void Update () {
		
		/*********************DEBUGGING CONTROLS********************/
		// resetting level with T before leaving first star orbit freezes the game 
		//R causes the player to die
		if(Input.GetKeyDown(KeyCode.R))
			Die();
		//T resets the level
		if(Input.GetKeyDown(KeyCode.T))
			ResetLevel();
		//Y resets camera to learth's position
		if(Input.GetKeyDown (KeyCode.Y))
			Camera.main.transform.position = new Vector3(l.transform.position.x, l.transform.position.y, Camera.main.transform.position.z);
		//U prints position and last position and their difference on demand
		if(Input.GetKeyDown(KeyCode.U))
			Debug.Log("pos: "+l.transform.position+" last pos: "+Learth_Movement.lastPos.position+" dist: "
				+Vector3.Distance(l.transform.position,Learth_Movement.lastPos.position));
		//f increases energy by 1
		if(Input.GetKeyDown(KeyCode.F))
			energy++;
		/*********************END DEBUGGING CONTROLS*****************/
		
		//Speed increases logarithmically with energy
		speed = Mathf.Log(energy)*MOVEMENT_SPEED;
		
		//bending
		if(Input.GetKey(KeyCode.Q))
		{
			energy -= BEND_COST;
			Learth_Movement.lastPos.RotateAround(Learth_Movement.velocity,Vector3.forward,-1*Time.deltaTime*BEND_FACTOR);
		}
		else if (Input.GetKey(KeyCode.W))
		{		
			energy -= BEND_COST;
			Learth_Movement.lastPos.RotateAround(Learth_Movement.velocity,Vector3.forward,Time.deltaTime*BEND_FACTOR);
		}
		
		//temporary invincibility, logic implemented in Learth_Movement.cs 
		if(Input.GetKey(KeyCode.E))
		{
			l.renderer.material.color = Color.green;
			energy -= INVINC_COST;
		}
		//change learth color back to normal
		if(Input.GetKeyUp (KeyCode.E))
			l.renderer.material.color = Color.white;
		
		//Death conditions
		//if you run out of energy, you die, but you get a little energy back
		if(energy < 1)
		{
			Die ();
			energy = 2f;
		}
		
		//if you travel outside the bounds of the level, you die
		if(l.transform.position.x > LEVEL_X_MAX
			|| l.transform.position.x < LEVEL_X_MIN
			|| l.transform.position.y > LEVEL_Y_MAX
			|| l.transform.position.y < LEVEL_Y_MIN)
			Die ();
		
		
		//if learth is tangent to star s
		if (Learth_Movement.isTangent) {
			//if in orbit, decrease energy at correct rate
			energy -= ORBITING_COST;
			
			//rotate around star s
			if (clockwise){
				l.transform.RotateAround(s.transform.position, Vector3.forward, 
					-(speed > 1 ? speed : 1)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime)*ORBIT_SPEED_FACTOR);
			}
			else  {
				l.transform.RotateAround(s.transform.position, 
					Vector3.forward, (speed > 1 ? speed : 1)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime/ORBIT_SPEED_FACTOR));
			}
			if (Vector3.Distance (l.transform.position, tangent) < 2) {
				revisit++;
				if (revisit == 1) {
					energy -= 1f;
				}
			}
			else {
				revisit = 0;
			}
			
			//if space bar is pressed, accelerate away from star. 
			if (Input.GetKeyDown(KeyCode.Space)) {
				Learth_Movement.isTangent = false;
				lastStar = s;			
				energy -= LEAVING_COST;
				Learth_Movement.lastPos.position = l.transform.position - Learth_Movement.velocity.normalized*speed;
			}
		}
		//if earth is not tangent to any star
		else if (!Learth_Movement.isTangent) {
			//if not in orbit, decrease energy at correct rate
			energy -= FLYING_COST;
			
			//loop through array and calculate tangent vectors to every star
			for (int i = 0; i < numStars; i++){
				s = star_arr[i];
				Starscript sscript = s.GetComponent<Starscript>();
				Vector3 l_movement = Learth_Movement.velocity;
				Vector3 star_from_learth = s.transform.position - l.transform.position;
				Vector3 projection = Vector3.Project (star_from_learth, l_movement);
				tangent = projection + l.transform.position;
				//if planet is within star's orbital radius, set isTangent to true
				if (s != lastStar 
					&& Vector3.Distance(s.transform.position, l.transform.position) >= (sscript.orbitRadius - RADIAL_ERROR) 
					&& Vector3.Distance(s.transform.position, l.transform.position) <= (sscript.orbitRadius + RADIAL_ERROR) 
					&& Vector3.Distance (tangent, l.transform.position) <= TAN_ERROR) 
				{	
					Learth_Movement.isTangent = true;
					cur_star = s;
					//determine direction of orbit
					if (tangent.y < s.transform.position.y && l_movement.x < 0) { 
						clockwise = true;
					}
					else if (tangent.y > s.transform.position.y  && l_movement.x > 0) {
						clockwise = true;
					}		
					else if (tangent.x < s.transform.position.x && l_movement.y > 0) {
						clockwise = true;
					}
					else {
						clockwise = false;
					}
					
					//update last stars, last energy value, last entrances, last velocity vectors, and last rotations to include this star
					for(int k=2; k>0;k--)
					{
						
						Learth_Movement.last_stars[k] = Learth_Movement.last_stars[k-1];
						Learth_Movement.last_energies[k] = Learth_Movement.last_energies[k-1];
						Learth_Movement.last_stars_velocity[k] = Learth_Movement.last_stars_velocity[k-1];
						Learth_Movement.last_star_gos[k] = Learth_Movement.last_star_gos[k-1];
						Learth_Movement.last_star_rots[k] = Learth_Movement.last_star_rots[k-1];
					}
					Learth_Movement.last_stars[0] = l.transform.position;
					Learth_Movement.last_energies[0] = energy;
					Learth_Movement.last_stars_velocity[0] = l_movement;
					Learth_Movement.last_star_gos[0] = s;
					Learth_Movement.last_star_rots[0] = clockwise;
					
					//add appropriate energy value depending on color of star
					if (sscript.c == Color.blue) {
						energy += 5f;
					} else if (sscript.c == Color.white){
						energy += 4f;
					} else if (sscript.c == Color.yellow) {
						energy += 3f;
					} else if (sscript.t == torange) {
						energy += 2f;
					} else if (sscript.c == Color.red) {
						energy += 1f;
					}
					else {
						energy -= 1f;
					}
					sscript.c = dgray;
					sscript.t = tgray;
					break;
				}
			}
		}
	
		//camera follows learth
		Camera.main.transform.position = Learth_Movement.isTangent ? 
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(l.transform.position.x,l.transform.position.y,Camera.main.transform.position.z),ORBIT_LERP*Time.deltaTime)
				:
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(l.transform.position.x,l.transform.position.y,Camera.main.transform.position.z),TRAVEL_LERP*Time.deltaTime)
				;
		
		//A moves the camera farther, S moves the camera closer
		if(Input.GetKey(KeyCode.A) && Camera.main.orthographicSize <= CAM_MAX_DIST)
			Camera.main.orthographicSize += CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.S) && Camera.main.orthographicSize >= CAM_MIN_DIST)
			Camera.main.orthographicSize -= CAM_MOVE_SPEED;
		
	
	}
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 150, 50), "Energy: " + energy + ", Currency: "+currency);
    }
		
}
	