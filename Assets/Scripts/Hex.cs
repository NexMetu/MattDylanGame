using UnityEngine;
using System.Collections;

public class Hex {
	//if the player can move a unit throught this tile
	public bool traversable = false;
	//if an event is possiable to happen on moving onto this tile
	public bool Event = false; //need to change this when i get events up and running @Matt
	//if any resources are located on this tile
	public GameResource[] resources;
	//if there rescue boat/small boats to check
	public bool boatcheck = false; //need to change this once i have the boat moving and need it to collect resources etc
	//if this tile has been seen by the player
	public bool seen = false;
	//if the player is on that tile
	public bool player = false;
	//if this tile is in sight of the player this turn;
	public bool InSight = false;

	 
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}
