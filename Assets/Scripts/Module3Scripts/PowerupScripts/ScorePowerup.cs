using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // Module 4 Script to make Score Pickup
public class ScorePowerup : Powerup
{
    public float scoreToAdd;
    public override void Apply(PowerupManager target)
    {
        Pawn pawn = target.GetComponent<Pawn>();

        if (pawn != null)
        {
            pawn.controller.AddToScore(scoreToAdd); 
        }
    }

    public override void Remove(PowerupManager target)
    {
        // Not to be implimented
    }
}
