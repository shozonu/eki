using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGenerator : MonoBehaviour {
	// Use this for initialization
    public int seed;
    public Hashtable map;
    public int numTiles;
    public Vector2 nextCoord;
    public float tileSpacing;
	void Start () {
        Random.InitState(seed);
        map = new Hashtable();
        numTiles = map.Count;
        nextCoord = new Vector2(0,0);
        tileSpacing = 10.0F;
	}

	// Update is called once per frame
	void Update () {
        numTiles = map.Count;
        if(Input.GetKeyDown(KeyCode.G)) {
            if(!map.Contains(nextCoord)) {
                GameObject tile = new GameObject();
                tile.AddComponent<Tile>().init(nextCoord, tileSpacing);
                tile.name = tile.GetComponent<Tile>().coord.ToString();
                map.Add(tile.GetComponent<Tile>().coord, tile);
                nextCoord = getAdjacentEmptyCoord(nextCoord);
            }
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            foreach(DictionaryEntry entry in map) {
                print(entry.Key);
            }
        }
	}
    Vector2 getAdjacentEmptyCoord(Vector2 arg) {
        Hashtable coords = new Hashtable();
        if(!map.Contains(arg + Vector2.up))
            coords.Add(0, arg + Vector2.up);
        if(!map.Contains(arg + Vector2.right))
            coords.Add(1, arg + Vector2.right);
        if(!map.Contains(arg + Vector2.down))
            coords.Add(2, arg + Vector2.down);
        if(!map.Contains(arg + Vector2.left))
            coords.Add(3, arg + Vector2.left);
        if(coords.Count <= 0) {
            //return (Vector2) null;
        }
        DictionaryEntry[] coord = new DictionaryEntry[coords.Count];
        coords.CopyTo(coord, 0);
        //string s = "";
        // foreach(DictionaryEntry en in coord) {
        //     s += en.Key.ToString() + " " + en.Value.ToString();
        // }
        // print(s);
        return (Vector2)coord[Random.Range(0,coords.Count)].Value;
    }
}
