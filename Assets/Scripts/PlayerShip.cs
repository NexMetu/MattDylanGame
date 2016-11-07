/* Matthew lang 2016	
 * This object is for the infomation that each player ships needs to hold such as 
 * its movemnt range, sight range, hp etc.
 * 
 * Methods in this are to effect the ship itself. 
 * 
 */

using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour
{
	public int movement;
	public int sight;
	public int[,] Location;

	// Use this for initialization
	void Start ()
	{
		sight = movement + 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

