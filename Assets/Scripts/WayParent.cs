using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayParent : MonoBehaviour {
	public List<WayPointScript> Waypoints = new List<WayPointScript>();
	// Use this for initialization
	void Start () {
		WayPointScript[] temp = GetComponentsInChildren<WayPointScript>();
		foreach (WayPointScript waypoint in temp) {
			Waypoints.Add (waypoint);
		}
		}
		
	
	// Update is called once per frame
	void Update () {
	
	}
}
