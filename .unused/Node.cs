using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	NodeType type;
	GameObject nodeParent;
	float nodeWidth;
	int nodeIndex;

	public GameObject[] adjNode;

	Node() {
		type = NodeType.SquareOpen10;
		nodeParent = null;
		nodeWidth = 10F;
		nodeIndex = -1;
		init();
	}
	Node(NodeType nt) {
		type = nt;
		nodeParent = null;
		nodeWidth = 10F;
		nodeIndex = -1;
		init();
	}
	Node(NodeType nt, float f) {
		type = nt;
		nodeParent = null;
		nodeWidth = f;
		nodeIndex = -1;
		init();
	}
	Node(NodeType nt, float f, GameObject n) {
		type = nt;
		nodeParent = n;
		nodeWidth = f;
		nodeIndex = -1;
		init();
	}
	Node(Node no) {
		type = no.type;
		nodeParent = no.nodeParent;
		nodeWidth = no.nodeWidth;
		nodeIndex = no.getNodeIndex();
		init();
		for(int i = 0; i < 9; i++) {
			adjNode[i] = no.adjNode[i];
		}
	}
/*
	static public void reload(GameObject oldTile) {
		Node temp = new Node();
		temp.copy(oldTile.GetComponent<Node>());
		Destroy(oldTile);
		GameObject newTile;
		newTile = GameObject.Find("Map").GetComponent<LevelGrid>().createTile(temp.getType());
		newTile.GetComponent<Node>().copy(temp);
	}
	*/
	void init() {
		adjNode = new GameObject[9];
		for(int k = 0; k < 9; k++) {
			adjNode[k] = null;
		}
		//8 possible adjacent nodes, in the 8 directions, ordered like a numpad.
		//index 0 will not be used
	}
	public void copy(Node nod) {
		type = nod.type;
		nodeParent = nod.nodeParent;
		nodeWidth = nod.nodeWidth;
		for(int i = 0; i < 9; i++) {
			adjNode[i] = nod.adjNode[i];
		}
	}
	public void setType(NodeType nt) {
		type = nt;
	}
	public void setNodeParent(GameObject n) {
		nodeParent = n;
		print(this + "'s nodeParent set to " + n);
	}
	public void setNodeWidth(float f) {
		nodeWidth = f;
	}
	public void setNodeIndex(int i) {
		nodeIndex = i;
	}
	public NodeType getType() {
		return type;
	}
	public GameObject getNodeParent() {
		return nodeParent;
	}
	public float getNodeWidth() {
		return nodeWidth;
	}
	public int getNodeIndex() {
		return nodeIndex;
	}
}
