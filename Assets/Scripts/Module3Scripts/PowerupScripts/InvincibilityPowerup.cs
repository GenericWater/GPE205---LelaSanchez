using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // This needs to be added to expose duration and isPermanent Bool
public class InvincibilityPowerup : Powerup
{
    private Health healthComponent; // refrenece to Health script

    private GameObject pointLightObject; // Reference to the point light GameObject
    private Light pointLight; // Reference to the Light component

    public override void Apply(PowerupManager target)
    {
        // TODO: make the player immune to damage and display a visual effect.
        //Health targetHealth = target.GetComponent<Health>();
        healthComponent = target.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.SetIsInvincible(true); // disable the health component entirely during invincibility

            // Create a GameObject for the point light if it doesn't exist
            if (pointLightObject == null)
            {
                pointLightObject = new GameObject("InvincibilityPointLight");
                pointLight = pointLightObject.AddComponent<Light>();
            }

            pointLightObject.transform.position = target.transform.position; // Grab position and set it the same 
            pointLightObject.transform.SetParent(target.transform); // Make light a child for who picked it up


            // Enable the point light
            pointLight.enabled = true;

            // Set light properties
            pointLight.color = Color.red;
            pointLight.range = 10.0f;
            pointLight.intensity = 3.0f;
        }

    }

    public override void Remove(PowerupManager target)
    {
        //Health targetHealth = target.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.SetIsInvincible(false); // enable health component on Remove
        }

        // Remove visual effects here...
        if (pointLight != null)
        {
            pointLight.enabled = false;
            UnityEngine.Object.Destroy(pointLightObject); // removes from scene when unenabled
        }
    }
}
