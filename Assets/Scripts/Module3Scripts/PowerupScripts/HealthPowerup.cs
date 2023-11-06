[System.Serializable] // Did this to make healthToAdd exposed in inspector
public class HealthPowerup : Powerup // Not going to be attached to anything in game specifically. ONLY will be refrenced through Pickup!
{
    // Specifies how much health to add
    public float healthToAdd;

    public override void Apply(PowerupManager target)
    {
        // Apply Health changes
        Health targetHealth = target.GetComponent<Health>(); // local variable to get target health
        if (targetHealth != null )
        {
            // The second parameter is the pawn who caused healing - in this case, they heal themselves | PowerupManager provides the health increase to the player
            targetHealth.Heal(healthToAdd, target.GetComponent<Pawn>());
        }
    }

    public override void Remove(PowerupManager target)
    {
        // TODO: Remove Health changes - to remove health buff we will take damage by same amount healed
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null )
        {
           // targetHealth.TakeDamage(healthToAdd, target.GetComponent<Pawn>()); // Calls TakeDamage function from Health script
        }
    }

}
