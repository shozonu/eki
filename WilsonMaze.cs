using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilsonMaze : MonoBehaviour {
    public bool debugLog;
    public bool pauseAtSuccessfulTrail;
    public int seed;
    public int sizeX;
    public int sizeY;
    public float cellDensity;
    public float autoStepInterval;
    public bool stopAuto;
    public HashSet<Vector2Int> maze;
    public HashSet<Vector2Int> trail;
    public HashSet<Vector2Int> notMaze;
    public Vector2Int trailHead;
    public Vector2Int trailHeadPrev;
    public List<Vector2Int> moves;
    float maxCells;
    //when adding a trail to the maze,
    //use notMaze.ExceptWith(trail) to remove walked cells from notMaze
    //use maze.UnionWith(trail) add trail to maze
    //then trail.Clear()

    void Start() {
        gameObject.BroadcastMessage("init");
        maze = new HashSet<Vector2Int>();
        notMaze = new HashSet<Vector2Int>();
        trail = new HashSet<Vector2Int>();
        moves = new List<Vector2Int>();
        Random.InitState(seed);
        stopAuto = false;
        maxCells = sizeX * sizeY;
        //populate notMaze with all cells
        for(int x = 0; x < sizeX; x++) {
            for(int y = 0; y < sizeY; y++) {
                notMaze.Add(new Vector2Int(x,y));
            }
        }
        gameObject.BroadcastMessage("RefreshEmpty");

        //initialize maze with one cell
        if(maze.Count < 1) {
            Vector2Int initial =
                new Vector2Int(Random.Range(0,sizeX - 1), Random.Range(0,sizeY - 1));
            maze.Add(initial);
            notMaze.Remove(initial);
        gameObject.BroadcastMessage("RefreshMaze");
        }
    }

    void Update() {
        if((pauseAtSuccessfulTrail == true) && (stopAuto == true) && (IsInvoking("AutoStep"))) {
            CancelInvoke();
        }
        if(IsInvoking("AutoStep") && ((maze.Count / maxCells)>= cellDensity)) {
            stopAuto = true;
            CancelInvoke();
            print("Minimum Cell Density Reached: " + maze.Count + " cells ("
            + (100*(maze.Count / maxCells)) + "%)");
        }
        if(Input.GetKeyDown(KeyCode.G)) {
            if(maze.Count >= (maxCells * cellDensity)) {
                print("Minimum Cell Density Reached: " + maze.Count + " cells ("
                + (100*(maze.Count / maxCells)) + "%)");
            }
            else if(IsInvoking("AutoStep")) {
                CancelInvoke();
                stopAuto = true;
                print("Autostep Paused");
            }
            else {
                InvokeRepeating("AutoStep", autoStepInterval, autoStepInterval);
                stopAuto = false;
                print("Autostep Resumed");
            }
            // WilsonStep();
            // gameObject.BroadcastMessage("RefreshTrail");
            // gameObject.BroadcastMessage("RefreshMaze");
            // gameObject.BroadcastMessage("RefreshMoves");
        }
        if(Input.GetKeyDown(KeyCode.I)) {
            print("# of Empty Cells: " + notMaze.Count);
        }
    }

    void AutoStep() {
        WilsonStep();
        // gameObject.BroadcastMessage("RefreshTrail");
        // gameObject.BroadcastMessage("RefreshMaze");
        // gameObject.BroadcastMessage("RefreshMoves");
    }
    void WilsonStep() {
        //if this is the first step in the trail, choose random empty start cell
        if((trail.Count < 1)) {
            Vector2Int [] arr = new Vector2Int[notMaze.Count];
            notMaze.CopyTo(arr);
            trailHead = arr[Random.Range(0, arr.Length - 1)];
            trailHeadPrev = trailHead;
            trail.Add(trailHead);
            if(debugLog) {
                print("First step in new trail.");
                print(trailHead);
            }
            moves.Clear();
            if(!(trailHead.y + 1 > sizeY - 1)) {
                if(!(trailHead + Vector2Int.up == trailHeadPrev)) {
                    moves.Add(trailHead + Vector2Int.up);
                }
            }
            if(!(trailHead.y - 1 < 0)) {
                if(!(trailHead + Vector2Int.down == trailHeadPrev)) {
                    moves.Add(trailHead + Vector2Int.down);
                }
            }
            if(!(trailHead.x + 1 > sizeX - 1)) {
                if(!(trailHead + Vector2Int.right == trailHeadPrev)) {
                    moves.Add(trailHead + Vector2Int.right);
                }
            }
            if(!(trailHead.x - 1 < 0)) {
                if(!(trailHead + Vector2Int.left == trailHeadPrev)) {
                    moves.Add(trailHead + Vector2Int.left);
                }
            }
        }
        else {
            trailHeadPrev = trailHead;
            trailHead = moves[Random.Range(0,moves.Count)];
            if(trail.Contains(trailHead)) {
                //trailHead = null;
                trail.Clear();
                if(debugLog) print(trailHead.ToString() + " Loop Erased.");
                stopAuto = false;
                return;
            }
            else if(maze.Contains(trailHead)) {
                notMaze.ExceptWith(trail);
                maze.UnionWith(trail);
                trail.Clear();
                //trailHead = null;
                if(debugLog) print(trailHead.ToString() + " Trail added to maze.");
                gameObject.BroadcastMessage("RefreshMaze");
                stopAuto = true;
                return;
            }
            //calculate move
            moves.Clear();
            if(!(trailHead.y + 1 > sizeY - 1)&&(trailHead + Vector2Int.up != trailHeadPrev)) {
                moves.Add(trailHead + Vector2Int.up);
            }
            if(!(trailHead.y - 1 < 0)&&(trailHead + Vector2Int.down != trailHeadPrev)) {
                moves.Add(trailHead + Vector2Int.down);
            }
            if(!(trailHead.x + 1 > sizeX - 1)&&(trailHead + Vector2Int.right != trailHeadPrev)) {
                moves.Add(trailHead + Vector2Int.right);
            }
            if(!(trailHead.x - 1 < 0)&&(trailHead + Vector2Int.left != trailHeadPrev)) {
                moves.Add(trailHead + Vector2Int.left);
            }
            if(debugLog) print(trailHead);
            trail.Add(trailHead);
        }
        gameObject.BroadcastMessage("RefreshTrail");
        gameObject.BroadcastMessage("RefreshMoves");
        return;
    }
}
