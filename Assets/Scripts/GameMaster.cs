/*
	Dylan Pringle 2016 Hexgrid
	Matthew Lang 2016 Fog


*/
using UnityEngine;
using System.Collections;


public class GameMaster : MonoBehaviour {
	//arrays that deal with the metadata of the hexgrid and Fog of war
	public int [,] HexGrid;
	public int[,] visable; //this ends to be made into the tileinfo object

	//arrays that store each time of the hexgrid and Fog of war
	public Vector3[,] TileGrid;
	public Transform[,] FogGrid;

	//holds all the transform for the hexgrid
	public Transform[] sea_RoughPrefab;
	public Transform[] sea_CalmPrefab;
	public Transform[] grass_DirtPrefab;

	//an array to hold all player objects
	public Transform[] playerObjects;
	//an array of different ships to use as new ships
	public Transform playershipspawn;

	public Transform fog; //gameobject to hold the fog sprites
	public Sprite fogs; //havn't explored/seen yet
	public Sprite OoS; //out of sight
    
	//size of the game map
	public int hexSize;
	//Setting to turning fog on or off at the start of the game;
	public bool FogOfWarOn = false;

	//holds infomation on every tile
	public Hex[,] Tileinfo;

	//Values that set the number of islands to make; the max and min size of each island
	public int numberOfIslands;
	public int minSizeOfIslands;
	public int maxSizeOfIslands;

	//sets the max distance in tiles the shallows of water from a mass of land can be placed
	public int minSizeOfShallows;
	public int maxSizeOfShallows;

 	// Use this for initialization
	void Start () {
		HexGrid = new int [hexSize , hexSize];
		visable = new int [hexSize, hexSize];
		TileGrid = new Vector3[hexSize, hexSize];
		FogGrid = new Transform[hexSize, hexSize];
		Tileinfo = new Hex[hexSize, hexSize];
		playerObjects = new Transform[20];
				// Fill Hex with integer values that will correspond to map tiles
		FillHex ();

		// Fill Hex with map tiles corresponding to integer values
		PopulateHex ();

		//place the player onto the map
		placePlayer();
		filltileinfo ();
		//sets the basic visablility to 0 on start up (fog should be all up)
		print("just doing fog");
		if (FogOfWarOn) {
			for (int x = 0; x < hexSize; x++) {
				for (int y = 0; y < hexSize; y++) {
					print("got into fog script");
					FogGrid [x, y] = Instantiate (fog);
					Vector3 xv = TileGrid [x, y];
					FogGrid [x, y].position = xv;

				}
			}
			fogofwar ();
		}
	}
	// Update is called once per frame
	void Update () {
		if (FogOfWarOn) {
			fogofwar ();
		}

	}
    
	//used in Update to make sure that 
    void fogofwar()
    {
   		if (playerObjects.Length != 0) {
			int sightrange = playerObjects [0].transform.GetComponent<PlayerShip> ().sight;
			print (sightrange);
			int x = playerObjects [0].transform.GetComponent<PlayerShip> ().Location[0];
			int y = playerObjects [0].transform.GetComponent<PlayerShip> ().Location[1];
			for (int x1 = x - sightrange; x1 <= x + sightrange; x1++) {
				if (x1 >= 0 && x1 < hexSize) {
					for (int y1 = y - sightrange; y1 <= y + sightrange; y1++) {
						//&& (x1 - x != Mathf.Abs(sightrange) && y1 - y != Mathf.Abs(sightrange))
						if (y1 >= 0 && y1 < hexSize ) {
							print (x1);
							print (y1);
							print (Tileinfo [x1, y1]);
							Tileinfo [x1, y1].seen = true;
							Tileinfo [x1, y1].InSight = true;
						}
					}
				}
			}
		}
		for (int x = 0; x < hexSize; x++) {
			for (int y = 0; y < hexSize; y++) {
				if (Tileinfo [x, y].InSight) {
					FogGrid [x, y].GetComponent<SpriteRenderer> ().enabled = false; 
				} else if (Tileinfo [x, y].seen) {
					FogGrid [x, y].GetComponent<SpriteRenderer> ().sprite = OoS;
				} else {
					FogGrid [x, y].GetComponent<SpriteRenderer> ().sprite = fogs;
				}
			}
	    }
	}

	// Fill Hex with integer values that will correspond to map tiles
	void FillHex() {

		// X value
		int colLength = hexSize-1;
		// Y value
		int rowLength = hexSize-1;


		// Filling Grid with Island Nodes
		for (int i = 0; i < numberOfIslands; i++) {

			//  --------rows, columns
			// HexGrid [5   ,    10] = 2;
			HexGrid [Random.Range (0 + maxSizeOfIslands + maxSizeOfShallows, colLength - maxSizeOfShallows) , Random.Range (0 + maxSizeOfIslands + maxSizeOfShallows, rowLength - maxSizeOfShallows)] = 2;

		}  


		// First for loops scan Hex grid for island Nodes
		// Rows iterator
		for (int x = 0; x < rowLength; x++) {
			// Columns iterator
			for (int y = 0; y < colLength; y++) {

				if (HexGrid [x, y] == 2) {
					// When Island node found assign
					// Different size of island each iteration
					int islandSize = Random.Range(minSizeOfIslands , maxSizeOfIslands+1);

					// Second for loops place more island pieces around island Node
					// Rows iterator
					for (int i = x - islandSize ; i <= x ; i++) {
						// Columns iterator
						for (int j = y - islandSize; j <= y; j++) {

							HexGrid [i, j] = 2;

							// a layer of shallow sea tiles set around it
							int shallowSize = Random.Range(minSizeOfShallows , maxSizeOfShallows+1);

							// Third for loops Each added island tile assigns 
							// a layer of shallow sea tiles set around it
							// Rows iterator
							for (int c = i - shallowSize; c <= i + shallowSize; c++) {
								// Columns iterator
								for (int k = j - shallowSize; k <= j + shallowSize; k++) {

									// Assigning calm sea tiles
									if (HexGrid [c, k] == 2) {
										HexGrid [c, k] = 2;
									} else {
										HexGrid [c, k] = 1;
									}
								}
							}
						}
					}
				}
			} 
		}


		// Fill Hex with map tiles corresponding to integer values
		//PopulateHex ();
	}




	// Fill Hex with map tiles corresponding to integer values
	void PopulateHex() {

		// Geographical Placement of tiles 
		float colPlace = 0;
		float rowPlace = 0;

		// Row and Column Count 
		int rowCount = 0;
		int colCount = 0;

		// hex rows sitting at different positions
		int offset = 0;

		// X value
		int colLength = hexSize-1;
		// Y value
		int rowLength = hexSize-1;

		// Assigns Tiles to Grid Numbers
		// Rows iterator
		for (int i = 0; i <= rowLength; i++) {
			// Columns iterator
			for (int j = 0; j <= colLength; j++) {

				if (HexGrid [rowCount, colCount] == 0) {
					Transform sea_CalmTile = Instantiate (sea_RoughPrefab[Random.Range (0, sea_RoughPrefab.Length)]);
					sea_CalmTile.transform.Translate (new Vector2 (colPlace, rowPlace));
					TileGrid [i, j] = sea_CalmTile.position;
				}
				if (HexGrid [rowCount, colCount] == 1) {
					Transform sea_RoughTile = Instantiate (sea_CalmPrefab[Random.Range (0, sea_CalmPrefab.Length)]);
					sea_RoughTile.transform.Translate (new Vector2 (colPlace, rowPlace));
					TileGrid [i, j] = sea_RoughTile.position;
				}
				if (HexGrid [rowCount, colCount] == 2) {
					Transform grass_DirtTile = Instantiate (grass_DirtPrefab[Random.Range (0, grass_DirtPrefab.Length)]);
					grass_DirtTile.transform.Translate (new Vector2 (colPlace, rowPlace));
					TileGrid [i, j] = grass_DirtTile.position;
				}

				colPlace += 1.4f;
				colCount++;
			}

			// For aligning the Hex tiles
			if (offset == 0) {
				colPlace = 0.7f;
				rowPlace += 1.2f;
				offset = 1;
			} else {
				colPlace = 0f;
				rowPlace += 1.2f;
				offset = 0;
			}

			rowCount++;
			colCount = 0;

		}
	}



	void placePlayer() {
		//select a random hex and see if the player can be placed
		int placex = Random.Range (0, hexSize-1);
		int placey = Random.Range (0, hexSize-1);
		print (placex);
		print (placey);
		bool placed = false;
		while (!placed) {
			if (HexGrid [placex, placey] != 2) {
				Transform Playership = Instantiate (playershipspawn);
				Playership.GetComponent<PlayerShip> ().Location = new int[2];
				Playership.GetComponent<PlayerShip> ().Location[0] = placex;
				Playership.GetComponent<PlayerShip> ().Location[1] = placey;
				Vector3 xy = TileGrid [placex, placey];
				xy.x -= 0.07f;
				xy.y += 0.03999f;
				Playership.transform.Translate (xy);
				playerObjects [0] = Playership;
				placed = true;
			} else {
				//if not a sea tile then selected another tile
				placex = Random.Range (0, hexSize);
				placey = Random.Range (0, hexSize); 
			}
		}

	}


	void filltileinfo(){
		for (int x = 0; x < hexSize; x++) {
			for (int y = 0; y < hexSize; y++) {
				Hex hex = new Hex ();
				print (hex);
				Tileinfo [x, y] = hex;
				print (Tileinfo [x, y]);
				if (HexGrid [x, y] != 2)
					Tileinfo [x, y].traversable = true;
				Tileinfo [x, y].resources = new GameResource [2];
				if (playerObjects [0].GetComponent<PlayerShip> ().Location[0] == x && playerObjects [0].GetComponent<PlayerShip> ().Location[1] == y)
					Tileinfo [x, y].player = true; //this needs to be changed when the player can have multiply ships
			}
		}


	}

		
		
	
}