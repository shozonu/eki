/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScriptAlpha : MonoBehaviour {
	public GameObject squareOpen10;
	public GameObject squareHall10z;
	GameObject prevTile;
	NodeType currentType;
	Vector3 dir;
	// Use this for initialization
	void Start () {
		dir = new Vector3(0, 0, 1);
		GameObject tile = Instantiate(squareOpen10);
		tile.AddComponent<Node>();
		tile.GetComponent<Node>().setType(NodeType.SquareOpen10);
		tile.transform.position = new Vector3(0,0,0);
		prevTile = tile;
		currentType = NodeType.SquareOpen10;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)) {
			dir = new Vector3(0, 0, -1 * prevTile.GetComponent<Node>().getNodeWidth());
		}
		if(Input.GetKeyDown(KeyCode.W)) {
			dir = new Vector3(0, 0, 1 * prevTile.GetComponent<Node>().getNodeWidth());
		}
		if(Input.GetKeyDown(KeyCode.A)) {
			dir = new Vector3(-1 * prevTile.GetComponent<Node>().getNodeWidth(), 0, 0);
		}
		if(Input.GetKeyDown(KeyCode.D)) {
			dir = new Vector3(1 * prevTile.GetComponent<Node>().getNodeWidth(), 0, 0);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			currentType = NodeType.SquareOpen10;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			currentType = NodeType.SquareHall10z;
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			CreateAdjacentPlane(dir);
		}
	}

	void CreateAdjacentPlane(Vector3 direction) {
		GameObject tile;
		switch (currentType) {
			case NodeType.SquareOpen10:
				tile = Instantiate(squareOpen10);
				tile.GetComponent<Node>().setType(NodeType.SquareOpen10);
				break;
			case NodeType.SquareHall10z:
				tile = Instantiate(squareHall10z);
				tile.GetComponent<Node>().setType(NodeType.SquareHall10z);
				break;
			default:
				tile = Instantiate(squareOpen10);
				tile.GetComponent<Node>().setType(NodeType.SquareOpen10);
				break;
		}

		Vector3 pos = prevTile.transform.position + direction;
		tile.transform.position = pos;
		prevTile = tile;
	}
}
*/
