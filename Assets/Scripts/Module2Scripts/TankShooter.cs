using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter // fireRate is set in pawn
{
    public Transform firepointTransform; // Public transform that will be linked to the firePoint on Tank in game

    public float nextShootTime; // Used this variable to set time delay between fireRate

    private float fireRate;

    private float timerDelay; 




    // Start is called before the first frame update
    public override void Start()
    {
        /*
        //float shotsPerSecond;
        fireRate = gameObject.GetComponent<Pawn>().fireRate; // Gets component from Pawn Script
        float secondsPerShot; // We will convert to shots per second


        if (fireRate <= 0)
        {
            secondsPerShot = Mathf.Infinity;
        }
        else
        {
            secondsPerShot = 1 / fireRate; 
        }

        timerDelay = secondsPerShot;

        nextShootTime = Time.time + timerDelay;
        */

    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
            //Instantiate our projectile
            GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject;

            //Get the DamageOnHit component
            DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

            // If it has this component (doh) ...
            if (doh != null)
            {
                // ... set the damageDone in the DamageOnHit component to the value passed in
                //doh.damageDone = damageDone;
                // ORRR
                doh.setDamageDone(damageDone); // Public void on DamageOnHit component script

                // ... set the owner to the pawn that shot this shell, if there is one (otherwise, owner is null).
                doh.owner = GetComponent<Pawn>();
            }

            // Get the rigidbody component
            Rigidbody rb = newShell.GetComponent<Rigidbody>();

            // If it has a rigidbody component...
            if (rb != null)
            {
                // ... AddForce to make it move forward
                rb.AddForce(firepointTransform.forward * fireForce); // fire force is speed of bullet
            }

            // Destroy it after a set time
            Destroy(newShell, lifeSpan);

        //return;

    }
}
