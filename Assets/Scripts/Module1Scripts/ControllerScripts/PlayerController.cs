using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;

    //Module 2 - shootKey added!
    public KeyCode shootKey;

    public KeyCode respawnKey;

    // Start is called before the first frame update
    public override void Start()
    {
        // If we have a GameManager
        if (GameManager.instance != null)
        {
            // And it tracks the player(s)
            if (GameManager.instance.players != null)
            {
                // Register with the GameManager
                GameManager.instance.players.Add(this); // Adds player to game manager list
            }
        }

        // Runs the Start() function from the parent (base) class
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        
        // Process our Keyboard Inputs
        ProcessInputs(); // Only processes Tank Controls 
        
        

        //Run the Update() function from the parent (base) class
        base.Update();

        if (Input.GetKeyDown(respawnKey)) // REMOVE LATER
        {
            GameManager.instance.RestartPlayer(this); // Restart player
        }

    }

    public override void ProcessInputs()
    {
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
            // Module 2: MakeNoise function added to Inputs
            pawn.MakeNoise();
        }

        if (Input.GetKey(moveBackwardKey)) 
        { 
            pawn.MoveBackward();
            // Module 2: MakeNoise function added to Inputs
            pawn.MakeNoise();
        }

        if (Input.GetKey(rotateClockwiseKey))
        {
            pawn.RotateClockwise();
            // Module 2: MakeNoise function added to Inputs
            pawn.MakeNoise();
        }

        if (Input.GetKey(rotateCounterClockwiseKey))
        {
            pawn.RotateCounterClockWise();
            // Module 2: MakeNoise function added to Inputs
            pawn.MakeNoise();
        }

        //Module 2 Reading Input for shootKey

        if (Input.GetKeyDown(shootKey))
        {
            pawn.Shoot();            
            // Module 2: MakeNoise function added to Inputs
            pawn.MakeNoise();
        }

        // If no Input keys are being pressed stop making noise
        if (!Input.GetKeyDown(moveForwardKey) && !Input.GetKeyDown(moveBackwardKey) && !Input.GetKeyDown(rotateClockwiseKey) && !Input.GetKeyDown(rotateCounterClockwiseKey) && !Input.GetKeyDown(shootKey))
        {
            pawn.StopNoise();
        }


    }



    public void OnDestroy()
    {
        // If we have a GameManager 
        if (GameManager.instance != null)
        {
            // And it tracks the player(s)
            // Deregister with the GameManager
            GameManager.instance.players.Remove(this);
        }
    }
}
