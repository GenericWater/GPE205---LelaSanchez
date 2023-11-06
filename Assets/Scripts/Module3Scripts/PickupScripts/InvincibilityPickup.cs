using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPickup : MonoBehaviour
{
    public InvincibilityPowerup powerup; // Refrence to Invincibility powerup script

    public float rotationSpeed = 30f; // Speed for rotation of gameObject

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

    public void OnTriggerEnter(Collider other)
    {
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        // If the other object has a PowerupManager - added to playerPawn on spawn!
        if (powerupManager != null)
        {
            // Add the powerup
            powerupManager.Add(powerup);

            // Destroy the pickup
            Destroy(gameObject); // Destroys object this component is attached to
        }
    }
}
