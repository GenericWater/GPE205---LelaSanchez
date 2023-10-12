using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleExample : MonoBehaviour
{
    //Variables
    public string theText = "Hello World.";


    // Start is called before the first frame update
    private void Start() // added private before void to match UAT Unity C# standards
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Writes the value stored in our variable "theText" to the console window
        Debug.Log(theText);
    }
}
