using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeViewer : MonoBehaviour {
    private WilsonMaze MazeObject;
    public bool initFlag;
    public GameObject UIRect; //reference blank image for cell graphic
    HashSet<Vector2Int> UIMaze; //hashsets for checking occupancy of vectors
    HashSet<Vector2Int> UITrack;
    List<GameObject> UITrail; //for keeping references to instantiated GameObjects
    List<GameObject> UITrackIndicator;
    List<GameObject> UIMovesIndicator;
    List<GameObject> UIEmptyIndicator;
    float pixelCellWidth;
    float pixelOffsetFromLeft;
    float pixelOffsetFromBottom;

    void Start() {
        //if objects haven't been initialized
        if(!initFlag) {
            init();
        }
    }

    void Update() {
        //
    }

    public void init() {
        //Function for initializing objects.
        //May be necessary because other scripts may call this script before
        //this script's Start() function is called,
        // so they may call this function manually
        MazeObject = GetComponent<WilsonMaze>();
        int area = MazeObject.sizeX * MazeObject.sizeY;
        //Initialize collection objects with 'area' capacity to avoid
        //capacity resizing during runtime.
        UIMaze = new HashSet<Vector2Int>();
        UITrail = new List<GameObject>();
        UITrack = new HashSet<Vector2Int>();
        UIMovesIndicator = new List<GameObject>();
        UIEmptyIndicator = new List<GameObject>();
        UITrackIndicator = new List<GameObject>();
        //calculate cell width depending on screen size
        pixelCellWidth =
            GetComponent<RectTransform>().rect.height /
            MazeObject.sizeY;
        //needs offset equal to width for some reason
        pixelOffsetFromLeft = pixelCellWidth;
        pixelOffsetFromBottom = pixelCellWidth;
        initFlag = true;
    }

    public void RefreshMaze() {
        //Instantiates white squares to represent the cells occupied
        //by the current maze.
        //Instantiates only cells that have not already been instantiated.
        foreach(WilsonCell c in MazeObject.maze) {
            Vector2Int vec = c.vec;
            if(!UIMaze.Contains(vec)) {
                UIMaze.Add(vec);
                GameObject newCell = Instantiate(UIRect, this.gameObject.transform);
                RectTransform newCellTrans = newCell.GetComponent<RectTransform>();
                //set size of cell in pixels
                newCellTrans.sizeDelta = new Vector2(pixelCellWidth, pixelCellWidth);
                //cell anchor is bottom left corner of rectangle
                newCellTrans.anchoredPosition =
                    new Vector2((pixelOffsetFromLeft + ((vec.x - 1) * pixelCellWidth)),
                                pixelOffsetFromBottom + ((vec.y - 1) * pixelCellWidth));
                newCell.name = vec.ToString();
            }
        }
    }

    public void RefreshTrail() {
        //Instantiates green squares to represent cells occupied by current trail.
        //Since trail may have been erased during generation, all trail cell
        //sprites must be destroyed before instantiating the updated trail.
        foreach(GameObject go in UITrail) {
            Destroy(go);
        }
        UITrail.Clear();
        Transform trfm = this.gameObject.transform;
        foreach(Vector2Int c in MazeObject.trail) {
            GameObject newCell;
            newCell = Instantiate(UIRect,trfm);
            //different cell color for current head and previous head cells
            if(c == MazeObject.trailHead) {
                newCell.GetComponent<Graphic>().color = new Color(1f, 0f, 0f, 1f);
                //red for current trail head
            }
            else if(c == MazeObject.trailHeadPrev) {
                newCell.GetComponent<Graphic>().color = new Color(1f, 0.5f, 0f, 1f);
                //orange for last trail head
            }
            else {
                newCell.GetComponent<Graphic>().color = new Color(0.4f, 1f, 0.4f, 1f);
                //green for trail cells
            }
            RectTransform newCellTrans = newCell.GetComponent<RectTransform>();
            newCellTrans.sizeDelta = new Vector2(pixelCellWidth, pixelCellWidth);
            newCellTrans.anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((c.x - 1) * pixelCellWidth)),
                            pixelOffsetFromBottom + ((c.y - 1) * pixelCellWidth));
            newCell.name = c.ToString() + " (Trail)";
            UITrail.Add(newCell);
        }
    }

    public void RefreshMoves() {
        //sprites for representing the current valid cells for next move.
        //current instances are destroyed before instantiating updated sprites
        foreach(GameObject go in UIMovesIndicator) {
            Destroy(go);
        }
        foreach(Vector2Int v in MazeObject.moves) {
            GameObject indi = Instantiate(UIRect, this.gameObject.transform);
            indi.GetComponent<Graphic>().color = new Color(0.8f, 0.2f, 0.8f, 1f);
            //purple colored
            RectTransform newCellTrans = indi.GetComponent<RectTransform>();
            newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5f, pixelCellWidth * 0.5f);
            newCellTrans.anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((v.x - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                            pixelOffsetFromBottom + ((v.y - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F));
            indi.name = "[Potential Move] " + v.ToString();
            UIMovesIndicator.Add(indi);
        }
    }

    public void RefreshTrack() {
        //Instantiates lines that represent the direction of each cell's
        //link to each other.
        foreach(GameObject go in UITrackIndicator) {
            Destroy(go);
        }
        UITrack.Clear();
        Transform thisTsfm = this.gameObject.transform;
        foreach(WilsonCell c in MazeObject.maze) {
            if(c.next != null) {
                GameObject track = Instantiate(UIRect, thisTsfm);
                track.GetComponent<Graphic>().color = new Color(1f, 0f, 0f, 1f);
                //color track line is red
                RectTransform newCellTrans = track.GetComponent<RectTransform>();
                WilsonCell nxt = c.next;
                Vector2Int nxtVec = c.next.vec;
                Vector2Int vec = c.vec;
                int xtra = 1;
                int ytra = 1; //anchor offset depending on direction of indicator
                if(nxtVec - vec == Vector2Int.up) {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5F, pixelCellWidth * 1.5F);
                }
                else if(nxtVec - vec == Vector2Int.down) {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5F, pixelCellWidth * 1.5F);
                    ytra = 2;
                }
                else if(nxtVec - vec == Vector2Int.right) {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 1.5F, pixelCellWidth * 0.5F);
                }
                else {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 1.5F, pixelCellWidth * 0.5F);
                    xtra = 2;
                }
                //setting anchor position using offset xtra and ytra
                newCellTrans.anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((vec.x - xtra) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                pixelOffsetFromBottom + ((vec.y - ytra) * pixelCellWidth) + (pixelCellWidth * 0.25F));

                track.name = "[Line] " + vec.ToString() + " -> " + nxtVec.ToString();
                UITrackIndicator.Add(track);
                UITrack.Add(vec);
            }
        }
    }

    public void RefreshEmpty() {
        //Instantiates sprites representing empty space
        //called once before any other cells are created.
        //Other cells that are created are layered on top of these.
        Vector2Int [] cells = new Vector2Int[MazeObject.notMaze.Count];
        MazeObject.notMaze.CopyTo(cells);
        foreach(GameObject go in UIEmptyIndicator) {
            Destroy(go);
        }
        UIEmptyIndicator.Clear();
        foreach(Vector2Int c in cells) {
            GameObject newCell = Instantiate(UIRect, this.gameObject.transform);
            newCell.GetComponent<Graphic>().color = new Color(1f, 0.4f, 0.6f, 0.33f);
            //empty cell is transparent red
            RectTransform newCellTrans = newCell.GetComponent<RectTransform>();
            newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5f, pixelCellWidth * 0.5f);
            newCellTrans.anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((c.x - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                            pixelOffsetFromBottom + ((c.y - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F));
            newCell.name = "[Empty] " + c.ToString();
            UIEmptyIndicator.Add(newCell);
        }
    }
}
