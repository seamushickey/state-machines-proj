using UnityEngine;
using System.Collections;

public class EnemyCroc : MonoBehaviour {

	NavMeshAgent myNMA;
	Transform player;
	
	public Transform [] wayPointList;
	public float waitTime, waitTimer = 1f;
	public int waypointIndex = 0;
	Ray ray;
	RaycastHit hit;
	
	enum States
	{
		Initialize,
		Idle,
		Attack,
		Search,
		
	}
	States currentState = States.Initialize;
	

	
	// Update is called once per frame
	void Update () 
	{
		switch(currentState)
		{
		case States.Initialize:
			Initialize();
			break;
		case States.Idle:
			Idle ();
			break;
		case States.Attack:
			Attack();
			break;
		case States.Search:
			Search();
			break;
			
		}
		Debug.DrawRay(transform.position, transform.forward * 10f, Color.magenta);
	}
	
	void Initialize()
	{
		myNMA = transform.GetComponent<NavMeshAgent>();
		//player = GameObject.FindWithTag("Player").transform;
		
		for (int i = 0; i < wayPointList.Length; i++)
		{
			wayPointList[i] = GameObject.Find ("waypoint"+i).transform;
		}
		currentState = States.Idle;
	}
	
	void Idle()
	{
		
		
		myNMA.destination = wayPointList[waypointIndex].position;
		myNMA.speed = 3.5f;
		
		if (myNMA.remainingDistance < myNMA.stoppingDistance)
		{
			waitTime += Time.deltaTime;
			
			if (waitTime >= waitTimer)
			{
				waypointIndex = (waypointIndex + 1) % 4;
				waitTime = 0;
			}
		}
	
		ray = new Ray(transform.position,transform.forward);
		if(Physics.Raycast(ray, out hit, 10))
		{
			if(hit.collider.tag == "Player")
			{
				
				currentState = States.Attack;
			}
			
		}
	}
	
	void Attack()
	{
		
		player = GameObject.FindWithTag("Player").transform;
		myNMA.destination = player.transform.position;
		myNMA.speed = 7f;
		
		if(Vector3.Distance(transform.position, player.position) > 10f)
		{
			currentState = States.Idle;
		}
	}
	
	void Search()
	{
		
		
		
	}
}
