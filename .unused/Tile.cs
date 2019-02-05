using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	// Use this for initialization
    public Hashtable elements;
    public Vector2 coord;
    [TextArea(4,10)]
    public string list;
	void Start () {
        elements = new Hashtable();
        list = "";
        addFloor();
        addLeftWall();
        addRightWall();
        addFrontWall();
        addBackWall();
        updateInspectorList();
	}

	// Update is called once per frame
	void Update () {

	}
    Tile() {
        //constuctor
    }
    public void init(Vector2 xy, float spacing) {
        coord = new Vector2(xy.x, xy.y);
        this.gameObject.transform.position =
            new Vector3(coord.x * spacing, 0, coord.y * spacing);
    }
    public void updateInspectorList() {
        list = "";
        foreach(DictionaryEntry entry in elements) {
            list += entry.Key + "\n";
        }
    }
    public GameObject addFloor() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        obj.transform.SetParent(this.gameObject.transform);
        obj.name = "floor";
        obj.transform.eulerAngles = new Vector3(0.0F,0.0F,0.0F);
        obj.transform.localScale = new Vector3(1.0F,1.0F,1.0F);
        obj.transform.localPosition = new Vector3(0.0F,0.0F,0.0F);
        this.elements.Add(obj.name, obj);
        return obj;
    }
    public GameObject addLeftWall() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        obj.transform.SetParent(this.gameObject.transform);
        obj.name = "leftWall";
        obj.transform.eulerAngles = new Vector3(0,0,270);
        obj.transform.localScale = new Vector3(0.5F,1.0F,1.0F);
        obj.transform.localPosition = new Vector3(-5.0F,2.5F,0);
        this.elements.Add(obj.name, obj);
        return obj;
    }
    public GameObject addRightWall() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        obj.transform.SetParent(this.gameObject.transform);
        obj.name = "rightWall";
        obj.transform.eulerAngles = new Vector3(0,0,90);
        obj.transform.localScale = new Vector3(0.5F,1.0F,1.0F);
        obj.transform.localPosition = new Vector3(5.0F,2.5F,0);
        this.elements.Add(obj.name, obj);
        return obj;
    }
    public GameObject addFrontWall() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        obj.transform.SetParent(this.gameObject.transform);
        obj.name = "frontWall";
        obj.transform.eulerAngles = new Vector3(270,0,0);
        obj.transform.localScale = new Vector3(1.0F,1.0F,0.5F);
        obj.transform.localPosition = new Vector3(0,2.5F,5.0F);
        this.elements.Add(obj.name, obj);
        return obj;
    }
    public GameObject addBackWall() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        obj.transform.SetParent(this.gameObject.transform);
        obj.name = "backWall";
        obj.transform.eulerAngles = new Vector3(90,0,0);
        obj.transform.localScale = new Vector3(1.0F,1.0F,0.5F);
        obj.transform.localPosition = new Vector3(0,2.5F,-5.0F);
        this.elements.Add(obj.name, obj);
        return obj;
    }
}
