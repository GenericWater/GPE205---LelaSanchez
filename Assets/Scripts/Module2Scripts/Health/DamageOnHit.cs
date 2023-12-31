using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{

    public float damageDone = 10f; // store the amount of damage it does

    [HideInInspector]public Pawn owner; // store the pawn that is responsible for the damage done

    public LayerMask tankMask; //added to layer mask
    public ParticleSystem explosionParticles;
    public AudioSource explosionAudio;
    //public float maxDamage = 50f;
    public float explosionForce = 500f;
    public float maxLifeTime = 2f;
    public float explosionRadius = 3f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("On update damage done: " + damageDone);
    }

    public void OnTriggerEnter(Collider other)
    {
        // Get the Health component from the Game Object that has the Collider we are overlapping
        //Health otherHealth = other.gameObject.GetComponent<Health>();

        // Find all tanks in an area around the shell and gamage them
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask); // checks colliders within an overlap sphere


        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Pawn>() != owner ) //only enter loop if collider is not the one who fired it
            {
                //Debug.Log("The game object DOH is on: " + colliders[i].name);
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                if (!targetRigidbody)
                {
                    continue;
                }

                targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                Health targetHealth = targetRigidbody.GetComponent<Health>();

                if (!targetHealth)
                {
                    continue;
                }

                float damageAOE = CalculateAOEDamage(targetRigidbody.position);

                targetHealth.TakeDamage(damageAOE, owner); // Owner should be who fired bullet
                //Debug.Log("Owner for collider DOH: " + owner.name + "TargetHealth is: " + targetHealth.name); 
            }
        }

        /*
        //Only damage if it has a Health component 
        if (otherHealth != null)
        {
            // Do damage
            otherHealth.TakeDamage(damageDone, owner);
            Debug.Log(damageDone);
        }

        //Destroy ourselves, whether we did damage or not
        */
        explosionParticles.transform.parent = null; // removes shell from Tank-Parent class

        explosionParticles.Play();

        explosionAudio.Play();

        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        Destroy(gameObject, explosionAudio.clip.length);
    }

    public float getDamageDone()  // Created a getter function to easily access DamageDone attribute
    {
        return damageDone;
    }

    public void setDamageDone(float damage) // Created a Set function to easily access DamageDone attribute
    {
        damageDone = damage;
    }

    private float CalculateAOEDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;  // 1 for relativeDistance would mean center - full impact | trails off damage amount near end of Radius

        float damage = relativeDistance * damageDone; // changed from maxDamage

        //damage = Mathf.Max(0f, damage); // if negative sets to zero otherwise keep as is.

        damage = Mathf.Abs(damage); // Only will return absolute values! (+)

        return damage;
    }
}
