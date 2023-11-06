using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChildCrazyPersonality : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // Runs base class Start()
        ChangeState(AIState.Guard);
    }

    // Update is called once per frame
    public override void Update()
    {
        //base.Update(); // will run process Inputs script
        ProcessInputs();
    }

    public override void ProcessInputs()
    {
        Debug.Log("Making Decisions from Crazy Personality");

        switch (currentState)  // Add case to switch current state
        {
            case AIState.Guard:
                // Do work for Guard state
                base.DoGuardState();
                TargetPlayerOne(); // Target player one
                // Check for transitions
                if (base.IsCanHear(target))
                {
                    base.ChangeState(AIState.Scan);
                }
                if (!base.IsCanHear(target))
                {
                    TargetNearestTank();
                }
                break;
            case AIState.Scan:
                // Do work for Scan state:
                base.DoScanState();
                // Check for transitions
                if (IsCanSee(target))
                {
                    base.ChangeState(AIState.Chase); // ************** OVERRIDE CHASE FUNCTION TO INCREASE PAWNS SPEED BY 50% AS WELL
                }
                break;
            case AIState.Chase:
                // Do work for Chase
                DoChaseState();
                // Check for transitions
                if (IsDistanceLessThan(target, 5))
                {
                    base.ChangeState(AIState.Attack);
                }
                if (IsDistanceGreaterThan(target, 10))
                {
                    base.ChangeState(AIState.Scan);
                }
                break;
            case AIState.Attack:
                // Do work for Attack state
                base.DoAttackState();
                // Check for transitions
                if (IsDistanceGreaterThan(target, 10))
                {
                    base.ChangeState(AIState.Chase);
                }
                if (IsHealthBelow(50))
                {
                    base.ChangeState(AIState.Aggressive);
                }
                break;
            case AIState.Aggressive:
                // Do work for Aggressive State
                base.DoAggressiveState();
                //Check for transitions
                if (IsHealthBelow(10))
                {
                    base.ChangeState(AIState.Unpredictable);
                }
                break;
            case AIState.Unpredictable:
                // Do work for Unpredictable State
                base.DoUnpredictableState();
                // Check for transitions
                if (IsDistanceGreaterThan(target, 10))
                {
                    base.ChangeState(AIState.Chase);
                }
                if (IsHasTimePassed(3))
                {
                    base.ChangeState(AIState.Attack);
                }
                break;
        }
    }
    protected override void DoChaseState()  // In lessons it is called Seek State // Made virtual to override on AIChildCrazyPersonality script.
    {
        // Doing Chase State
        //TargetPlayerOne(); // Target Player one from Base class
        Debug.Log("Chasing from Crazy AI Personality");
        Seek(target);
        pawn.moveSpeed *= 1.0f; // Move speed up by 25%
    }

}