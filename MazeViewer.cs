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

    void Start() {
        UIMaze = new HashSet<Vector2Int>();
        UITrail = new List<GameObject>();
        UIMovesIndicator = new List<GameObject>();
        UIEmptyIndicator = new List<GameObject>();
        // GameObject test = Instantiate(whiteCell, this.gameObject.transform);
        // test.GetComponent<RectTransform>().anchoredPosition = new Vector2(150,150);
        // test.name = new Vector2Int(1,1).ToString();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            RefreshMaze();
        }
    }

    void init() {
        UIMaze = new HashSet<Vector2Int>();
        UITrail = new List<GameObject>();
        UIMovesIndicator = new List<GameObject>();
        UIEmptyIndicator = new List<GameObject>();
    }

    void RefreshMaze() {
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
        // print("Refreshed Screen, " + MazeObject.maze.Count + " cells on screen.");
    }

    void RefreshTrail() {
        Vector2Int [] cells = new Vector2Int[MazeObject.trail.Count];
        MazeObject.trail.CopyTo(cells);
        foreach(GameObject go in UITrail) {
            Destroy(go);
        }
        UITrail.Clear();
        foreach(Vector2Int c in cells) {
            GameObject newCell;
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

    /*
    protected override void OnPopulateMesh(VertexHelper vh) {
        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;

        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        vert.position = new Vector2(corner1.x, corner1.y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(corner1.x, corner2.y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner2.y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner1.y);
        vert.color = color;
        vh.AddVert(vert);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
    */
}
