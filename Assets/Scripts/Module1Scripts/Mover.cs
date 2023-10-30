using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public float turnSpeed; // Created a variable for the degrees we will rotate in a frame draw.
    public float moveSpeed; // Created a public variable for move speed - will be framereate independent.


    // Public abstarct because this class is designed to be inherited.
    public abstract void Start();

    // holds our variables we will set on subclass scripts <3
    public abstract void Move(Vector3 direction, float speed);
    //internal abstract void rotate(float turnSpeed);

    // Removed update function -- not needed.

    public abstract void Rotate(float rotationSpeed);

}
