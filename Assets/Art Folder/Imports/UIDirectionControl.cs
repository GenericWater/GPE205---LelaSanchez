using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool useRelativeRotation = true;

    private Quaternion relativeRotation;

    // Start is called before the first frame update
    private void Start()
    {
        relativeRotation = transform.parent.localRotation; // finds local rotation of canvas using .parent
    }

    // Update is called once per frame
    void Update()
    {
        if (useRelativeRotation)
        {
            transform.rotation = relativeRotation;  // sets current location to match local rotation
        }
    }
}
