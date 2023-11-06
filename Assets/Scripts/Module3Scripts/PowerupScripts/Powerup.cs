public abstract class Powerup // No Monobehavior because not a component - nor will be directly assigned to a game object
{
    public float duration; // Variable for designers to control the length a powerup is applied for

    public bool isPermanent; // Variable for designers to check if a certain powerup will be permanent
    public abstract void Apply(PowerupManager target);
    public abstract void Remove(PowerupManager target);

}
