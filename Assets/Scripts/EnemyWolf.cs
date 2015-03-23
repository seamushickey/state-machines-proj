using UnityEngine;
using System.Collections;

public class EnemyWolf : MonoBehaviour {

	NavMeshAgent myNMA;
	Transform player, trap;
	
	public Transform [] wayPointList;
	public float waitTime, waitTimer = 1f, trapTime, trapTimer = 6f;
	public int waypointIndex = 0;
	//Ray ray;
	//RaycastHit hit;
	//public float fovrange;
	public AudioClip WolfPatrol, WolfAttack;
	
	bool playAudio = false;
	Vector2 a, b;
	public Transform target;
	
	
	
	//Transform[] children;
	//public float scrollSpeed = 0.5F;
	//public Renderer rend;
	
	enum States //setting up the four states the wolf can exist in
	{
		Initialize,
		Patrol,
		Attack,
		Trapped,
		
	}
	States currentState = States.Initialize; //sets initialize as the start state
	
	
	
	// Update is called once per frame
	void Update () 
	{
		switch(currentState) //setting up switch function on update, so the state changes on whichever frame state change is called
		{
		case States.Initialize:
			Initialize();
			break;
		case States.Patrol:
			Patrol ();
			break;
		case States.Attack:
			Attack();
			break;
		case States.Trapped:
			Trapped();
			break;
			
		
			
		}
		
		if(playAudio && !audio.isPlaying) //an attempt to get a non-overlapping sound in game
		{
			audio.PlayOneShot(WolfAttack);
			
			playAudio=false;
		}
		//Debug.DrawRay(transform.position, transform.forward * 10f, Color.magenta);
	}
	
	void Initialize()
	{
		
		//rend = GetComponent<Renderer>();
		myNMA = transform.GetComponent<NavMeshAgent>(); // accessing the nav mesh agent in unity
		//player = GameObject.FindWithTag("Player").transform;
		
		for (int i = 0; i < wayPointList.Length; i++) //adding waypoints from list and interating through them using a for loop
		{
			wayPointList[i] = GameObject.Find ("waypoint"+i).transform;
		}
		currentState = States.Patrol; //sets state to patrolling, wolf heads to first waypoint
		
		
	}
	
	void Patrol()
	{
		audio.PlayOneShot(WolfPatrol); //plays public audio clip of wolf patrol (left empty because of overlapping problems)
		gameObject.animation.Play("WolfIdle");
		
		myNMA.destination = wayPointList[waypointIndex].position;
		myNMA.speed = 3.5f;
		
		if (myNMA.remainingDistance < myNMA.stoppingDistance)
		{
			waitTime += Time.deltaTime;
			
			if (waitTime >= waitTimer)
			{
				waypointIndex = (waypointIndex + 1) % 6;
				waitTime = 0;
			}
		}
		
		FieldOfView(); //runs field of view script, which handles state change to attack
		
		
		/*trap = GameObject.FindWithTag("Trap").transform; // the other ways in which i tried to make wolf stay in trap
		if(Vector3.Distance(transform.position, trap.position) < 1f)
		{
			currentState = States.Trapped;
		}*/
		
		/*ray = new Ray(transform.position, transform.forward);
		if(Physics.Raycast(ray, out hit, 10))
		{
			if(hit.collider.tag == "Player")
			{
				
				currentState = States.Attack;
			}
			
		}*/
		
		
	}
	
	void Attack()
	{
		
		//float offset = Time.time * scrollSpeed;
		//rend.material.mainTextureOffset = new Vector2(offset, 0);
		//audio.PlayOneShot(WolfAttack);
		//playAudio = true;
			
		player = GameObject.FindWithTag("Player").transform; //sets the wolf on players position by accessing its tag
		myNMA.destination = player.transform.position;
		myNMA.speed = 7f; //doubles move speed
		gameObject.animation.Stop("WolfIdle"); //switches animation from idling to attacking
		gameObject.animation.Play("WolfAttack");
		
		trap = GameObject.FindWithTag("Trap").transform; //creates a reference to trap in scene
		
		/*if (angle >= fov)
		{
			currentState = States.Initialize;
		
		}
		
		*/if(Vector3.Distance(transform.position, player.position) > 7f) //if player is 7 units or more away from enemy, return to patrol state
		{
			currentState = States.Patrol;
		}
		else
		{
			playAudio = true; //if he is closer, play his audio
		
		}
		if(Vector3.Distance(transform.position, trap.position) < 1f) //if wolf is within 1 unit of trap, switch to trapped state
		{
			currentState = States.Trapped;
		}
		
		
	}
	
	
	void Trapped()
	{
		gameObject.animation.Stop("WolfAttack"); //switches animation from attacking to idling
		gameObject.animation.Play("WolfIdle");
		trap = GameObject.FindWithTag("Trap").transform; //sets wolfs pos to trap pos and holds him there for as long as the timer is running
		myNMA.destination = trap.transform.position;
		trapTime += Time.deltaTime;
		
		if(trapTime >= trapTimer)
		{
			trapTimer = 0;
			currentState = States.Patrol;
			
		}
	
	}
	
	
	
	public bool FieldOfView() //function to handle cone of vision of enemy
	{
		Vector3 forwardDirection = transform.TransformDirection(transform.forward); //sets facing direction to forward
		Vector3 vectorToOther = target.position - transform.position;
		vectorToOther.Normalize();  
		
		Debug.DrawLine (transform.position, target.position, Color.magenta);
		Debug.DrawLine (transform.position, transform.position + transform.forward * 4, Color.cyan);
		
		float dot = Vector3.Dot(vectorToOther, transform.forward); //takes two vectors and returns the dot product between them

		float fov = 45f;
		float angle = Mathf.Acos(dot) * Mathf.Rad2Deg; 
		
		if(angle < fov) //if the angle of dot is less than 45 degrees, target in sight, switch to attack state
		{
			
			currentState = States.Attack;
			return true;
		}
		else
		{
			
			return false;
		}
	}
}
