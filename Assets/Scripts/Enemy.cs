using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	NavMeshAgent myNMA;
	Transform player;
	
	public Transform [] wayPointList;
	public float waitTime, waitTimer = 1f;
	
	public int waypointIndex = 0;
	

	// Use this for initialization
	void Start () 
	{
	
		myNMA = transform.GetComponent<NavMeshAgent>();
		//player = GameObject.FindWithTag("Player").transform;
		
		for (int i = 0; i < wayPointList.Length; i++)
		{
			wayPointList[i] = GameObject.Find ("waypoint"+i).transform;
		}
		

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		myNMA.destination = wayPointList[waypointIndex].position;
		
		if (myNMA.remainingDistance < myNMA.stoppingDistance)
		{
			waitTime += Time.deltaTime;
			
			if (waitTime >= waitTimer)
			{
			 	waypointIndex = (waypointIndex + 1) % 4;
			 	waitTime = 0;
			}
			
		
			
			
			//Application.LoadLevel(Application.loadedLevelName);
		
		}
		
		
	
	}
	

}

