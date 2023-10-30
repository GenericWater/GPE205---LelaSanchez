using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public Transform playerSpawnTransform;


    // Prefabs
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;

    public List<PlayerController> players; // List created on Player Controller Script
    public List<AIController> aiEntities; // Not using, tried to set target for AI Controllers using GameManager


    private void Start()
    {
        // Temp code - for now we spawn player as soon as the GameManager starts
        SpawnPlayer(); // calling function I just made
    }


    //Awake is called when the gme object is first created - before even Start can run!
    private void Awake()
    {
        if (instance == null) // If the instance doesn't exist yet...
        {
            instance = this; // This is the instance
            DontDestroyOnLoad(gameObject); // Don't destroy if we have a new scene
        }
        else
        {
            // Otherwise, there is already an instance, so destroy this gameObject
            Destroy(gameObject);
        }
    }
    // To use our Singleton, we need to create a new object. We will want to name this object GameManager

    public void SpawnPlayer()
    {
        // Spawn the Player Controller at (0, 0, 0) with no rotation
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Spawn the Pawn and connect it to the Controller
        GameObject newPawnObj = Instantiate(tankPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation) as GameObject;

        // Get the Player Controller component and Pawn component.
        Controller newController = newPlayerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        // Module 2: add and set noiseMaker component on Pawn
        newPawnObj.AddComponent<NoiseMaker>();
        newPawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
        newPawn.noiseMakerVolume = 3;

        // Hook them up!
        newController.pawn = newPawn;
    }
}
