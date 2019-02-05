using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {
	public float moveSpeed;
    public float rotationSpeed;
	Transform objectTransform;
	// Use this for initialization
	void Start () {
		objectTransform = this.transform;
	}

	// Update is called once per frame
	void Update () {
		Vector3 vecPos = new Vector3(0,0,0);
        Vector3 vecRot = new Vector3(0,0,0);
		if(Input.GetKey(KeyCode.W)) {
			vecPos = vecPos + Vector3.Scale(objectTransform.forward, new Vector3(1,0,1)).normalized;
		}
		if(Input.GetKey(KeyCode.A)) {
			vecPos = vecPos - objectTransform.right;
		}
		if(Input.GetKey(KeyCode.S)) {
			vecPos = vecPos - Vector3.Scale(objectTransform.forward, new Vector3(1,0,1)).normalized;
		}
		if(Input.GetKey(KeyCode.D)) {
			vecPos = vecPos + objectTransform.right;
		}
		if(Input.GetKey(KeyCode.Space)) {
			vecPos = vecPos + Vector3.up;
		}
		if(Input.GetKey(KeyCode.LeftShift)) {
			vecPos = vecPos + Vector3.down;
		}
        if(Input.GetKey(KeyCode.Q)) {
            vecRot = vecRot + new Vector3(0,-1,0);
        }
        if(Input.GetKey(KeyCode.E)) {
            vecRot = vecRot + new Vector3(0,1,0);
        }
		objectTransform.position += (vecPos * Time.deltaTime * moveSpeed);
        objectTransform.eulerAngles += (vecRot * Time.deltaTime * rotationSpeed);
	}
}
