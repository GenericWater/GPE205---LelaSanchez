using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer2Example : MonoBehaviour
{
    //UTILIZES COUNT-DOWN METHOD

    public float timerDelay = 1.0f;

    private float timeUntilNextEvent;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextEvent = timerDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilNextEvent -= Time.deltaTime;
        if (timeUntilNextEvent <= 0)
        {
            Debug.Log("It's me!!");
            timeUntilNextEvent = timerDelay;
        }
    }
}
