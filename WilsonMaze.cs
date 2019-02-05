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
        gameObject.BroadcastMessage("init"); //ensure MazeViewer objects are initialized
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
        //instantiate UI representations of empty cells
        gameObject.BroadcastMessage("RefreshEmpty");

        //initialize maze with one cell
        if(maze.Count < 1) {
            Vector2Int initial =
                new Vector2Int(Random.Range(0,sizeX - 1), Random.Range(0,sizeY - 1));
            maze.Add(initial);
            notMaze.Remove(initial);
            //update UI representation of maze
        gameObject.BroadcastMessage("RefreshMaze");
        }
    }

    void Update() {
        //stops auto maze generation every time maze is expanded (if option is enabled)
        if((pauseAtSuccessfulTrail == true) && (stopAuto == true) && (IsInvoking("AutoStep"))) {
            CancelInvoke();
        }
        //automatically stops generation when number of cells in maze reach certain density
        if(IsInvoking("AutoStep") && ((maze.Count / maxCells)>= cellDensity)) {
            stopAuto = true;
            CancelInvoke();
            print("Minimum Cell Density Reached: " + maze.Count + " cells ("
            + (100*(maze.Count / maxCells)) + "%)");
        }
        //'G' for 'Generate'
        if(Input.GetKeyDown(KeyCode.G)) {
            //if already at desired density, do not generate any further
            if(maze.Count >= (maxCells * cellDensity)) {
                print("Minimum Cell Density Reached: " + maze.Count + " cells ("
                + (100*(maze.Count / maxCells)) + "%)");
            }
            //stops auto generation if it is currently going
            else if(IsInvoking("AutoStep")) {
                CancelInvoke();
                stopAuto = true;
                print("Autostep Paused");
            }
            //starts auto generation if currently stopped
            else {
                InvokeRepeating("AutoStep", autoStepInterval, autoStepInterval);
                stopAuto = false;
                print("Autostep Resumed");
            }
        }
        if(Input.GetKeyDown(KeyCode.I)) {
            print("# of Empty Cells: " + notMaze.Count);
        }
    }

    void AutoStep() {
        //step function wrapped in AutoStep,
        //in case additional functions need to be called
        WilsonStep();
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
            //calculate next set of moves.
            //each move is the Vector2Int of a valid cell adjacent to the head.
            //potential move is checked against maze bounds,
            //and then checked if not the immediate previously walked cell.
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
        //if not the first step
        else {
            //update trailHeadPrev,
            //and select new trailHead from list of valid moves
            trailHeadPrev = trailHead;
            trailHead = moves[Random.Range(0,moves.Count)];
            //if new head is part of current trail, erase the trail
            //TO DO: change so that it only erased trail up to point where loop was created
            if(trail.Contains(trailHead)) {
                trail.Clear();
                if(debugLog) print(trailHead.ToString() + " Loop Erased.");
                stopAuto = false;
                return;
            }
            //if new head is part of existing maze, add trail to maze
            else if(maze.Contains(trailHead)) {
                notMaze.ExceptWith(trail);
                maze.UnionWith(trail);
                trail.Clear();
                if(debugLog) print(trailHead.ToString() + " Trail added to maze.");
                gameObject.BroadcastMessage("RefreshMaze");
                stopAuto = true;
                return;
            }
            //calculate next set of move
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
        //update UI representation of trail and move indicators
        gameObject.BroadcastMessage("RefreshTrail");
        gameObject.BroadcastMessage("RefreshMoves");
    }
}
