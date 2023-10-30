using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;  // Variable that holds current health Float value
    public float maxHealth;  // Variable that holds maximum health Float value

    public GameObject explosionPrefab;
    private AudioSource explosionAudio;
    private ParticleSystem explosionParticles;

    private Pawn tankPawn; // Refrence to the tank pawn script - inherits from Pawn script

    // Module 2: 
    private bool IsDead;

    public Transform respawnCheckpoint; //Stores respawn position


    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();

        explosionParticles.gameObject.SetActive(false);
        tankPawn = GetComponent<Pawn>(); // Initialize the tankPawn variable for use
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

    public void TakeDamage(float amount, Pawn source) // Made virtual to override in SpecialTankHealth script
    {
        currentHealth = currentHealth - amount;
        Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);
        Debug.Log("Current Health is: " + currentHealth);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // the Mathf.Clamp keeps our currentHealth from going out of range ( below 0, or above maxHealth) 

        if (currentHealth <= 0 && !IsDead)  // Checks for death when health is less than or = to zero
        {
            Debug.Log(source + " died! ");
            Die(source);
        }
    }

    public void Die(Pawn source)  // Could reduce lfe count here?
    {
        IsDead = true;
        if (tankPawn.currentLives <= 0) // If tankPawn current lives are < or = to 0...
        {
            Destroy(gameObject); // destroy the Game Object that this component instance is on

            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);

            explosionParticles.Play();

            explosionAudio.Play();
        }
        else
        {
            Destroy(gameObject);
            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);

            explosionParticles.Play();

            explosionAudio.Play();
            LoseLife(); // If lives are not equal to 0 on Death, we will do the same but LoseLife is run.
            Respawn(); // Respawn player at transform !
        }

    }

    public void Respawn()
    {
        if (tankPawn != null && respawnCheckpoint != null)
        {
            //Reset the tank Pawns position to the checkpoint position
            tankPawn.transform.position = respawnCheckpoint.position;

            //Reset Health back to full
            currentHealth = maxHealth;
        }
    }

    public void LoseLife()
    {
        tankPawn.currentLives--;  // Checks tankPawn script currentLives variable value and lowers it by one
        Debug.Log("Lower life count by one, Current Lives: " + tankPawn.currentLives);
    }

    public void Heal(float amount, Pawn source) 
    {
        currentHealth = currentHealth + amount;
        Debug.Log(source.name + "healed amount: " + amount + " to Target: " + gameObject.name);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
