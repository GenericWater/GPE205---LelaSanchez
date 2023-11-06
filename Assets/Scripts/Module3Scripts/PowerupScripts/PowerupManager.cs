using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerups; // List of all powerups | Can add to this!

    private List<Powerup> removedPowerupQueue; // Holds removable power ups

    // Start is called before the first frame update
    public void Start()
    {
        powerups = new List<Powerup>(); // initalize the list to use. 

        removedPowerupQueue = new List<Powerup>(); // initalize the new list to use it!
    }

    // Update is called once per frame
    public void Update()
    {
        DecrementPowerupTimers();
    }

    private void LateUpdate()
    {
        ApplyRemovePowerupsQueue();
    }

    // The Add function will eventually add a powerup 
    public void Add(Powerup powerupToAdd)
    {
        // TODO: Create the Add() Method
        powerupToAdd.Apply(this); // Calls the Apply function so the powerup could be applied to itself

        // Save it to the powerups list
        powerups.Add(powerupToAdd);

    }

    // The Remove function will eventually remove a powerup
    public void Remove(Powerup powerupToRemove)
    {
        // TODO: Create the Remove() Method
        powerupToRemove.Remove(this);

        // Add it to the "to be removed queue"
        removedPowerupQueue.Add(powerupToRemove);
    }

    public void DecrementPowerupTimers()
    {
        // One-at-a-time, put each object in "powerup" and do the loop body on it
        foreach (Powerup powerup in powerups) // type is Powerup class, powerup is local variable, in, the list powerups
        {
            if (!powerup.isPermanent) // If the public bool isPermanent is not checked, do this...
            {
                // Subtract the time it took to draw the frame from the duration
                powerup.duration -= Time.deltaTime;

                // If time is up, we want to remove this powerup
                if (powerup.duration <= 0)
                {
                    Remove(powerup);
                }
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        // Now that we are sure we are not iterating through "powerups", remove the powerups that are in our temporary list
        foreach (Powerup powerup in removedPowerupQueue)
        {
            powerups.Remove(powerup); // uses list function Remove
        }
        // And reset our temporary list after the loop is done
        removedPowerupQueue.Clear();
    }
}
