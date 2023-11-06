using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtraLifePowerup : Powerup
{


    public override void Apply(PowerupManager target)
    {
        Health targetHealth = target.GetComponent<Health>(); // local variable to get health component.. will call GainLife() function

        if (targetHealth != null) 
        {
            targetHealth.GainLife();
        }
    }

    public override void Remove(PowerupManager target)
    {
        Health targetHealth = target.GetComponent<Health>(); // local variable to get health component.. will call GainLife() function

        if (targetHealth != null)
        {
            //targetHealth.LoseLife(); // If powerup is not made permanaent, will run LoseLife function from Health script
        }
    }
}
