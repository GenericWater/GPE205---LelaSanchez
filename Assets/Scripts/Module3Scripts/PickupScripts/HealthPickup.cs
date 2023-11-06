using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public HealthPowerup powerup; // Refrence to HealthPowerup script to use its functions in this code - access the healthToAdd variable from HealthPowerup script

    public float rotationSpeed = 30f; // Adjust speed as needed 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object this component is on
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotates on Y axis or Vector3.up
    }

    public void OnTriggerEnter(Collider other) // Event so doesn't need to be called in any part of script.
    {
        // Variable to store other object's PowerupManager/Controller - if it has one
        PowerupManager powerupManager = other.GetComponent<PowerupManager>(); // stores variable for collider here locally under powerupManager

        // If the other object has a PowerupManager
        if (powerupManager != null )
        {
            // Add the powerup
            powerupManager.Add(powerup);

            // Destroy the pickup
            Destroy(gameObject); // Destroys object this component is attached to
        }
    }
}
