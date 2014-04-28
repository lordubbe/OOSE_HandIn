using UnityEngine;
using System.Collections;

public class Node {

	public float hCost;
	public float cCost;
	public float fCost;
	public float speedM;
	public float dmgCost;
	
	public int x,y; //position in the matrix
	
	public bool walkable;
	public bool finish;
	public bool start;
	
	public Node parent;
	
	
	public Node(Tile t, int x, int y){
		finish = false;
		start = false;
		parent = null;
		hCost = 0; 
		cCost = 0; 
		fCost = 0; 
		dmgCost = 0;
		speedM = 1;
		this.x = (int)x;
		this.y = (int)y;
		
		if(t == null){
			
			walkable = false;
		}else{
		
			dmgCost = t.damage;
			speedM = t.speed;
			walkable = t.isWalkable;
			
		}
		
	}
	
	public Node(bool walkable,int x, int y){
		start = false;
		finish = false;
		parent = null;
		hCost = 0; 
		cCost = 0; 
		fCost = 0; 
		dmgCost = 0;
		speedM = 1;
		this.x = x;
		this.y = y;
		this.walkable = walkable;
	}
	public Node(){
		start = false;
		finish = false;
		parent = null;
		hCost = 0; 
		cCost = 0; 
		fCost = 0; 
		dmgCost = 0;
		speedM = 1;
		x = 0 ; 
		y = 0;
		parent = null;
	}
	public override string ToString ()
	{
		return string.Format ("[Node] x:["+x+"] y:["+y+"] c:["+cCost+"] h:["+hCost+"] f:["+fCost+"]");
	}
}
