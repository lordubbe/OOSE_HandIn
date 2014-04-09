using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding  {
	
	private Node[,] nodeMap;
	private List<Node> openList;
	private List<Node> closedList;
	public delegate void pathF(Vector3[] path);
	public event pathF p;
	private Vector3 objPos;
	private Vector3[] path;
	private Node start;
	private Node end;
	private float height;
	private float error;
    
	public PathFinding(Vector3 sPos,Vector3 fPos, pathF completeF, float error = 0, float height = 1){
		
        buildNodeMap ();
		
		objPos = sPos;
		this.height = height;
		sPos = new Vector3(Mathf.Clamp (sPos.x,0,LevelSpawn.levelMatrix.GetLength(0)*LevelSpawn.tileWidth),sPos.y, Mathf.Clamp (sPos.z,0,LevelSpawn.levelMatrix.GetLength(1)*LevelSpawn.tileHeight));
		fPos = new Vector3(Mathf.Clamp (fPos.x,0,LevelSpawn.levelMatrix.GetLength(0)*LevelSpawn.tileWidth),fPos.y, Mathf.Clamp (fPos.z,0,LevelSpawn.levelMatrix.GetLength(1)*LevelSpawn.tileHeight));
		start = nodeMap[(int)(sPos.x/LevelSpawn.tileWidth),(int)(sPos.z/LevelSpawn.tileHeight)];
		end   = nodeMap[(int)(fPos.x/LevelSpawn.tileWidth),(int)(fPos.z/LevelSpawn.tileHeight)];
		
		end.finish = true;
		start.start = true;
		
		
		calculatePath();
		completeF(path);
		/*
		p = completeF;
		openList = new List<Node>();
		closedList = new List<Node>();
		
		openList.Add (start);
		Performance.UpdateEvent+=distributedPathCalculator;
		*/
        
	}
	
	
	private void buildNodeMap(){
		nodeMap = new Node[LevelSpawn.levelMatrix.GetLength(0),LevelSpawn.levelMatrix.GetLength(1)];
		
		for(int x = 0 ; x<LevelSpawn.levelMatrix.GetLength(0); x++){
			for(int y = 0 ; y<LevelSpawn.levelMatrix.GetLength(1); y++){
				nodeMap[x,y] = new Node(LevelSpawn.levelMatrix[x,y],x,y);
				if(nodeMap[x,y].walkable)Debug.DrawRay(new Vector3(x*LevelSpawn.tileWidth,2,y*LevelSpawn.tileHeight),Vector3.forward*0.1f,Color.green,10);
			}
		}
	}
	
	private float calculateCCost(Node curNode, Node thisNode){
		
		return curNode.cCost + (calculateHCost(curNode,thisNode)+ thisNode.dmgCost)/thisNode.speedM;
		
		
	}
	
	
	private float calculateHCost(Node first, Node next){
		return Mathf.Min (Mathf.Abs (first.x-next.x), Mathf.Abs (first.y-next.y)) * 14 +
			(Mathf.Max (Mathf.Abs (first.x-next.x), Mathf.Abs (first.y-next.y))-Mathf.Min (Mathf.Abs (first.x-next.x), Mathf.Abs (first.y-next.y)))*10;
	}
	/*
	private void distributedPathCalculator(){
		if(!closedList.Contains(end) && openList.Count>0){
			Node current = smallestFCost(openList.ToArray());
			openList.Remove(current);
			closedList.Add (current);
			
			for(int x = -1; x<2; x++){
				for(int y = -1; y<2; y++){
					if(x!=0 || y !=0){
						
						if(current.x+x>=0 && current.x+x<nodeMap.GetLength(0) && current.y+y>=0 && current.y+y<nodeMap.GetLength(1)){
							Node visited = nodeMap[current.x+x,current.y+y];
							if(visited.walkable && !closedList.Contains(visited)){
								if(openList.Contains(visited)){
									float cCost = calculateCCost(current,visited);
									if(cCost<visited.cCost){
										visited.cCost = cCost;
										visited.fCost = cCost+visited.hCost;
										visited.parent = current;
									}
								}else{
									visited.cCost = calculateCCost(current,visited);
									visited.hCost = calculateHCost(visited,end);
									visited.fCost = visited.cCost+visited.hCost;
									visited.parent = current;
									openList.Add (visited);
								}
							}
						}
					}
				}
				
			}
		}else{
			Performance.UpdateEvent-=distributedPathCalculator;
			if(closedList.Contains(end)){
				populatePath();
				p(path);
			}else{
				path = new Vector3[0];
			}
		}
	}
	*/
	
	private void calculatePath(){
		openList = new List<Node>();
		closedList = new List<Node>();
		
		openList.Add (start);
		Node current = null;
		while(!closedList.Contains(end) && openList.Count>0){
			current = smallestFCost(openList.ToArray());
			openList.Remove(current);
			closedList.Add (current);
			
			for(int x = -1; x<2; x++){
				for(int y = -1; y<2; y++){
					if(x!=0 || y !=0){
						
						if(current.x+x>=0 && current.x+x<nodeMap.GetLength(0) && current.y+y>=0 && current.y+y<nodeMap.GetLength(1)){
							Node visited = nodeMap[current.x+x,current.y+y];
							if(visited.walkable && !closedList.Contains(visited)){
								if(openList.Contains(visited)){
									float cCost = calculateCCost(current,visited);
									if(cCost<visited.cCost){
										visited.cCost = cCost;
										visited.fCost = cCost+visited.hCost;
										visited.parent = current;
									}
								}else{
									visited.cCost = calculateCCost(current,visited);
									visited.hCost = calculateHCost(visited,end);
									visited.fCost = visited.cCost+visited.hCost;
									visited.parent = current;
									openList.Add (visited);
								}
							}
						}
					}
			    }
			
		   }
		   
		
		}
		if(closedList.Contains(end)){
			populatePath();
		}else{
			path = new Vector3[0];
		}
		
		
	}
	
	private void populatePath(){
		List<Vector3> pathList = new List<Vector3>();
		Node visitor = end;
		while(visitor != start){
			pathList.Add (new Vector3(visitor.x*LevelSpawn.tileWidth,height,visitor.y*LevelSpawn.tileHeight));
			visitor = visitor.parent;
		} 
		path = new Vector3[pathList.Count+1];
		path[0] = objPos;
		for(int i =1; i<path.Length; i++){
			path[i] = pathList[pathList.Count - i]+ new Vector3(Random.Range(-error,error),0,Random.Range(-error,error));
		}
		
	}
	private Node smallestFCost(Node[] list){
		float minF = float.MaxValue;
		Node resultN = new Node();
		foreach(Node n in list){
			if(minF>n.fCost){
				minF = n.fCost;
				resultN = n;
			}
		}
		return resultN;
	}
	
}
