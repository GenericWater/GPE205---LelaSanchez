using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// changed : Monobehavior to Pawn Class (Script I made)
public class TankPawn : Pawn
{
    // it will inherit our two speed variables. and fire rate
    // Start is called before the first frame update

    private float nextShootTime; // Used this variable to set time delay between fireRate

    public GameObject Bullet;
    public Transform Barrele; 

    public override void Start()
    {
        base.Start();
        nextShootTime = Time.time + fireRate;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Start();
        /*
        if (Time.time >= nextShootTime)
        {
            Debug.Log("It's time to FIRE!");
            nextShootTime = Time.time + fireRate;
        }
        */

        if (Input.GetButtonDown("Fire1") && Time.time >= nextShootTime)
        {
            Debug.Log("FIRE");

            GameObject myBullet = Instantiate(Bullet, Barrele.position, Barrele.rotation); //Made a refrence

            myBullet.GetComponent<Rigidbody>().AddForce(myBullet.transform.forward * 20, ForceMode.Impulse);  // Makes bullet fire

            nextShootTime = Time.time + fireRate; // Add to time
        }
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


}
