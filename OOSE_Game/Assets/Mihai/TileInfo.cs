/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/
using UnityEngine;
using System.Collections;

public class TileInfo {

	public bool visited;
	public bool interior;
	public bool border;
	public int x, y;
	public TileInfo(int x,int y){
		this.x = x;
		this.y = y;
		visited = interior = border = false;
	}
}
