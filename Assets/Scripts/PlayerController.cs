using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed;
	private int count;
	public float life;
	
	public GUIText countText;
	public GUIText winText;
	public GUIText lifeText;
	public GUIText loseText;
	
	public Material matFirst;
	public Material matYellow;
	public Material matBlue;
	public AudioClip shrinkWav;
	public AudioClip growWav;
	public ParticleEmitter snowEmit;
	
	public bool isPlayerYellow;
	
	public float duration = 0.5f;
	float lerpControl = 0;
	
	void Start()
	{
		life = 2;
		count = 0;
		SetCountText();
		winText.text = "";
		gameObject.renderer.material = matFirst;
	}
	
	void FixedUpdate () 
	{
		
		
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		
		rigidbody.AddForce(movement*speed * Time.deltaTime);
		lifeText.text = "LIFE: " + (life * 50).ToString();
		
		
	}
	
	
	
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "PickUp")
		{
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
			
		}
		else if(other.gameObject.tag == "Fire")
		{
			audio.PlayOneShot(shrinkWav);
		}
		else if(other.gameObject.tag == "Ice")
		{
			audio.PlayOneShot(growWav);
		}
		
		
		
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Fire")
		{
			ShrinkPlayer();
			
			
		}
		else if(other.gameObject.tag == "Ice")
		{
			GrowPlayer();
			ColourBlue();
			
				
		}
		else if(other.gameObject.tag == "Reset")
		{
			Application.LoadLevel (0);
		}
		else if(other.gameObject.tag == "Yellow")
		{
			ColourPlayer();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
	
		if(other.gameObject.tag == "Yellow")
			{
				ColourPlayer();
			}
	
	}
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			life = (life - 0.1f);
			ScaleB = new Vector3 (life,life,life);
			//ScaleC = gameObject.transform.localScale + ScaleB;
			gameObject.transform.localScale = ScaleB;
			
			if (life<=0)
			{
				gameObject.SetActive (false);
				loseText.text = "CROCADILLYS GOTCHA!";
				
				
			}
		}
		
	}
	
	
	void SetCountText()
	{
		countText.text = "glo-bricks: " + count.ToString();
		if(count >= 12)
			{
				winText.text = "YOU DA WINNER!";
			}
	}
	
	public Vector3 ScaleB;
	Vector3 ScaleC;
	
	void ShrinkPlayer()
	{
		
		ScaleB = new Vector3 (life,life,life);
	//ScaleC = gameObject.transform.localScale + ScaleB;
	gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, ScaleB, Time.time);
	
		life = life-1 * Time.deltaTime / 2f;
		if (life<=0)
		{
			gameObject.SetActive (false);
			loseText.text = "AW SNAP, YOU LOSE!";
			
			
		}
	}
	
	/*if(gameObject.collider.x<= 0.1 )
	{
	Destroy (gameObject);
	}
	}
	*/
	void GrowPlayer()
	{
		ScaleB = new Vector3 (life,life,life);
		//ScaleC = gameObject.transform.localScale + ScaleB;
		gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, ScaleB, Time.time);
		life = life+1 * Time.deltaTime / 2f;
		if (life>=2)
		{
			life = 2;
		}
	
	}

	void ColourPlayer()
	{
		
		
		
			isPlayerYellow = true;
			renderer.material.Lerp(matFirst, matYellow, lerpControl);
			lerpControl += Time.deltaTime * 2;
		
		/*renderer.material.Lerp(matFirst, matYellow, lerpControl);
		lerpControl += Time.deltaTime/duration;*/
	
	}
	void ColourBlue()
	{
		if(isPlayerYellow == true)
		{
			gameObject.renderer.material = matYellow;
		renderer.material.Lerp(matYellow, matFirst, lerpControl);
		lerpControl += Time.deltaTime * 2;
		isPlayerYellow = false;
			
		}
	}
	
void ColourClear()
{



}
}
