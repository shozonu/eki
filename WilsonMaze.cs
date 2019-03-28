using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilsonMaze : MonoBehaviour {
    public MazeViewer viewer;
    public bool silentMode;
    public int seed;
    public int sizeX, sizeY;
    public float cellDensity;
    public int maxIterations;
    public float autoStepInterval;
    public bool pauseAtSuccessfulTrail;
    public bool initFlag, debugLog;
    public HashSet<Vector2Int> mazeCoords; //for checking occupancy of vectors
    public HashSet<WilsonCell> maze; //keeping references of objects
    public HashSet<Vector2Int> notMaze; //for checking occupancy of non-used vecs
    public HashSet<Vector2Int> trail; //for checking occupancy of trail vectors
    public List<Vector2Int> orderedTrail; //for keeping order of trail vectors
    public Vector2Int trailHead; //keep track of current vector in step
    public Vector2Int trailHeadPrev; //keep track of last vector in step
    public List<Vector2Int> moves; //list of valid adjacent vectors in step
    float maxCells;
    int mazeCount;
    int targetCells;
    int iterations;

    void Start() {
        if(!initFlag) {
            init();
        }
    }

    public void init() {
        // Ensure MazeViewer object is initialized;
        // Populate notMaze with vectors of all cells.
        viewer = gameObject.GetComponent<MazeViewer>();
        if(!viewer.initFlag) viewer.init();

        maze = new HashSet<WilsonCell>();
        mazeCoords = new HashSet<Vector2Int>();
        notMaze = new HashSet<Vector2Int>();
        trail = new HashSet<Vector2Int>();
        orderedTrail = new List<Vector2Int>();
        moves = new List<Vector2Int>();
        Random.InitState(seed);

        mazeCount = maze.Count;
        iterations = 0;
        maxCells = sizeX * sizeY;
        targetCells = (int)(maxCells * cellDensity);

        for(int x = 0; x < sizeX; x++) {
            for(int y = 0; y < sizeY; y++) {
                notMaze.Add(new Vector2Int(x,y));
            }
        }
        //Initialize maze.
        if(mazeCount < 1) {
            Vector2Int initial =
                new Vector2Int(Random.Range(0,sizeX - 1), Random.Range(0,sizeY - 1));
            maze.Add(new WilsonCell(initial));
            mazeCoords.Add(initial);
            notMaze.Remove(initial);
        }
        viewer.RefreshEmpty();
        viewer.RefreshMaze();
        initFlag = true;
    }

    void Update() {
        mazeCount = maze.Count;
        // Automatically stops generation when number of cells in maze reach certain number.
        if(IsInvoking("WilsonStep") && (mazeCount >= targetCells)) {
            CancelInvoke();
            print("Minimum Cell Density Reached: " + mazeCount + " cells ("
            + (100*(mazeCount / maxCells)) + "%)");
        }
        if(Input.GetKeyDown(KeyCode.G)) {
            if(iterations >= maxIterations) {
                print("Maximum Iterations Reached: " + iterations + "/" + maxIterations);
            }
            // Do not generate if cell density reached.
            else if(mazeCount >= targetCells) {
                print("Minimum Cell Density Reached: " + mazeCount + " cells ("
                + (100*(mazeCount / maxCells)) + "%)");
            }
            else if(silentMode) {
                SilentStep();
            }
            else {
                // Stops generation if it is currently active.
                if(IsInvoking("WilsonStep")) {
                    CancelInvoke();
                    print("Generation Paused");
                }
                // Starts generation if currently stopped.
                else {
                    InvokeRepeating("WilsonStep", autoStepInterval, autoStepInterval);
                    print("Generation Resumed");
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.I)) {
            print("Iterations: " + iterations + "/" + maxIterations);
        }
    }

    void SilentStep() {
        while(iterations < maxIterations - 1) {
            mazeCount = maze.Count;
            if(mazeCount >= (maxCells * cellDensity)) {
                print("Minimum Cell Density Reached: " + mazeCount + " cells ("
                + (100*(mazeCount / maxCells)) + "%)");
                break;
            }
            else {
                WilsonStep();
            }
        }
        viewer.RefreshMaze();
        viewer.RefreshTrack();
    }
    void WilsonStep() {
        ++iterations;
        int trailCount = trail.Count;
        int notMazeCount = notMaze.Count;
        Vector2Int up = Vector2Int.up;
        Vector2Int down = Vector2Int.down;
        Vector2Int right = Vector2Int.right;
        Vector2Int left = Vector2Int.left;
        // If this is the first step in the trail, choose random empty start cell;
        // calculate moves, and end current step.
        if((trailCount < 1)) {
            Vector2Int [] arr = new Vector2Int[notMazeCount];
            notMaze.CopyTo(arr);

            trailHead = arr[Random.Range(0, arr.Length - 1)];
            trailHeadPrev = trailHead;
            trail.Add(trailHead);
            orderedTrail.Add(trailHead);

            // Calculate next set of moves by not selecting cells that
            // are out of bounds or the immediate previous cell.
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

        // If this is not the first step, select head from moves,
        // and check if head is part of current trail or maze.
        else {
            trailHeadPrev = trailHead;
            trailHead = moves[Random.Range(0,moves.Count)];
            int headY = trailHead.y;
            int headX = trailHead.x;
            // If loop is created in trail, delete loop.
            if(trail.Contains(trailHead)) {
                int cutoff = 0;
                for(int i = 0; i < trailCount; i++) {
                    // Find index of overlapping cell and store in cutoff;
                    // update trailHeadPrev to cell behind cutoff.
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
                // Delete vectors of post-cutoff cells in hashtable.
                for(int i = cutoff + 1; i < trailCount; i++) {
                    trail.Remove(orderedTrail[i]);
                }
                // Delete number of vectors after cutoff index.
                orderedTrail.RemoveRange(cutoff, trailCount - cutoff);
                if(debugLog) print(trailHead.ToString() + " Loop Erased.");
            }
            // If trail connects to existing maze, construct, add, and
            // link cells in maze; restart trail and end step.
            else if(mazeCoords.Contains(trailHead)) {
                notMaze.ExceptWith(trail); //remove vectors from notMaze
                mazeCoords.UnionWith(trail); //add trail to mazeCoords
                WilsonCell prevCell = new WilsonCell(orderedTrail[0]);
                for(int i = 0; i < trailCount; i++) {
                    if(i > 0) {
                        // Pass prevCell as argument to forward-link cells in trail.
                        WilsonCell tempCell = new WilsonCell(orderedTrail[i], prevCell);
                        maze.Add(tempCell);
                        prevCell = tempCell;
                        // Link last cell in trail to the cell in the maze
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
                        // Link cell in trail to cell in maze;
                        // Edge case for 1-length trails (?)
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
                if(!silentMode) {
                    viewer.RefreshMaze();
                    viewer.RefreshTrack();
                    viewer.RefreshTrail();
                    viewer.RefreshMoves();
                }
                return;
            }
            // If head was not part of trail or part of maze, calculate
            // set of moves for next step, and add current head to trail.
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
        if(!silentMode) {
            viewer.RefreshTrail();
            viewer.RefreshMoves();
        }
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
