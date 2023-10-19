using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer1Example : MonoBehaviour
{
    public float timerDelay = 1.0f; // Sets a default value for the variable

    private float nextEventTime; // Holds value used while calculating timers

    // Start is called before the first frame update
    void Start()
    {
        nextEventTime = Time.time + timerDelay; // next time that we can perform the event is “timerDelay” seconds after “right now.”
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextEventTime)
        {
            Debug.Log("It's me!");
            nextEventTime = Time.time + timerDelay;
            // Every frame, we check if the current time is greater or equal to the time to perform the action.
            // If so, we do the action and then set the next time for the action to occur.
        }
    }
}
