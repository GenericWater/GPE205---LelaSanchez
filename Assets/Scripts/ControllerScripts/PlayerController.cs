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
                GameManager.instance.players.Add(this);
            }
        }

        // Runs the Start() function from the parent (base) class
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Process our Keyboard Inputs
        ProcessInputs(); 

        //Run the Update() function from the parent (base) class
        base.Update();

    }

    public override void ProcessInputs()
    {
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
        }

        if (Input.GetKey(moveBackwardKey)) 
        { 
            pawn.MoveBackward();
        }

        if (Input.GetKey(rotateClockwiseKey))
        {
            pawn.RotateClockwise();
        }

        if (Input.GetKey(rotateCounterClockwiseKey))
        {
            pawn.RotateCounterClockWise();
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
