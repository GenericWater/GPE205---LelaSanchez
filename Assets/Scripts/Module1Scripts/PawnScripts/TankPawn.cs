using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// changed : Monobehavior to Pawn Class (Script I made)
public class TankPawn : Pawn
{
    public bool IsSilent;
    // it will inherit our two speed variables. and fire rate


    //  private float nextShootTime; // Used this variable to set time delay between fireRate

    //public GameObject Bullet;  // Get rid of
    //public Transform Barrele; // Get rid of
    //public ScorePowerup scorePowerup;
    public float scoreToAddForDeath = 100.0f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // nextShootTime = Time.time + fireRate;

        // Module 2 - loading shooter on start
        //shooter = GetComponent<Shooter>();
        currentLives = maxLives;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if ( IsSilent && noiseMaker.volumeDistance > 0)
        {
            noiseMaker.volumeDistance -= Time.deltaTime; // Gradual decrease of noise on Stop Moving / StopNoise
            
        }

        /*
        if (Time.time >= nextShootTime)
        {
            Debug.Log("It's time to FIRE!");
            nextShootTime = Time.time + fireRate;
        }
        */

        /*
        if (Input.GetButtonDown("Fire1") && Time.time >= nextShootTime)  // Only works when on UATank (Left click or left Ctrl will cause "Fire"
        {
            Debug.Log("FIRE");

            GameObject myBullet = Instantiate(Bullet, Barrele.position, Barrele.rotation); //Made a refrence

            myBullet.GetComponent<Rigidbody>().AddForce(myBullet.transform.forward * 20, ForceMode.Impulse);  // Makes bullet fire

            nextShootTime = Time.time + fireRate; // Add to time
        }
        */
    }

    public override void MoveForward()
    {
        //Debug.Log("Move Forward");
        if (mover != null)
        {
            mover.Move(transform.forward, moveSpeed); // will move tank forward | moveSpeed is from Pawn Script Variable
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.MoveForward()!");
        }
        
    }

    public override void MoveBackward()
    {
        //Debug.Log("Move Backward");
        if (mover != null)
        {
            mover.Move(transform.forward, -moveSpeed); // will move tank in reverse
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.MoveBackward()!");
        }
        
    }

    public override void RotateClockwise()
    {
        //Debug.Log("Rotate Clockwise");
        if (mover != null)
        {
            mover.Rotate(turnSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.RotateClockwise()!");
        }
        
    }

    public override void RotateCounterClockWise()
    {
        //Debug.Log("Rotate Counter-Clockwise");
        if (mover != null)
        {
            mover.Rotate(-turnSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.RotateCounterClockwise()!");
        }
    }

    // Module 2: implimenting inherited abstract member Pawn.Shoot()
    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifespan); // Refrences Parent's shooter class (Pawn) and tankShooter
    }

    // Module 2: implimenting RotateTowards function
    public override void RotateTowards(Vector3 targetPosition)
    {
        // Find the vector to our target
        Vector3 vectorToTarget = targetPosition - transform.position;  // transform.position is Enemy Pawn's current position
        // Find the rotation to look down taht vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        // Rotate closer to that vector, but don't rotate more than our turn speed allows in one frame
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }


    

    // Module 2: Implimenting Make/Stop noise functions to be used on AIController
    public override void MakeNoise()
    {
        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = noiseMakerVolume;
            IsSilent = false;
        }
    }
    public override void StopNoise()
    {
        //noiseMaker.volumeDistance = 0;
        IsSilent = true;
    }

    public override void Die(Pawn source) 
    {
        Destroy(gameObject, 2); // Destroys with a 2 second delay
        Debug.Log(gameObject.name + " was killed by: " + source.name);

        //Module 4: on death will add to score
        source.controller.AddToScore(scoreToAddForDeath); // Add to score based on amount defined in inspector
    }


}
