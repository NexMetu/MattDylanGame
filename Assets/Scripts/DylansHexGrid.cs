

using UnityEngine;
using System.Collections;

public class DylansHexGrid : MonoBehaviour {

	public int [,] HexGrid;
	public int hexSize;
	public Transform[] sea_RoughPrefab;
	public Transform[] sea_CalmPrefab;
	public Transform[] grass_DirtPrefab;

	public int numberOfIslands;
	public int minSizeOfIslands;
	public int maxSizeOfIslands;

	public int minSizeOfShallows;
	public int maxSizeOfShallows;

	// Use this for initialization
	void Start () {

		HexGrid = new int [hexSize , hexSize];


		// Fill Hex with integer values that will correspond to map tiles
		FillHex ();

		// Fill Hex with map tiles corresponding to integer values
		PopulateHex ();
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
				}
				if (HexGrid [rowCount, colCount] == 1) {
					Transform sea_RoughTile = Instantiate (sea_CalmPrefab[Random.Range (0, sea_CalmPrefab.Length)]);
					sea_RoughTile.transform.Translate (new Vector2 (colPlace, rowPlace));
				}
				if (HexGrid [rowCount, colCount] == 2) {
					Transform grass_DirtTile = Instantiate (grass_DirtPrefab[Random.Range (0, grass_DirtPrefab.Length)]);
					grass_DirtTile.transform.Translate (new Vector2 (colPlace, rowPlace));
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
}
