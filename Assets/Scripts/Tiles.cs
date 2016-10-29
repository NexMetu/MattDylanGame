/*
 * Dylan Pringle 2016 Tiles
 * 
 * 
 * 
*/

using UnityEngine;
using System.Collections;

public class Tiles : MonoBehaviour {


	SpriteRenderer rend;
	// Use this for initialization
	void Start () {

		rend = GetComponent<SpriteRenderer> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {

		Debug.Log ("here");
		rend.enabled = true;

	}

	void OnMouseExit() {
		
		rend.enabled = false;

	}
}
