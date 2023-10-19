using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    //Variable to hold speed value
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Vector3 math 
        transform.position = transform.position + (Vector3.up * speed);
        // The "transform.position = " part means that we are changing the value of the position variable on the transform object.
        // new vector is in the direction up ( positive Y ) and a distance of whatever value is in our speed var.

    }
}
