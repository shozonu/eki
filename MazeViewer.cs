using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeViewer : MonoBehaviour {
    public WilsonMaze MazeObject;
    public GameObject whiteCell;
    public GameObject greenCell;
    public GameObject redCell;
    public GameObject orangeCell;
    public GameObject purpleMiniCell;
    public GameObject pinkMiniFadeCell;
    public int pixelOffsetFromLeft;
    public int pixelOffsetFromBottom;
    public int pixelCellWidth;
    HashSet<Vector2Int> UIMaze;
    List<GameObject> UITrail;
    List<GameObject> UIMovesIndicator;
    List<GameObject> UIEmptyIndicator;
    bool initFlag;

    void Start() {
        //if objects haven't been initialized
        if(initFlag) {
            init();
        }
    }

    void Update() {
        //
    }

    void init() {
        //function for initializing objects.
        //may be necessary because other scripts may call this script before
        //this script's Start() function is called.
        UIMaze = new HashSet<Vector2Int>();
        UITrail = new List<GameObject>();
        UIMovesIndicator = new List<GameObject>();
        UIEmptyIndicator = new List<GameObject>();
        initFlag = true;
    }

    void RefreshMaze() {
        //instantiates blank square images to represent the current maze.
        //copies all maze cells to array and instantiates them iteratively.
        //instantiates only cells that have not already been instantiated.
        Vector2Int [] cells = new Vector2Int[MazeObject.maze.Count];
        MazeObject.maze.CopyTo(cells);
        foreach(Vector2Int c in cells) {
            if(!UIMaze.Contains(c)) {
                UIMaze.Add(c);
                GameObject newCell = Instantiate(whiteCell, this.gameObject.transform);
                newCell.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2((pixelOffsetFromLeft + ((c.x - 1) * pixelCellWidth)),
                                pixelOffsetFromBottom + ((c.y - 1) * pixelCellWidth));
                newCell.name = c.ToString();
            }
        }
    }

    void RefreshTrail() {
        //instantiates green square images to represent current trail.
        //copies trail cells to array and instantiates them.
        //since trail may have been erased during generation,
        //all trail cell sprites must be destroyed before instantiating the updated trail.
        Vector2Int [] cells = new Vector2Int[MazeObject.trail.Count];
        MazeObject.trail.CopyTo(cells);
        foreach(GameObject go in UITrail) {
            Destroy(go);
        }
        UITrail.Clear();
        foreach(Vector2Int c in cells) {
            GameObject newCell;
            //different cell color for current head and previous head cells
            if(c == MazeObject.trailHead) {
                newCell = Instantiate(redCell, this.gameObject.transform);
            }
            else if(c == MazeObject.trailHeadPrev) {
                newCell = Instantiate(orangeCell, this.gameObject.transform);
            }
            else {
                newCell = Instantiate(greenCell, this.gameObject.transform);
            }
            newCell.GetComponent<RectTransform>().anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((c.x - 1) * pixelCellWidth)),
                            pixelOffsetFromBottom + ((c.y - 1) * pixelCellWidth));
            newCell.name = c.ToString() + " (Trail)";
            UITrail.Add(newCell);
        }
    }

    void RefreshMoves() {
        //sprites for representing the current valid cells for next move.
        //current instances are destroyed before instantiating updated sprites
        foreach(GameObject go in UIMovesIndicator) {
            Destroy(go);
        }
        foreach(Vector2Int v in MazeObject.moves) {
            GameObject indi = Instantiate(purpleMiniCell, this.gameObject.transform);
            indi.GetComponent<RectTransform>().anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((v.x - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                            pixelOffsetFromBottom + ((v.y - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F));
            indi.name = "[Potential Move] " + v.ToString();
            UIMovesIndicator.Add(indi);
        }
    }

    void RefreshEmpty() {
        //instantiates sprites representing empty space
        //called once before any other cells are created.
        //other cells that are created are layered on top of these.
        Vector2Int [] cells = new Vector2Int[MazeObject.notMaze.Count];
        MazeObject.notMaze.CopyTo(cells);
        foreach(GameObject go in UIEmptyIndicator) {
            Destroy(go);
        }
        UIEmptyIndicator.Clear();
        foreach(Vector2Int c in cells) {
            GameObject newCell = Instantiate(pinkMiniFadeCell, this.gameObject.transform);
            newCell.GetComponent<RectTransform>().anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((c.x - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                            pixelOffsetFromBottom + ((c.y - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F));
            newCell.name = "[Empty] " + c.ToString();
            UIEmptyIndicator.Add(newCell);
        }
    }
}
