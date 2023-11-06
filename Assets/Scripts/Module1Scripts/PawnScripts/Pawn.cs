using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    // Variable for move speed
    public float moveSpeed;
    // Variable for turn speed
    public float turnSpeed;

    // Variable for Rate of Fire
    public float fireRate = 1.0f;

    // Module 2: Variable for our shell prefab
    public GameObject shellPrefab;

    // Module 2: Variable for our firing force
    public float fireForce;

    // Module 2: Variable for our Damage Done
    public float damageDone;

    // Module 2: Variable for how long our bullets survive if they don't collide
    public float shellLifespan;

    // Module 2: Variable for volume of our NoiseMaker | Updates volumeDistance
    public float noiseMakerVolume;

    public Mover mover; // Mod 1: variable to hold our Mover that calls from the Mover class script

    // Module 2 : Variable to hold our Shooter
    public Shooter shooter;

    // Module 2: Variable to hold noiseMaker
    public NoiseMaker noiseMaker;

    public int maxLives = 3; // Maximum number of lives

    public int currentLives; // Holds current number of lives 

    // Start is called before the first frame update
    public virtual void Start() // Virtual allows to be called on sub-scripts
    {
        mover = GetComponent<Mover>();

        // Module 2: associate variable for shooter with component already attached to object pawn is associated with
        shooter = GetComponent<Shooter>();

        // Module 2: Associate variable for noiseMaker with component already attached to the pawn 
        noiseMaker = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterClockWise();

    // Module 2: Shoot function that can be called / overridden
    public abstract void Shoot();

    // Module 2: Will cause AI to rotate towards a position we send it. 
    public abstract void RotateTowards(Vector3 targetPosition);

    // Module 2: Make and stop noise functions created 
    public abstract void MakeNoise();
    public abstract void StopNoise();

    public abstract void Die(Pawn soucre);

}
