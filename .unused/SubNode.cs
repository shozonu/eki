using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubNode : MonoBehaviour {

	SubNodeType type;

	SubNode() {
		type = SubNodeType.Xp;
	}
	SubNode(SubNodeType nt) {
		type = nt;
	}
	public void setType(SubNodeType nt) {
		type = nt;
	}
	public SubNodeType getType() {
		return type;
	}
}
