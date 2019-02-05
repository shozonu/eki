using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType {
	SquareOpen10,
	SquareHall10z, //wall off x directions (-1,0,0) and (1,0,0)
	SquareHall10x, //wall off z directions (0,0,-1) and (0,0,1)
	SquareCorner10ne, //wall off positive x and z (1,0,1)
	SquareCorner10se,
	SquareCorner10sw,
	SquareCorner10nw
};
