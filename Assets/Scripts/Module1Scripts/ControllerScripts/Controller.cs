using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Controller : MonoBehaviour
{
    // Variable to hold our Pawn
    public Pawn pawn;

    // Module 4: Public variable to hold Score points - will be able to keep points per player
    public float score; // Allows for players and AI to keep score if wanted.

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    // Our child classes MUST override the way they process inputs
    public abstract void ProcessInputs();

    public virtual void AddToScore(float scoreToAdd) // Will add to score by parameter scoreToAdd
    {
        score += scoreToAdd;
    }
}
