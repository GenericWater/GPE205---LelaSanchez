using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    // Public abstarct because this class is designed to be inherited.
    public abstract void Start();

    // holds our variables we will set on subclass scripts <3
    public abstract void Move(Vector3 direction, float speed);

    // Removed update function -- not needed.
   
}
