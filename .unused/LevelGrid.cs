// ﻿using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class LevelGrid : MonoBehaviour {
// 	public int seed;
// 	public int levelWidth;
// 	public int levelHeight;
// 	public int maxTiles;
//
// 	public GameObject squareOpen10;
// 	public GameObject squareHall10z;
// 	public GameObject squareHall10x;
// 	public GameObject squareCorner10ne;
// 	public GameObject squareCorner10nw;
// 	public GameObject squareCorner10se;
// 	public GameObject squareCorner10sw;
//
// 	public GameObject[,] tile;
// 	public GameObject[] tileList;
// 	public int listPos;
//
// 	public TileReference tileHead;
// 	public TileReference tileMid;
// 	public TileReference tileTail;
//
// 	// Use this for initialization
// 	void Start () {
// 		Random.InitState(seed);
// 		int wut = 0;
//
// 		for(int i = 0; i < 32; i++) {
// 			tileList[i] = null;
// 		}
//
// 		tile = new GameObject[levelWidth, levelHeight];
// 		for(int x = 0; x < levelWidth; x++) {
// 			for(int y = 0; y < levelHeight; y++) {
// 				tile[x,y] = null;
// 			}
// 		}
// 		tileHead = new TileReference(); tileHead.init();
// 		tileMid = new TileReference(); tileMid.init();
// 		tileTail = new TileReference(); tileTail.init();
// 	}
//
// 	// Update is called once per frame
// 	void Update () {
// 		if(Input.GetKeyDown(KeyCode.G)) {
//
// 		}
// 	}
// }
//
// public class TileReference : MonoBehaviour {
// 	LevelGrid lGrid;
// 	GameObject tile;
// 	int x;
// 	int y;
//
// 	void Start() {
// 		init();
// 	}
// 	void Update() {
//
// 	}
//
// 	public void init() {
// 		tile = null;
// 		lGrid = GameObject.Find("Map").GetComponent<LevelGrid>();
// 		x = 0;
// 		y = 0;
// 	}
//
// 	public bool isTile() {
// 		if(tile != null) {
// 			return true;
// 		}
// 		else {
// 			return false;
// 		}
// 	}
// 	public void setX(int a) {
// 		x = a;
// 	}
// 	public void setY(int a) {
// 		y = a;
// 	}
// 	public int getX() {
// 		return x;
// 	}
// 	public int getY() {
// 		return y;
// 	}
// 	public GameObject getTileObj() {
// 		return tile;
// 	}
// 	public void copyRef(TileReference tr) {
// 		//followers copy preceding refs, head moves last
// 		x = tr.getX();
// 		y = tr.getY();
// 		tile = tr.getTileObj();
// 	}
// 	public GameObject create(NodeType nt) {
// 		switch(nt) {
// 			case(NodeType.SquareOpen10):
// 				tile = Instantiate(lGrid.squareOpen10); break;
// 			case(NodeType.SquareHall10x):
// 				tile = Instantiate(lGrid.squareHall10x); break;
// 			case(NodeType.SquareHall10z):
// 				tile = Instantiate(lGrid.squareHall10z); break;
// 			case(NodeType.SquareCorner10ne):
// 				tile = Instantiate(lGrid.squareCorner10ne); break;
// 			case(NodeType.SquareCorner10nw):
// 				tile = Instantiate(lGrid.squareCorner10nw); break;
// 			case(NodeType.SquareCorner10se):
// 				tile = Instantiate(lGrid.squareCorner10se); break;
// 			case(NodeType.SquareCorner10sw):
// 				tile = Instantiate(lGrid.squareCorner10sw); break;
// 			default:
// 				tile = Instantiate(lGrid.squareOpen10); break;
// 		}
// 		tile.transform.position = new Vector3(x * 10, 0, y * 10);
// 		lGrid.tile[x,y] = tile;
// 		lGrid.tileList[lGrid.listPos++] = tile;
// 		return tile;
// 	}
// }
