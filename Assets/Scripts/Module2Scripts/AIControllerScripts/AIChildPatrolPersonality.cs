using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChildPatrolPersonality : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // Runs base class Start()
        ChangeState(AIState.Patrol);
    }

    // Update is called once per frame
    public override void Update()
    {
        //base.Update(); // will run process Inputs script
        ProcessInputs();
    }

    public override void ProcessInputs()
    {
        Debug.Log("Making Decisoions from Patrol Personality");

        switch (currentState)  // Add case to switch current state
        {
            case AIState.Patrol:
                // Do work for Patrol state
                base.DoPatrolState();
                TargetNearestTank();
                // Check for transitions
                if (base.IsCanSee(target))
                {
                    base.ChangeState(AIState.Chase);
                }
                break;
            case AIState.Chase:
                // Do work for Chase state
                base.DoChaseState();
                // Check for transitions
                if (base.IsDistanceLessThan(target, 6) && base.IsCanSee(target))
                {
                    base.ChangeState(AIState.Attack);
                }
                break;
            case AIState.Attack:
                // Do work for attack state
                DoAttackState();
                // Check for transitions
                if (base.IsDistanceLessThan(target, 4))
                {
                    base.ChangeState(AIState.Flee);
                }
                if (base.IsDistanceGreaterThan(target, 8))
                {
                    base.ChangeState((AIState.Chase));
                }
                break;
            case AIState.Flee:
                // Do work for flee state
                base.DoFleeState();
                base.TargetNearestTank(); 
                // Check for transitions
                if (!base.IsCanSee(target) && base.IsDistanceGreaterThan(target, 10))
                {
                    base.ChangeState(AIState.Chase);
                }
                break;
        }
    }

    public override void DoAttackState()
    {
        if (base.IsHasTarget())
        {
            base.Shoot();
        }
        else
        {
            TargetPlayerOne();
        }
    }
}
