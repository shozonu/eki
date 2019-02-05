/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
	public int seed;
	public int levelWidth;
	public int levelHeight;
	public int maxTiles;

	public GameObject squareOpen10;
	public GameObject squareHall10z;
	public GameObject squareHall10x;
	public GameObject squareCorner10ne;
	public GameObject squareCorner10nw;
	public GameObject squareCorner10se;
	public GameObject squareCorner10sw;

	GameObject[,] tile;
	GameObject[] tileList;
	GameObject startingTile;
	GameObject endingTile;

	int listPos;

	// Use this for initialization
	void Start () {
		Random.InitState(seed);
		tile = new GameObject[levelWidth, levelHeight];
		for(int x = 0; x < levelWidth; x++) {
			for(int y = 0; y < levelHeight; y++) {
				tile[x,y] = null;
			}
		}
		startingTile = Instantiate(squareOpen10);
		startingTile.transform.position = new Vector3(0,0,0);
		tileList = new GameObject[maxTiles];
		listPos = 0;
		tileList[listPos] = startingTile;
		startingTile.GetComponent<Node>().setNodeIndex(listPos);
		listPos++;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.G)) {
			tileList[listPos] = generateTileFrom(tileList[listPos - 1]);
			listPos++;
		}
	}

	public GameObject generateTileFrom(GameObject prev) {
		//print("generating tile from " + prev);
		int newTileCardinal = 0;
		newTileCardinal = returnAdjacentTileIndex(prev);
		print("Cardinal of new tile is " + newTileCardinal);
		NodeType genType = determineNodeType(prev, newTileCardinal);
		GameObject genTile = createTile(genType);
		//vec and dir to hold temporary Vector3 for calculating final position
		Vector3 vec = prev.transform.position;
		Vector3 dir = new Vector3(0,0,0);
		float width = prev.GetComponent<Node>().getNodeWidth() + genTile.GetComponent<Node>().getNodeWidth();
		if(newTileCardinal == (7 | 8 | 9)) {
			//multiply by half width in order to get correct spacing distance
			dir += new Vector3(0,0,0.5F * width);
		}
		if(newTileCardinal == (1 | 2 | 3)) {
			dir += new Vector3(0,0,-0.5F * width);
		}
		if(newTileCardinal == (1 | 4 | 7)) {
			dir += new Vector3(-0.5F * width,0,0);
		}
		if(newTileCardinal == (3 | 6 | 9)) {
			dir += new Vector3(0.5F * width,0,0);
		}
		//modify new tile space by dir
		print("Last Tile's position is " + vec + ", new tile differs " + dir);
		genTile.transform.position = vec + dir;
		genTile.GetComponent<Node>().setNodeParent(prev);
		prev.GetComponent<Node>().adjNode[newTileCardinal] = genTile;
		return genTile;
	}

	public GameObject createTile(NodeType ntype) {
		GameObject retTile = null;
		switch(ntype) {
			case (NodeType.SquareOpen10):
				retTile = Instantiate(squareOpen10);
				break;
			case (NodeType.SquareHall10x):
				retTile = Instantiate(squareHall10x);
				break;
			case (NodeType.SquareHall10z):
				retTile = Instantiate(squareHall10z);
				break;
			case (NodeType.SquareCorner10ne):
				retTile = Instantiate(squareCorner10ne);
				break;
			case (NodeType.SquareCorner10nw):
				retTile = Instantiate(squareCorner10nw);
				break;
			case (NodeType.SquareCorner10se):
				retTile = Instantiate(squareCorner10se);
				break;
			case (NodeType.SquareCorner10sw):
				retTile = Instantiate(squareCorner10sw);
				break;
		}
		retTile.GetComponent<Node>().setNodeIndex(listPos);
		//print(ntype + " created at list position " + listPos);
		return retTile;
	}

	int returnAdjacentTileIndex(GameObject baseTile) {
		int numNotOccupied = 0;
		int[] validCardinals;
		//first pass to determine number of unoccupied adjacent tiles
		//array index 0 is not to be used; array is inflated to align index with cardinals
		validCardinals = new int[8];
		for(int i = 1; i < 9; i++) {
			if(baseTile.GetComponent<Node>().adjNode[i] == null) {
				//increment counter if not null
				validCardinals[numNotOccupied] = i;
				numNotOccupied++;
			}
		}
		//return random value from validCardinals array
		if(numNotOccupied == 0) {
			return -1;
		}
		else {
			print("numNotOccupied = " + numNotOccupied);
			return validCardinals[Random.Range(0, numNotOccupied)];
		}
	}

	public NodeType determineNodeType(GameObject A, int cardinal) {
		//check B's postion in relation to A and determine what node type
		//cardinal is the integer representing the position around A that B will occupy
		NodeType typeA = A.GetComponent<Node>().getType();
		NodeType typeB = NodeType.SquareOpen10;
		NodeType newA = NodeType.SquareOpen10;
		bool changeA = false;
		//check A's type
		print("Previous nodeType is " + typeA);
		switch(typeA) {
			case NodeType.SquareOpen10:
				if(cardinal % 2 == 0) {
					//even cardinals are N, E, S, W
					if(cardinal == 2 || cardinal == 8) {
						typeB = NodeType.SquareHall10z;
					}
					else if(cardinal == 4 || cardinal == 6) {
						typeB = NodeType.SquareHall10x;
					}
				}
				break;
			case NodeType.SquareHall10z:
				print("not a SquareOpen10");
				if(cardinal % 2 == 0) {
					//even cardinals are N, E, S,W
					if(cardinal == 2 || cardinal == 8) {
						typeB = NodeType.SquareHall10z;
					}
					else if(cardinal == 4 || cardinal == 6) {
						//if B doesn't align with current hall, make A into a corner
						typeB = NodeType.SquareHall10x;
						changeA = true;
						if(cardinal == 4) {
							//if B is on west side
							//check whether to make A north corner or south corner depending on vacancy of north or south
							if(A.GetComponent<Node>().adjNode[8] == null) {
								newA = NodeType.SquareCorner10ne;
							}
							else {
								newA = NodeType.SquareCorner10se;
							}
						}
						if(cardinal == 6) {
							newA = NodeType.SquareCorner10nw;
							if(A.GetComponent<Node>().adjNode[8] == null) {
								newA = NodeType.SquareCorner10nw;
							}
							else {
								newA = NodeType.SquareCorner10sw;
							}
						}
					}
				}
				break;
			case NodeType.SquareHall10x:
				print("not a SquareOpen10");
				if(cardinal % 2 == 0) {
					//even cardinals are N, E, S,W
					if(cardinal == 4 || cardinal == 6) {
						typeB = NodeType.SquareHall10x;
					}
					else if(cardinal == 2 || cardinal == 8) {
						typeB = NodeType.SquareHall10z;
						changeA = true;
						if(cardinal == 2) {
							if(A.GetComponent<Node>().adjNode[4] == null) {
								newA = NodeType.SquareCorner10nw;
							}
							else {
								newA = NodeType.SquareCorner10ne;
							}
						}
						if(cardinal == 8) {
							if(A.GetComponent<Node>().adjNode[4] == null) {
								newA = NodeType.SquareCorner10sw;
							}
							else {
								newA = NodeType.SquareCorner10se;
							}
						}
					}
				}
				break;
		}
		print("New tile type will be " + typeB);
		if(changeA == true) {
			//if A needs its node type changed, change existing tile to new type,
			//or delete and instantiate new correct type
			print("Changing previous tile to " + newA);
			A.GetComponent<Node>().setType(newA);
			Node.reload(A);
		}
		return typeB;
	}
}
*/
