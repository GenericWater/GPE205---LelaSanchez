using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Room : MonoBehaviour {

	public GameObject doorNorth;
	public GameObject doorSouth;
	public GameObject doorEast;
	public GameObject doorWest;

	public Transform[] patrolWaypoints; // Added
	// Used to check if an AI Tank is spawned in a room already
	[HideInInspector]public bool isAiTankSpawned = false; // Not serialized but still accessible through other scripts

    public List<AISpawnPoint> aiSpawnPoints; 

}
