using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float heightAboveTank = 50.0f; // The height above target tank
    private Transform target; // Refrenece tp the target object to follow (TankPawn)
    private bool isTargetSet = false; // Track whether the target is set or not

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isTargetSet = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargetSet &&  target != null)
        {
            Vector3 newPosition = target.position;
            newPosition.y = heightAboveTank; // Set the camera's Y position using the variable
            transform.position = newPosition;
        }
    }
}
