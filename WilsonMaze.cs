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
    public HashSet<Vector2Int> mazeCoords; //for checking occupancy of vectors
    public HashSet<WilsonCell> maze; //keeping references of objects
    public HashSet<Vector2Int> notMaze; //for checking occupancy of non-used vecs
    public HashSet<Vector2Int> trail; //for checking occupancy of trail vectors
    public List<Vector2Int> orderedTrail; //for keeping order of trail vectors
    public Vector2Int trailHead; //keep track of current vector in step
    public Vector2Int trailHeadPrev; //keep track of last vector in step
    public List<Vector2Int> moves; //list of valid adjacent vectors in step
    float maxCells;
    //when adding a trail to the maze,
    //use notMaze.ExceptWith(trail) to remove walked cells from notMaze
    //use maze.UnionWith(trail) add trail to maze
    //then trail.Clear()

    void Start() {
        gameObject.BroadcastMessage("init"); //ensure MazeViewer objects are initialized
        maze = new HashSet<WilsonCell>();
        mazeCoords = new HashSet<Vector2Int>();
        notMaze = new HashSet<Vector2Int>();
        trail = new HashSet<Vector2Int>();
        orderedTrail = new List<Vector2Int>();
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
            maze.Add(new WilsonCell(initial));
            mazeCoords.Add(initial);
            notMaze.Remove(initial);
            //update UI representation of maze
        gameObject.BroadcastMessage("RefreshMaze");
        }
    }

    void Update() {
        int mazeCount = maze.Count;
        //stops auto maze generation every time maze is expanded (if option is enabled)
        if((pauseAtSuccessfulTrail == true) && (stopAuto == true) && (IsInvoking("AutoStep"))) {
            CancelInvoke();
        }
        //automatically stops generation when number of cells in maze reach certain density
        if(IsInvoking("AutoStep") && ((mazeCount / maxCells)>= cellDensity)) {
            stopAuto = true;
            CancelInvoke();
            print("Minimum Cell Density Reached: " + mazeCount + " cells ("
            + (100*(mazeCount / maxCells)) + "%)");
        }
        //'G' for 'Generate'
        if(Input.GetKeyDown(KeyCode.G)) {
            //if already at desired density, do not generate any further
            if(mazeCount >= (maxCells * cellDensity)) {
                print("Minimum Cell Density Reached: " + mazeCount + " cells ("
                + (100*(mazeCount / maxCells)) + "%)");
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
        int trailCount = trail.Count;
        int notMazeCount = notMaze.Count;
        Vector2Int up = Vector2Int.up;
        Vector2Int down = Vector2Int.down;
        Vector2Int right = Vector2Int.right;
        Vector2Int left = Vector2Int.left;
        if((trailCount < 1)) {
            Vector2Int [] arr = new Vector2Int[notMazeCount];
            notMaze.CopyTo(arr);
            trailHead = arr[Random.Range(0, arr.Length - 1)];
            trailHeadPrev = trailHead;
            trail.Add(trailHead);
            orderedTrail.Add(trailHead);
            if(debugLog) {
                print("First step in new trail.");
                print(trailHead);
            }
            //calculate next set of moves.
            //each move is the Vector2Int of a valid cell adjacent to the head.
            //potential move is checked against maze bounds,
            //and then checked if not the immediate previously walked cell.
            int headY = trailHead.y;
            int headX = trailHead.x;
            moves.Clear();
            if(!(headY + 1 > sizeY - 1)) {
                if(!(trailHead + up == trailHeadPrev)) {
                    moves.Add(trailHead + up);
                }
            }
            if(!(headY - 1 < 0)) {
                if(!(trailHead + down == trailHeadPrev)) {
                    moves.Add(trailHead + down);
                }
            }
            if(!(headX + 1 > sizeX - 1)) {
                if(!(trailHead + right == trailHeadPrev)) {
                    moves.Add(trailHead + right);
                }
            }
            if(!(headX - 1 < 0)) {
                if(!(trailHead + left == trailHeadPrev)) {
                    moves.Add(trailHead + left);
                }
            }
            return;
        }
        //if not the first step
        else {
            //update trailHeadPrev,
            //and select new trailHead from list of valid moves
            trailHeadPrev = trailHead;
            trailHead = moves[Random.Range(0,moves.Count)];
            int headY = trailHead.y;
            int headX = trailHead.x;
            //if new head is part of current trail,
            //erase the trail following the overlap
            if(trail.Contains(trailHead)) {
                int cutoff = 0;
                for(int i = 0; i < trailCount; i++) {
                    //find index of overlapping cell and store in cutoff,
                    //update trailHeadPrev to cell behind overlap
                    if(orderedTrail[i] == trailHead) {
                        cutoff = i;
                        if(i > 0) {
                            trailHeadPrev = orderedTrail[i - 1];
                        }
                        else {
                            trailHeadPrev = orderedTrail[i];
                        }
                        break;
                    }
                }
                for(int i = cutoff + 1; i < trailCount; i++) {
                    trail.Remove(orderedTrail[i]);
                }
                //second argument represents number of elements to remove after cutoff
                orderedTrail.RemoveRange(cutoff, trailCount - cutoff);

                if(debugLog) print(trailHead.ToString() + " Loop Erased.");
                stopAuto = false;
            }
            //if new head is part of existing maze, add trail to maze
            else if(mazeCoords.Contains(trailHead)) {
                notMaze.ExceptWith(trail); //remove vectors from notMaze
                mazeCoords.UnionWith(trail); //add trail to mazeCoords
                WilsonCell prevCell = new WilsonCell(orderedTrail[0]);
                //add cells to maze from trail, and forward-link cells
                for(int i = 0; i < trailCount; i++) {
                    if(i > 0) {
                        //constructor sets new cell as prevCell's next
                        WilsonCell tempCell = new WilsonCell(orderedTrail[i], prevCell);
                        maze.Add(tempCell);
                        prevCell = tempCell;
                        //link last cell in trail to maze
                        if(i == trailCount - 1) {
                            foreach(WilsonCell mc in maze) {
                                if(mc.vec == trailHead) {
                                    tempCell.next = mc;
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        //first cell in trail doesn't have a next
                        maze.Add(prevCell);
                        if(trailCount == 1) {
                            foreach(WilsonCell mc in maze) {
                                if(mc.vec == trailHead) {
                                    prevCell.next = mc;
                                    break;
                                }
                            }
                        }
                    }
                }
                trail.Clear();
                orderedTrail.Clear();
                if(debugLog) print(trailHead.ToString() + " Trail added to maze.");
                gameObject.BroadcastMessage("RefreshMaze");
                gameObject.BroadcastMessage("RefreshTrack");
                gameObject.BroadcastMessage("RefreshTrail");
                gameObject.BroadcastMessage("RefreshMoves");
                stopAuto = true;
                return;
            }
            //calculate next set of move
            moves.Clear();
            if(!(headY + 1 > sizeY - 1)&&(trailHead + up != trailHeadPrev)) {
                moves.Add(trailHead + up);
            }
            if(!(headY - 1 < 0)&&(trailHead + down != trailHeadPrev)) {
                moves.Add(trailHead + down);
            }
            if(!(headX + 1 > sizeX - 1)&&(trailHead + right != trailHeadPrev)) {
                moves.Add(trailHead + right);
            }
            if(!(headX - 1 < 0)&&(trailHead + left != trailHeadPrev)) {
                moves.Add(trailHead + left);
            }
            if(debugLog) print(trailHead);
            trail.Add(trailHead);
            orderedTrail.Add(trailHead);
        }
        //update UI representation of trail and move indicators
        gameObject.BroadcastMessage("RefreshTrail");
        gameObject.BroadcastMessage("RefreshMoves");
    }
}

public class WilsonCell {
    public Vector2Int vec;
    public WilsonCell next;

    public WilsonCell(Vector2Int v) {
        vec = v;
    }
    public WilsonCell(Vector2Int v, WilsonCell cell) {
        vec = v;
        cell.next = this;
    }
    public WilsonCell(int x, int y) {
        vec = new Vector2Int(x,y);
    }
    public WilsonCell(int x, int y, WilsonCell cell) {
        vec = new Vector2Int(x,y);
        cell.next = this;
    }
}
