using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	public float speed;
	public float life = 2f;
	public Vector3 ScaleB;
	public float explosionForce;
	public float cubeForce, cubeRadius;
	public float explosionRadius;
	public float waitTime = 2;
	float cubeTime, cubeTimer = 2f;
	 Vector3 blockScale, blockDirection, ScaleC;
	public AudioClip  PlayerGrow, PlayerHit, PlayerDeath, Pickup;
	private int count;
	public GUIText countText, winText;
	
	// Use this for initialization
	void Start () {
		count = 0;
		SetCountText();
		winText.text = "";
	
	}
	
	void FixedUpdate () //control system accessing rigidbody
	{
		
		
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		
		rigidbody.AddForce(movement*speed * Time.deltaTime);
		
		
		
	}
	void OnCollisionStay(Collision other) //function to decrease players life and mass on contact with enemy
	{
		if(other.gameObject.tag == "Enemy")
		{
			life = (life * 0.95f);
			ScaleB = new Vector3 (life,life,life); //new player scale based on players life value created
			//ScaleC = gameObject.transform.localScale + ScaleB;
			gameObject.transform.localScale = ScaleB;
			audio.PlayOneShot(PlayerHit);
			
			
			blockScale = new Vector3(0.1f,0.1f,0.1f); //on contact, player discard bits of himself, primitives created to simulate this
			blockDirection = new Vector3(0, Random.Range(0.1f,0.5f), 0);
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.AddComponent<Rigidbody>();
			
		
			cube.transform.position = transform.position + blockDirection;
			cube.rigidbody.mass = 0.1f;
			
			cube.transform.localScale = blockScale;
			cubeTime += Time.deltaTime;
			if (cubeTime >= cubeTimer)
			{
				cube.SetActive (false);
				cubeTimer=0;
			}
			//ScaleC = new Vector3 (0,0,0);
			//ScaleC = gameObject.transform.localScale + ScaleB;
			//cube.transform.localScale = Vector3.Lerp(cube.transform.localScale, ScaleC, Time.deltaTime);
			cube.rigidbody.AddExplosionForce(cubeForce, cube.transform.position, cubeRadius); //adding explosion forces to the discarded cubes and the player
			this.rigidbody.AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
			
			
			if (life<=0.1) //on player death, set death sound to play
			{
				gameObject.SetActive (false);
				AudioSource.PlayClipAtPoint(PlayerDeath, transform.position);
				life = 0;
			
				
				
			}
		}
		
	}
	
	void OnTriggerStay(Collider other)
	{
		
		if(other.gameObject.tag == "Ice") //calling our growth/add life on contact
		{
			GrowPlayer();
		}
	}
	void OnTriggerEnter(Collider other)
	{
	if(other.gameObject.tag == "Ice") // creates an audiosource when player enters trigger, plays grow sound
		{
			AudioSource.PlayClipAtPoint(PlayerGrow, transform.position);
			
		}
	if(other.gameObject.tag == "Pickup") // creates an audiosource when player enters trigger, plays grow sound
		{
			count = count+1;
			AudioSource.PlayClipAtPoint(Pickup, transform.position);
			other.gameObject.SetActive(false);
			SetCountText();
			
			
		}
	
	}
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Ice")
		{
			if (audio.isPlaying)
			{
				audio.Stop();
			}
			
		}
	
	}
	
	void GrowPlayer()
	{
		ScaleB = new Vector3 (life,life,life);
		//ScaleC = gameObject.transform.localScale + ScaleB;
		gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, ScaleB, Time.time); //lerping between scales, smooth transitioning
		
		life = life+1 * Time.deltaTime / 2f;
		if (life>=3)
		{
			life = 3;
		}
		
	}
	void SetCountText()
	{
		countText.text = "gold bits: " + count.ToString();
		if(count >= 8)
		{
			winText.text = "YOU WIN!";
			countText.text = "";
		}
	}
}
