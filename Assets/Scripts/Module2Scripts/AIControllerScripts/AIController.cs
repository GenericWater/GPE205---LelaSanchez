using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller // Generic AIController Personality
{
    // Has access to Pawn variable since inheritied from Controller
    public enum AIState { Guard, Chase, Attack, Patrol, Flee, Unpredictable, Aggressive, Scan }; // Add to this list to have more AI states...
    public AIState currentState; // Variable to call an enum type from list! ^

    private float lastStateChangeTime; // use time as an event to determine if a state should be updated || Can be implimented with one of the Timers from earlier lessons ******

    public GameObject target; // Target for AI to "Chase" or "Seek"

    public Transform[] waypoints; // Accessable array from editor for developers to plug in their own waypoint transforms
    public float waypointStopDistance; // Indicates how close we are to waypoint

    private int currentWaypoint = 0; // Going to store index that is pointing to a specific transform component in waypoints[] (public array)

    public float hearingDistance;

    public float fieldOfView;

    public float fleeDistance; // Used for Flee

    public float rotationSpeed = 250.0f; // Used with DoUnpredictableState();

    private float startTime; // Holds starting time in game

    public float timeThreshold = 3.0f; // Amount of time in seconds

    public Health healthComponent; // Reference to Health component script

    public float healthThreshold = 30.0f; // Health threshold to check 

    public TankShooter tankShooterComponent; // Refrence to TankShooter script to edit timer for nextShootTime

    // Start is called before the first frame update
    public override void Start()
    {
        //currentState = AIState.Chase;  // Sets inital state to be "Chase" State || NOT USED ANYMORE
        //ChangeState(AIState.Chase); // set to chase just for testing | should be guard or Idle state
        //ChangeState(AIState.Patrol);
        ChangeState(AIState.Guard);
        //ChangeState(AIState.Flee);
        //ChangeState(AIState.Unpredictable);
        // Run the parent (base) Start
        base.Start();
        startTime = Time.time; // set the startTime variable on start
        tankShooterComponent = pawn.GetComponent<TankShooter>(); 
        healthComponent = pawn.GetComponent<Health>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //Make decisions
        ProcessInputs();
        // Run the parent (base) Update
        base.Update();
    }


    // Going to be responsible for making AI decisions
    public override void ProcessInputs()
    {
        Debug.Log("Making Decisions from base AIController");

        if (pawn == null) // If pawn does not exist, destroy gameObject(Controller)
        {
            Destroy(gameObject); 
        }

        switch (currentState)  // Add case to switch current state
        {
            case AIState.Guard:
                // Do work for Guard
                DoGuardState();
                TargetNearestTank();
                //TargetPlayerOne();
                // Check for transitions
                if(IsDistanceLessThan(target, 5) && IsCanSee(target))
                 {
                   ChangeState(AIState.Chase);
                 }
                if(IsHasTimePassed(5))
                {
                    //ChangeState(AIState.Unpredictable);
                }
                break;
            case AIState.Chase:
                // Do work for Chase
                    DoChaseState();
                //Check for transitions
                if(IsDistanceLessThan(target, 5))
                {
                    ChangeState(AIState.Attack);
                }
                if(!IsDistanceLessThan(target, 10)) // If distance is not greater than 10 m, go back to guard state
                {
                    ChangeState(AIState.Guard);
                }
                break;
            case AIState.Attack:
                // Do work for Attack
                if (IsCanSee(target))
                {
                    DoAttackState();
                }
                else
                {
                    ChangeState(AIState.Guard);
                }
                //Check for transitions
                if(!IsDistanceLessThan(target, 7)) // Go back to chase state if distance is not less than 7 m
                {
                    ChangeState(AIState.Chase);
                }
                if (IsDistanceLessThan(target, 2) || IsHealthBelow(25)) // OR statement
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Flee:
                // Do work for Flee
                DoFleeState();
                //Check for transitions
                if(IsDistanceGreaterThan(target, 8)) // If health is lower than 25% Flee - change later
                {
                    ChangeState(AIState.Chase);  //
                }
                break;
            case AIState.Patrol:  // Not calling to this State currently from this script - used for testing.
                // Do work for Patrol
                DoPatrolState();
                // Check for transitions out of state - Fill in later - Add can hear player change to Guard and rotate
                if (IsCanHear(target))
                {
                    DoGuardState();
                }
                break;
            case AIState.Unpredictable:
                // Do work for Unpredictable
                DoUnpredictableState();
                // Check for transitions
                if (IsHasTimePassed(2)) // If 2 seconds have passed .. 
                {
                    ChangeState(AIState.Chase);
                }
                break;
        }
    }

    // Patrol State
    protected void DoPatrolState()
    {
        // If we have enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint) // As long as there are elements in our list...
        {
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);

            //If we are close enough, then increment to the next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) < waypointStopDistance) // Pawn is stored in Controller Script
            {
                currentWaypoint++; //Updates to next waypoint in list/array
            }
        }
        else // Restart Patrol at end of list || Insert isLooping inplace of else statement put another if ????
        {
            RestartPatrol();
            TargetPlayerOne();
        }
        
    }

    protected void RestartPatrol()
    {
        // Set the index back to 0
        currentWaypoint = 0;
    }

    //Guard State
    protected void DoGuardState() 
    {
        // Doing Guard State
        //TargetNearestTank();
        //Debug.Log("Guarding");
    }

    // Chase State
    protected virtual void DoChaseState()  // In lessons it is called Seek State // Made virtual to override on AIChildCrazyPersonality script.
    {
        // Doing Chase State
        //Debug.Log("Chasing");
        Seek(target);
        //TargetPlayerOne();
    }

    // Very basic Flee function | hard to manipulate
    protected void DoFleeState() // will choose a point that is away from our target and move towards that point 
    {
        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;

        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;

        //float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        
        //float percentOfFleeDistance = targetDistance / fleeDistance;

        //percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        //float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;
        //flippedPercentOfFleeDistance = flippedPercentOfFleeDistance * fleeDistance;

        //Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector ); //* flippedPercentOfFleeDistance
    }

    protected void DoUnpredictableState()
    {
        //TargetNearestTank();
        if (pawn == null)
        {
            return; // just leave if no pawn
        }
        // Rotate AI counter-clockwise using the rotationSpeed variable
        pawn.transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);

        // Fire bullets
        pawn.fireRate = 0.5f; // sets fire rate to 0 | umlimited shooting
        
        tankShooterComponent.nextShootTime = 1.0f; 
        Shoot();
        //TargetPlayerOne();
        
    }

    protected void DoScanState()
    {
        TargetNearestTank();
        // Rotate the pawn clockwise
        //pawn.RotateClockwise();
        pawn.transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);

    }

    protected virtual void DoAggressiveState()
    {
        //TargetNearestTank();
        //TargetPlayerOne();

        pawn.moveSpeed *= 1.5f; // 50% increase in movement speed

        pawn.fireRate = 0.8f; // Unlimited firing rate

        //tankShooterComponent.nextShootTime = 0; // sets fire rate to 0 | umlimited shooting

        Seek(target);  // Seek the player

        Shoot(); // Shoot 
    }

    public void Flee() // Not used just done in lesson
    { 

    }
    

    public void Seek(GameObject target)
    {
        // Do Seek
        //RotateTowards the target
        pawn.RotateTowards(target.transform.position);

        // Move forward towards the target
        pawn.MoveForward();
    }

    public void Seek(Transform targetTransform) // Overloading our Seek function!!
    {
        // Seek position of our target Transform
        Seek(targetTransform.gameObject);  //Gets transform component of gameObject
    }

    public void Seek(Vector3 targetPosition)
    {
        //RotateTowards the Function
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }

    // Attack State
    public virtual void DoAttackState() // Marked virtual to override on AIChildPatrolPersonality script
    {
        // Chase
        Seek(target);
        // Shoot
        Shoot();
        pawn.fireRate = 1; // Unlimited firing rate

        tankShooterComponent.nextShootTime = 1; // sets fire rate to 0 | umlimited shooting

        //TargetNearestTank(); // Added this
    }

    public void Shoot() // Define Shoot function
    {
        pawn.Shoot(); // Calls definition from Pawn script
    }

    // Transition - can be used in other states
    protected bool IsDistanceLessThan (GameObject target, float distance) // protected means only this class and children of this class can access this function
    {
        if (Vector3.Distance (pawn.transform.position, target.transform.position) < distance) // Vector3.Distance returns the distance between a and b. 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool IsDistanceGreaterThan (GameObject target, float distance)
    {
        if (Vector3.Distance (pawn.transform.position, target.transform.position) > distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void ChangeState(AIState newState) // made virtual to have different variations depending on specific child of AIController
    {
        // Change the current state
        currentState = newState;

        //Save the time when we changed states
        lastStateChangeTime = Time.time; // Time.time grabs current time since the game started and stores it here
    }

    public void TargetPlayerOne()
    {
        // If the GameManager exists | Singleton pattern
        if(GameManager.instance != null)
        {
            // And the array of players exist
            if(GameManager.instance.players != null)
            {
                // And there are players in it
                if(GameManager.instance.players.Count > 0)
                {
                    // Then target the gameObject of the pawn of the first player controller in the list | Assumes fist player is person playing game
                    target = GameManager.instance.players[0].pawn.gameObject;
                }
            }
        }
    }

    protected void TargetNearestTank()
    {
        // Get a list of all the tanks (pawns)
        Pawn[] allTanks = FindObjectsOfType<Pawn>(); // can be changed to tankPawn if wanting to be more specific

        //Assume the first tank is the closest tank
        //Pawn closestTank = allTanks[0];
        //float closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);

        Pawn closestTank = null;
        float closestTankDistance = float.MaxValue; // Initialize with a large value


        //Iterate through them one at a time
        foreach (Pawn tank in allTanks)
        {
            //If this one is closer than the closest
            /*if (Vector3.Distance(pawn.transform.position, tank.transform.position) <= closestTankDistance)
            {
                // It is the closest tank
                closestTank = tank;
                closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
            }
            */

            // Skip the AI's own pawn (if it exists) by comparing GameObjects
            if (tank.gameObject == pawn.gameObject)
            {
                continue;
            }

            // Calculate the distance to this tank
            float distance = Vector3.Distance(pawn.transform.position, tank.transform.position);

            // If this tank is closer than the closest one found so far...
            if (distance < closestTankDistance)
            {
                closestTank = tank;
                closestTankDistance = distance;
            }
        }

        if (closestTank != null)
        {
            //Target the closest Tank
            target = closestTank.gameObject;
        }
        
    }

    protected bool IsHasTarget() // Transition - can be used in other states
    {
        // Return true if we have a target, false if we don't
        return (target != null); // logical expression that evaluates to a boolean
    }

    protected bool IsCanHear(GameObject target)
    {
        // Get the target's noiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        // If they don't have one, can't make noise, return false
        if(noiseMaker == null)
        {
            return false;
        }
        // If they are making 0 noise, also can not be heard
        if(noiseMaker.volumeDistance <= 0)
        {
            return false;
        }

        // If they are making noise, add the volumeDistance in the noiseMaker to the hearingDistance of this AI 
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        // If the distance beween our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            return true;
        }
        else
        {
            // Otherwise, too far away to hear them
            return false;
        }
    }

    protected bool IsCanSee(GameObject target) // Impliment raycast going down agentToTargetVector to check if anything intersects other than the player
    {
        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;


        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);
       // Debug.Log("Angle To Target: " + angleToTarget);

        // If angle is less than out field of view
        if (angleToTarget < fieldOfView)
        {
            //Debug.Log("In Field Of View!");
            // Calculate the direction from the AI to the target
            Vector3 directionToTarget = target.transform.position - pawn.transform.position;

            // Create a ray from the AI's position in the direction of target
            Ray ray = new Ray(pawn.transform.position, directionToTarget);

            // Set the max distance for the ray (adjust this value based on game's scale)
            float maxRayDistance = 100f;

            // Create a layer mask to filter which objects the ray should hit (Uses Player Layer)
            LayerMask layerMask = LayerMask.GetMask("Players");

            RaycastHit hitInfo;

            // Perform the raycast
            if (Physics.Raycast(ray, out hitInfo, maxRayDistance, layerMask))
            {
                // A player object was hit, so the AI can see the target
                return true;
            }
            // No player objects were hit, so the AI cannot see the target
            return false;
        }
        else
        {
            return false;
        }
    }

    protected bool IsHasTimePassed(float timeThreshold)
    {
        return Time.time - startTime >= timeThreshold;
    }

    protected bool IsHealthBelow(float healthThreshold)
    {
        //currentHealth = Health.
        return healthComponent.currentHealth < healthThreshold; // Use variable refrence to check health component then return true if under the specified threshold
    }

}
