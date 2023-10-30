using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChildDominantPersonality : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        ChangeState(AIState.Scan);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs()
    {
        Debug.Log("Making Decisions from the Dominant Personality!");

        switch (currentState)  // Add case to switch current state
        {
            case AIState.Scan:
                // Do work for Scan state:
                base.DoScanState();
                // Check for transitions
                if (IsCanSee(target) && IsDistanceLessThan(target, 7))
                {
                    base.ChangeState(AIState.Aggressive); 
                }
                if (IsDistanceLessThan(target, 3))
                {
                    base.ChangeState(AIState.Flee);
                }
                if (IsHasTimePassed(7))
                {
                    base.ChangeState(AIState.Unpredictable);
                }
                break;
            case AIState.Aggressive:
                // Do work for Aggressive State
                DoAggressiveState();
                //Check for transitions
                if (!IsCanSee(target))
                {
                    base.ChangeState(AIState.Scan);
                }
                if (IsHasTimePassed(3))
                {
                    base.ChangeState(AIState.Unpredictable);
                }
                if (IsDistanceLessThan(target, 3))
                {
                    base.ChangeState(AIState.Flee);
                }
                break;
            case AIState.Unpredictable:
                // Do work for Unpredictable State
                base.DoUnpredictableState();
                // Check for transitions
                if (IsHasTimePassed(3))
                {
                    base.ChangeState(AIState.Flee);
                }
                if (IsCanHear(target))
                {
                    base.ChangeState(AIState.Scan);
                }
                break;
            case AIState.Flee:
                // Do work for Flee State
                base.DoFleeState();
                // Check for transitions
                if (IsHasTimePassed(3))
                {
                    base.ChangeState(AIState.Unpredictable);
                }
                if (IsHealthBelow(50))
                {
                    base.ChangeState(AIState.Scan);
                }
                if (IsDistanceGreaterThan(target, 7))
                {
                    base.ChangeState(AIState.Aggressive);
                }
                break;
        }
    }

    protected override void DoAggressiveState()
    {
        TargetPlayerOne();

        pawn.moveSpeed *= 1.5f; // 50% increase in movement speed

        //pawn.fireRate = 0; // Unlimited firing rate

        tankShooterComponent.nextShootTime = 0; // sets fire rate to 0 | umlimited shooting

        Seek(target);  // Seek the player

        Shoot(); // Shoot 
    }
}
