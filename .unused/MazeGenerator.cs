using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
    public int seed;
    public int sizeX;
    public int sizeY;
    public int minimumCells; //no larger than product of sizeX and sizeY
    public Hashtable maze;
    public List<Vector2Int> emptyCells;
    public List<int> pathLength;
    //remove gameObject-releated elements to separate component later?
    public Dictionary<Vector2, GameObject> map;
    //values in list represent length of the path at the index

	void Start() {
        Random.InitState(seed);
        map = new Dictionary<Vector2, GameObject>();
        maze = new Hashtable(sizeX * sizeY);
        pathLength = new List<int>();
        //populate emptyCells
        emptyCells = new List<Vector2Int>();
        for(int x = 1; x < sizeX; x++) {
            for(int y = 1; y < sizeY; y++) {
                emptyCells.Add(new Vector2Int(x, y));
            }
        }
	}

	void Update() {
        // if(Input.GetKeyDown(KeyCode.G)) {
        //     generatePath();
        // }
	}

    void generatePath() {
        int pathIndex = pathLength.Count; //paths identified by their index
        Vector2Int currentCell;
        List<int> trail = new List<int>();
        if(maze.Count < 1) { //if maze is empty
            //add random cell to maze
            int first = Random.Range(0, emptyCells.Count - 1);
            maze.Add(emptyCells[first], new MazeCell(emptyCells[first], pathIndex));
            //since cell is added to maze, remove from emptyCells
            emptyCells.RemoveAt(first);
        }
        //choose random empty cell to start random walk
        int randomIndex = Random.Range(0, emptyCells.Count - 1);
        trail.Add(randomIndex);
        currentCell = emptyCells[randomIndex];
        do { //choose random adjacent cell
            List<Vector2Int> validMove = new List<Vector2Int> {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };
            //up, down, left, right;
            //invalid if such move is out of bounds
            //or if move is last walked cell.
            for(int i = 3; i > 0; i--) {
                Vector2Int vec = currentCell + validMove[i];
                if(vec == emptyCells[trail[trail.Count - 1]]) {
                    //if vec is last walked cell, remove from validMove
                    validMove.RemoveAt(i);
                }
                else if((vec.x < 1) || (vec.x > sizeX)) {
                    //if vec.x is out of bounds, remove from validMove
                    validMove.RemoveAt(i);
                }
                else if((vec.y < 1) || (vec.y > sizeY)) {
                    //if vec.y is out of bounds, remove from validMove
                    validMove.RemoveAt(i);
                }
            }
            currentCell += validMove[Random.Range(0, validMove.Count - 1)];
            for(int k = 0; k < trail.Count; k++) {
                //check if current cell is already part of current path
                if(currentCell == emptyCells[trail[k]]) {
                    //if current cell is part of own trail,
                    //remove trail between current and this original trail.
                    if(trail.Count > 1) {
                        trail.RemoveRange(k + 1, trail.Count - 1);
                    }
                    else {
                        //edge case?
                    }
                }
            }
            //loop until currentCell equivalent exists in maze
        } while(!maze.ContainsKey(currentCell));

        //add each cell specified by trail[i] to maze.
        MazeCell prev = null;
        for(int i = 0; i < trail.Count; i++) {
            MazeCell curr = new MazeCell(emptyCells[trail[i]], pathIndex);
            if(prev != null) {
                //link cells with tail and head references
                prev.next = curr;
                curr.addTail(prev);
            }
            maze.Add(curr.pos, curr);
            prev = curr;
            //instantiate here
            GameObject tile = new GameObject();
            Vector2 temp = curr.pos;
            tile.AddComponent<Tile>().init(temp, 10.0F);
            tile.name = tile.GetComponent<Tile>().coord.ToString();
            map.Add(tile.GetComponent<Tile>().coord, tile);
        }
        //currentCell should already be in the maze,
        //so just add prev as a tail.
        (maze[currentCell] as MazeCell).addTail(prev);
        //add length of this path to list
        pathLength.Add(trail.Count);
    }
}

public class MazeCell {
    public Vector2Int pos;
    public int pathNumber;
    public List<MazeCell> tail;
    public MazeCell next;
    public MazeCell(Vector2Int vec, int num) {
        pos = vec;
        pathNumber = num;
        tail = null;
        next = null;
    }
    public void addTail(MazeCell cell) {
        if(tail == null) {
            tail = new List<MazeCell>();
            tail.Add(cell);
        }
        else {
            tail.Add(cell);
        }
    }
    public void setNext(MazeCell cell) {
        next = cell;
    }
}
