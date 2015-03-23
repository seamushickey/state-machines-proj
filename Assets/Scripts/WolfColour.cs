using UnityEngine;
using System.Collections;

public class WolfColour : MonoBehaviour {
	
	public float scrollSpeed = 0.5F;
	public Renderer rend;
	public bool isScrolling;
	
	// Use this for initialization
	void Start () {
	
		rend = GetComponent<Renderer>(); //accessing this objects renderer
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		
	}
	
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Trap") //calls colourchanging function, which accesses offset of texture position on the x position 
		{
			ColourScroll();
		
		}
	}
	void OnTriggerExit(Collider other)
	{
			if (other.gameObject.tag == "Trap") // resets texture to original when object leaves trap
			{
				rend.material.mainTextureOffset = new Vector2(0, 0);
				
			}
	
	}
	
	void ColourScroll()
	{
	isScrolling = true;
		if(isScrolling == true)
		{
			float offset = Time.time * scrollSpeed;
			rend.material.mainTextureOffset = new Vector2(offset, 0);
	}
}
}