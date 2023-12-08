using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Health : MonoBehaviour
{
    public float currentHealth;  // Variable that holds current health Float value
    public float maxHealth;  // Variable that holds maximum health Float value

    //Module 3:
    public bool isInvincible {  get; private set; } // private setter ensures that you can only change the value of isInvincible from within the script

    public GameObject explosionPrefab;
    private AudioSource explosionAudio;
    private ParticleSystem explosionParticles;

    private Pawn ownerPawn; // Refrence to the tank pawn script - inherits from Pawn script

    // Module 2: 
    protected bool IsDead;

    //public Transform respawnCheckpoint; //Stores respawn position


    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();

        explosionParticles.gameObject.SetActive(false);
        ownerPawn = GetComponent<Pawn>(); // Initialize the tankPawn variable for use
    }
    // Start is called before the first frame update
    void Start()
    {
        // Sets current health to Max
        currentHealth = maxHealth;
        IsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount, Pawn source) // Source is who attacked; the gameObject is who got hit
    {
        currentHealth = currentHealth - amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // the Mathf.Clamp keeps our currentHealth from going out of range ( below 0, or above maxHealth) 

       //Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);
        //Debug.Log(gameObject.name + " Current Health is: " + currentHealth);

        if (currentHealth <= 0) 
        {
            IsDead = true;
            LoseLife(source);
           
        }
    }



    public void ExplosionEffects()
    {
            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);

            explosionParticles.Play();

            explosionAudio.Play();
    }

    

 
    public void LoseLife(Pawn source)
    {
        ExplosionEffects();
        //SetInvisible(); // Call to custom function

        if (gameObject.CompareTag("Player")) 
        {
            Debug.Log("Owner pawn lost life" + ownerPawn.name);
            ownerPawn.currentLives--;  // Checks tankPawn script currentLives variable value and lowers it by one
            currentHealth = maxHealth;

            //Debug.Log("Lower life count by one, Current Lives: " + ownerPawn.currentLives + " | Owner of current lives: " + ownerPawn.name);

            if (ownerPawn.currentLives > 0)
            {
                GameManager.instance.Respawn(ownerPawn.controller, ownerPawn); // Attached Controller for Respawn target
            }

            if (ownerPawn.currentLives <= 0)
            {
                ownerPawn.Die(source); // Die() defined on pawn script - TankPawn
            }

            IsDead = false;
        }else // ensures no weird execution errors
        {
            ownerPawn.Die(source); // Die() defined on pawn script - TankPawn
        }


    }

    /*
    public void LoseLife()
    {
        ownerPawn.currentLives--;  // Checks tankPawn script currentLives variable value and lowers it by one
        Debug.Log("Lower life count by one, Current Lives: " + ownerPawn.currentLives);

        // TODO: instantiate explosion, call new spawn point (from random generated spawns), wait 4 sconds before respawn, make GameObject invisible, after Respawn make GameObject visible again
        ExplosionEffects();
        SetInvisible(); // Call to custom function
        // Removed LoseLife
        if (ownerPawn.currentLives <= 0) // If tankPawn current lives are < or = to 0... | Can add AITank life and respawn too??
        {
            //Destroy(gameObject, 1); // destroy the Game Object that this component instance is on after 1 second
            ownerPawn.Die(ownerPawn);

            // only through UI can you call the GameManager for respawn
        }
    }
    */

    /*
    public void SetInvisible() // Anthing that needs to be turned off on Death
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<TankShooter>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false; // Rigidbody is kinematic should be checked later...
    }

    public void SetVisible() // Anthing that needs to be turned back on after respawn
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.GetComponent<TankShooter>().enabled = true;
        this.gameObject.GetComponent<Collider>().enabled = true; // Rigidbody is kinematic should be checked later...
    }

    */


    public void GainLife()
    {
        ownerPawn.currentLives++; // Will add one life; used for pickup - ExtraLifePickup!
    }
   
    public void Heal(float amount, Pawn source) 
    {
        currentHealth = currentHealth + amount;
        //Debug.Log(source.name + "healed amount: " + amount + " to Target: " + gameObject.name);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Keeps current health between 0 and maxHealth // Can add overhealing if wanted later on...
    }

    public void SetIsInvincible (bool invincible) // Tried to do this for invincibility Powerup/pickup
    {
        isInvincible = invincible;
        if (isInvincible)
        {
            // Disable the Health component when invincible
            this.enabled = false;
        }
        else
        {
            // Re-enable the Health component when NOT invincible
            this.enabled = true;
        }
    }
}
