using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// changed : Monobehavior to Pawn Class (Script I made)
public class TankPawn : Pawn
{
    // it will inherit our two speed variables.
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Start();
    }

    public override void MoveForward()
    {
        Debug.Log("Move Forward");
    }

    public override void MoveBackward()
    {
        Debug.Log("Move Backward");
    }

    public override void RotateClockwise()
    {
        Debug.Log("Rotate Clockwise");
    }

    public override void RotateCounterClockWise()
    {
        Debug.Log("Rotate Counter-Clockwise");
    }
}
