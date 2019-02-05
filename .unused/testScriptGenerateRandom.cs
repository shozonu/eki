/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScriptGenerateRandom : MonoBehaviour {
	public int maxTiles;
	public int seed;
	public GameObject squareOpen10;
	public GameObject squareHall10z;
	public GameObject squareHall10x;

	int lastDirection;
	GameObject[] tileList;
	int tileListIndex;
	// Use this for initialization
	void Start () {
		lastDirection = 0;
		tileListIndex = 0;
		tileList = new GameObject[maxTiles];
		Random.InitState(seed);
		//create initial tile
		GameObject tile = Instantiate(squareOpen10);
		tile.AddComponent<Node>();
		tile.GetComponent<Node>().setType(NodeType.SquareOpen10);
		tile.transform.position = new Vector3(0,0,0);
		tileList[tileListIndex] = tile;
		tileListIndex++;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			if(tileListIndex < maxTiles) {
				CreateTileAtTile(tileList[tileListIndex - 1]);
			}
		}
	}

	void CreateTileAtTile(GameObject t) {
		GameObject tile;
		//get position of transition subnode
		Vector3 origin = transition.transform.position;
		Vector3 dir;
		int num;
		do {
			num = Random.range[1,4];
		}while(lastDirection == num);
		switch (num) {
			case 1:
				tile = Instantiate(squareHall10z);
				tile.GetComponent<Node>().setType(NodeType.SquareHall10z);
				tile.GetComponent<Node>().setNodeWidth(10F);
				dir = new Vector3(0.5F * tile.GetComponent<Node>().getNodeWidth(),0,0);
				tile.transform.position = origin + dir;
				break;
			case 2:
				tile = Instantiate(squareHall10z);
				tile.GetComponent<Node>().setType(NodeType.SquareHall10z);
				tile.GetComponent<Node>().setNodeWidth(10F);
				dir = new Vector3(-0.5F * tile.GetComponent<Node>().getNodeWidth(),0,0);
				tile.transform.position = origin + dir;
				break;
			case 3:
				tile = Instantiate(squareHall10x);
				tile.GetComponent<Node>().setType(NodeType.SquareHall10x);
				tile.GetComponent<Node>().setNodeWidth(10F);
				dir = new Vector3(0,0,0.5F * tile.GetComponent<Node>().getNodeWidth());
				tile.transform.position = origin + dir;
				break;
			case 4:
				tile = Instantiate(squareHall10x);
				tile.GetComponent<Node>().setType(NodeType.SquareHall10x);
				tile.GetComponent<Node>().setNodeWidth(10F);
				dir = new Vector3(0,0,0.5F * tile.GetComponent<Node>().getNodeWidth());
				tile.transform.position = origin + dir;
				break;
			default:
				tile = Instantiate(squareOpen10);
				tile.GetComponent<Node>().setType(NodeType.SquareOpen10);
				tile.GetComponent<Node>().setNodeWidth(10F);
				dir = new Vector3(0.5F * tile.GetComponent<Node>().getNodeWidth(),0,0);
				tile.transform.position = origin + dir;
				break;
		}
		tileList[tileListIndex] = tile;
		tileListIndex++;
	}
}
*/
