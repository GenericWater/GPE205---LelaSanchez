using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask tankMask; //added to layer mask
    public ParticleSystem explosionParticles;
    public AudioSource explosionAudio;
    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float maxLifeTime = 2f;
    public float explosionRadius = 5f;


    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask); // checks colliders within an overlap sphere

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody) 
            {
                continue;
            }

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>(); // Tank Health is SpecialTankHealth script

            if (!targetHealth)
            {
                continue;
            }

            float damage = CalculateDamage(targetRigidbody.position);

            targetHealth.TakeDamage(damage);
        }

        explosionParticles.transform.parent = null; // removes shell from Tank-Parent class

        explosionParticles.Play();

        explosionAudio.Play();

        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);

        Destroy(gameObject);
    }

    
    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;  // 1 for relativeDistance would mean center - full impact | trails off damage amount near end of Radius

        float damage = relativeDistance * maxDamage;

        damage = Mathf.Max(0f, damage); // if negative sets to zero otherwise keep as is.

        return damage;
    }
    
}