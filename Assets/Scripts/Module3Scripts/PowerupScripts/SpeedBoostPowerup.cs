using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpeedBoostPowerup : Powerup
{
    // Specifies how much speed to boost 
    public float speedBoostAmount;

    public override void Apply(PowerupManager target)
    {
        Pawn pawn = target.GetComponent<Pawn>();
        if (pawn != null)
        {
            pawn.moveSpeed = pawn.moveSpeed * speedBoostAmount;
        }
    }

    public override void Remove(PowerupManager target)
    {
        Pawn pawn = target.GetComponent<Pawn>();
        if (pawn != null)
        {
            pawn.moveSpeed = pawn.moveSpeed / speedBoostAmount; // division to remove multiplier of speedBoostAmount
        }
    }
}
