using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {
	//this is to show fog of war; 0 havnt seen the tile yet, 1 seen but not currently in sight, 2 in sight range of the player
	public int visablility;
	public Sprite fog;
	public Sprite tile;
	public Sprite explored;
	public SpriteRenderer SprRen;

	// Use this for initialization
	void Start () {
		visablility = 0;
		tile = GetComponent<SpriteRenderer>().sprite;
	}
	// Update is called once per frame
	void Update () {
		if (visablility == 0) {
			GetComponent<SpriteRenderer> ().sprite = fog;
		} else if (visablility == 1) {
			GetComponent<SpriteRenderer> ().sprite = explored;
		} else {
			GetComponent<SpriteRenderer> ().sprite = tile;
		}
	}
}
