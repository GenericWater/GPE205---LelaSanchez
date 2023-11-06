using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChildDominantPersonality : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // Runs base class Start()
        ChangeState(AIState.Scan);
        
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs();
    }

    public override void ProcessInputs()
    {
        Debug.Log("Making Decisions from the Dominant Personality!");

        switch (currentState)  // Add case to switch current state
        {
            case AIState.Scan:
                // Do work for Scan state:
                base.DoScanState();
                TargetNearestTank();
                // Check for transitions
                if (IsCanSee(target) && IsDistanceLessThan(target, 5))
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
                base.DoAggressiveState();
                //Check for transitions
                if (!IsCanSee(target))
                {
                    base.ChangeState(AIState.Scan);
                }
                if (IsHasTimePassed(10))
                {
                    base.ChangeState(AIState.Unpredictable);
                }
                if (IsDistanceLessThan(target, 3))
                {
                    base.ChangeState(AIState.Flee);
                }
                if (IsHealthBelow(50))
                {
                    base.ChangeState(AIState.Flee);
                }
                break;
            case AIState.Unpredictable:
                // Do work for Unpredictable State
                base.DoUnpredictableState();
                // Check for transitions
                if (IsHasTimePassed(5))
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


}
