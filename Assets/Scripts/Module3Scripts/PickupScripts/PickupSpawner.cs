using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour // Changed from HealthPickupSpawner to PickupSpawner
{
    public GameObject pickupPrefab; // Refrence to game object that will be used for pickup
    public float spawnDelay; // Used for timer - Are we there yet timer. 
    private float nextSpawnTime; // Used for timer
    private Transform tf; // Can initialize tf with object's current transform if wanted

    private GameObject spawnedPickup; // Stores what health pickup has been spawned


    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // If the spawned pickup is not there
        if (spawnedPickup == null)
        {
            // And it is time to spawn 
            if (Time.time > nextSpawnTime) // Are we there yet
            {
                // Spawn the pickup and set the next time
                spawnedPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else // If there is already a spawnedPickup present...
        {
            // Otherwise, the object still exists, so postpone the spawn
            nextSpawnTime = Time.time + spawnDelay; // Keep adding to time until spawned pickup = null!
        }
    }
}
