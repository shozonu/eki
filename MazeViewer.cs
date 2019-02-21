using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeViewer : MonoBehaviour {
    public WilsonMaze MazeObject;
    public GameObject UIRect;
    HashSet<Vector2Int> UIMaze;
    HashSet<Vector2Int> UITrack;
    List<GameObject> UITrail;
    List<GameObject> UITrackIndicator;
    List<GameObject> UIMovesIndicator;
    List<GameObject> UIEmptyIndicator;
    float pixelCellWidth;
    float pixelOffsetFromLeft;
    float pixelOffsetFromBottom;
    bool initFlag;

    void Start() {
        //if objects haven't been initialized
        if(initFlag) {
            init();
        }
    }

    // void OnGUI() {
    //     RectTransform rectTransform = GetComponent<RectTransform>();
    //     GUI.Label(new Rect(150, 150, 320, 480), "Rect : " + rectTransform.rect);
    // }

    void Update() {
        //
    }

    void init() {
        //function for initializing objects.
        //may be necessary because other scripts may call this script before
        //this script's Start() function is called.
        UIMaze = new HashSet<Vector2Int>();
        UITrail = new List<GameObject>();
        UITrack = new HashSet<Vector2Int>();
        UIMovesIndicator = new List<GameObject>();
        UIEmptyIndicator = new List<GameObject>();
        UITrackIndicator = new List<GameObject>();
        pixelCellWidth =
            GetComponent<RectTransform>().rect.height /
            GetComponent<WilsonMaze>().sizeY;
        pixelOffsetFromLeft = pixelCellWidth;
        pixelOffsetFromBottom = pixelCellWidth;
        initFlag = true;
    }

    void RefreshMaze() {
        //instantiates blank square images to represent the current maze.
        //instantiates only cells that have not already been instantiated.
        foreach(WilsonCell c in MazeObject.maze) {
            if(!UIMaze.Contains(c.vec)) {
                UIMaze.Add(c.vec);
                GameObject newCell = Instantiate(UIRect, this.gameObject.transform);
                RectTransform newCellTrans = newCell.GetComponent<RectTransform>();
                newCellTrans.sizeDelta = new Vector2(pixelCellWidth, pixelCellWidth);
                newCellTrans.anchoredPosition =
                    new Vector2((pixelOffsetFromLeft + ((c.vec.x - 1) * pixelCellWidth)),
                                pixelOffsetFromBottom + ((c.vec.y - 1) * pixelCellWidth));
                newCell.name = c.vec.ToString();
            }
        }
    }

    void RefreshTrail() {
        //instantiates green square images to represent current trail.
        //since trail may have been erased during generation,
        //all trail cell sprites must be destroyed before instantiating the updated trail.
        foreach(GameObject go in UITrail) {
            Destroy(go);
        }
        UITrail.Clear();
        foreach(Vector2Int c in MazeObject.trail) {
            GameObject newCell;
            //different cell color for current head and previous head cells
            if(c == MazeObject.trailHead) {
                newCell = Instantiate(UIRect, this.gameObject.transform);
                newCell.GetComponent<Graphic>().color = Color.red;
            }
            else if(c == MazeObject.trailHeadPrev) {
                newCell = Instantiate(UIRect, this.gameObject.transform);
                newCell.GetComponent<Graphic>().color = new Color(1f, 0.5f, 0f, 1f);
            }
            else {
                newCell = Instantiate(UIRect, this.gameObject.transform);
                newCell.GetComponent<Graphic>().color = new Color(0.4f, 1f, 0.4f, 1f);
            }
            RectTransform newCellTrans = newCell.GetComponent<RectTransform>();
            newCellTrans.sizeDelta = new Vector2(pixelCellWidth, pixelCellWidth);
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
            GameObject indi = Instantiate(UIRect, this.gameObject.transform);
            indi.GetComponent<Graphic>().color = new Color(0.8f, 0.2f, 0.8f, 1f);
            RectTransform newCellTrans = indi.GetComponent<RectTransform>();
            newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5f, pixelCellWidth * 0.5f);
            indi.GetComponent<RectTransform>().anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((v.x - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                            pixelOffsetFromBottom + ((v.y - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F));
            indi.name = "[Potential Move] " + v.ToString();
            UIMovesIndicator.Add(indi);
        }
    }

    void RefreshTrack() {
        foreach(GameObject go in UITrackIndicator) {
            Destroy(go);
        }
        UITrack.Clear();
        foreach(WilsonCell c in MazeObject.maze) {
            if(c.next != null) {
                GameObject track = Instantiate(UIRect, this.gameObject.transform);
                track.GetComponent<Graphic>().color = Color.red;
                RectTransform newCellTrans = track.GetComponent<RectTransform>();
                int xtra = 1;
                int ytra = 1; //anchor offset depending on direction of indicator
                if(c.next.vec - c.vec == Vector2Int.up) {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5F, pixelCellWidth * 1.5F);
                }
                else if(c.next.vec - c.vec == Vector2Int.down) {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5F, pixelCellWidth * 1.5F);
                    ytra = 2;
                }
                else if(c.next.vec - c.vec == Vector2Int.right) {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 1.5F, pixelCellWidth * 0.5F);
                }
                else {
                    newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 1.5F, pixelCellWidth * 0.5F);
                    xtra = 2;
                }
                //setting anchor position using offset xtra and ytra
                track.GetComponent<RectTransform>().anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((c.vec.x - xtra) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                pixelOffsetFromBottom + ((c.vec.y - ytra) * pixelCellWidth) + (pixelCellWidth * 0.25F));

                track.name = "[Line] " + c.vec.ToString() + " -> " + c.next.vec.ToString();
                UITrackIndicator.Add(track);
                UITrack.Add(c.vec);
            }
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
            GameObject newCell = Instantiate(UIRect, this.gameObject.transform);
            newCell.GetComponent<Graphic>().color = new Color(1f, 0.4f, 0.6f, 0.33f);
            RectTransform newCellTrans = newCell.GetComponent<RectTransform>();
            newCellTrans.sizeDelta = new Vector2(pixelCellWidth * 0.5f, pixelCellWidth * 0.5f);
            newCell.GetComponent<RectTransform>().anchoredPosition =
                new Vector2((pixelOffsetFromLeft + ((c.x - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F)),
                            pixelOffsetFromBottom + ((c.y - 1) * pixelCellWidth) + (pixelCellWidth * 0.25F));
            newCell.name = "[Empty] " + c.ToString();
            UIEmptyIndicator.Add(newCell);
        }
    }
}
