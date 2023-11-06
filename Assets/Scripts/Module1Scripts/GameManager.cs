using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public Transform playerSpawnTransform;

    public Transform AiSpawnTransform;


    // Prefabs
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;
    public GameObject aiTankPrefab;
    public GameObject aiTankControllerPrefab;

    public List<PlayerController> players; // List created on Player Controller Script
    //public List<AIController> aiEntities; // Not using, tried to set target for AI Controllers using GameManager
    public List<PawnSpawnPoint> pawnSpawnPoints = new List<PawnSpawnPoint>(); // Create and instanite list to use during code

    //public List<AISpawnPoint> aiSpawnPoints = new List<AISpawnPoint>(); //public List<AISpawnPoint> AISpawnPoints;

    public AiDictionary[] aiTanks;

    private List<Room> rooms; // List of rooms

    public MapGenerator mapGenerator; // Call to mapGenerator should change so GameManager does not depend on mapGenertaor script
    public CameraFollow cameraFollow; // Refrence to CameraFollow script

    // Module 4 enum State list
    //public enum GameState { TitleScreen, MainMenu, OptionsScreen, CreditsScreen, Gameplay, GameOverScreen}
    //public GameState currentState; // Var to call enum from list

    // Module 4: Game States
    //public GameObject TitleScreenStateObject;
    //public GameObject MainMenuStateObject;
    //public GameObject OptionsScreenStateObject;
    //public GameObject CreditsScreenStateObject;
    //public GameObject GameplayStateObject;
    //public GameObject GameOverScreenStateObject;
     

    private void Start()
    {
        /* only will spawn 4
        for (int i = 0; i < aiTanks.Length; i++)
        {
            SpawnAI(i);
        }
        */
        //SpawnAI(0);
        // Temp code - for now we spawn player as soon as the GameManager starts
        SpawnPlayer(); // calling function I just made

        if (mapGenerator != null) // If a MapGenerator is present, run PopulateAiSpawn()
        {
            mapGenerator.PopulateAiSpawn();
        }
        // Module 4: 
        //ChangeState(GameState.TitleScreen); // Will start the player on the Title Screen

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
        // Mod 3
        playerSpawnTransform = GetPawnSpawnPoint().transform; // Set transform of Player Spawn

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

        //Module 3: Add Powerup component to Pawn on spawn
        newPawnObj.AddComponent<PowerupManager>();

        // Hook them up!
        newController.pawn = newPawn;
        
        // Set camera to target Spawned tank Pawn!
        cameraFollow.SetTarget(newPawn.transform);
    }

    public AIController SpawnAI(int aiNumber, Transform AiSpawnTransform) // What AI they want to use and where to spawn it
    {
        //AiSpawnTransform = GetAiSpawnPoint().transform;

        GameObject newAiControllerObj = Instantiate(aiTanks[aiNumber].AiControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        GameObject newAiPawnObj = Instantiate(aiTanks[aiNumber].AiTankPrefab, AiSpawnTransform.position, AiSpawnTransform.rotation) as GameObject;

        AIController newAiController = newAiControllerObj.GetComponent<AIController>();

        Pawn newAiPawn = newAiPawnObj.GetComponent<Pawn>();

        newAiPawnObj.AddComponent<PowerupManager>();

        newAiController.pawn = newAiPawn;

        return newAiController;
    }

    public void RestartPlayer(Controller controller) // Restart game - with button press after lives are = 0
    {
        // Spawn the Player Controller at (0, 0, 0) with no rotation
        //GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        playerSpawnTransform = GetPawnSpawnPoint().transform; // Set transform of Player Spawn

        controller.transform.position = Vector3.zero; // 0s out
        controller.transform.rotation = Quaternion.identity;

        // Spawn the Pawn and connect it to the Controller
        GameObject newPawnObj = Instantiate(tankPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation) as GameObject;

        // Get the Player Controller component and Pawn component.
        //Controller newController = newPlayerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        // Module 2: add and set noiseMaker component on Pawn
        newPawnObj.AddComponent<NoiseMaker>();
        newPawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
        newPawn.noiseMakerVolume = 3;

        //Module 3: Add Powerup component to Pawn on spawn
        newPawnObj.AddComponent<PowerupManager>();

        controller.pawn = newPawn;
        // Set camera to target Spawned tank Pawn!
        cameraFollow.SetTarget(newPawn.transform);
    }



    public GameObject GetPawnSpawnPoint() // Gets a random player spawn point
    {
        if (mapGenerator == null)
        {
            return playerSpawnTransform.gameObject; 
        }
        int spawnSeed = (int)System.DateTime.Now.Ticks; // Casts
        
        UnityEngine.Random.InitState(spawnSeed); // Random seed based on time

        return pawnSpawnPoints[UnityEngine.Random.Range(0, pawnSpawnPoints.Count)].gameObject;
    }



    /*
    private GameObject GetAiSpawnPoint() // Will grab one random spawn point
    {
        int spawnSeed = (int)System.DateTime.Now.Ticks;

        UnityEngine.Random.InitState(spawnSeed);

        AISpawnPoint spawnPoint = aiSpawnPoints[UnityEngine.Random.Range(0, aiSpawnPoints.Count)];

        while (spawnPoint.isAiSpawnUsed == true)
        {
            spawnPoint = aiSpawnPoints[UnityEngine.Random.Range(0, aiSpawnPoints.Count)]; // Grab another random spawn point to check if used

            if (spawnPoint.isAiSpawnUsed == false)
            {
                spawnPoint.isAiSpawnUsed = true;
                break;
            }
            
        }

        return spawnPoint.gameObject; // returns SpawnPoint
    }
    */

    // Respawn
    public void Respawn(Controller tank) // Can be used with AI or Player Tank
    {
        if (tank.GetType() == typeof(AIController)) // If type AIController
        {
            Destroy(tank.gameObject);
        }

        else // If it is not AI, it should be our Player
        {
            if (tank.pawn != null && tank.pawn.currentLives > 0 )
            {
                Transform spawnTransform = tank.pawn.GetComponent<GameObject>().transform;
                
                spawnTransform = GetPawnSpawnPoint().transform; // Set transform of Player Spawn

                tank.transform.position = Vector3.zero; // 0s out
                tank.transform.rotation = Quaternion.identity;

                
            }
        }
    }
    /*

    // Module 4: Helper function to Deactivate all Game States
    private void DeactivateAllStates()
    {
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
    }

    public void ActivateTitleScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the title screen
        TitleScreenStateObject.SetActive(true);

        // TODO: Do what needds to be done when title screen starts... 
    }

    public void ActivateMainMenu()
    {
        // First we deactivate all states
        DeactivateAllStates();

        //Activate the MainMenu
        MainMenuStateObject.SetActive(true);

        // TODO: Do what needs to be done when main menu starts...

    }

    public void ActivateOptionsScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the Options Screen
        OptionsScreenStateObject.SetActive(true);

        // TODO: what needs to happen for Options screen
    }

    public void ActivateCreditsScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the Credits screen
        CreditsScreenStateObject.SetActive(true);

        // TODO: what needs to happen for Credits screen
    }

    public void ActivateGameplayScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the Gameplay screen
        GameplayStateObject.SetActive(true);

        //TODO: What needs to happen for Gameplay screen
    }

    public void ActivateGameoverScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the Gameover Screen
        GameOverScreenStateObject.SetActive(true);
    }

    // Helper functions
    public void ChangeState(GameState newState) // Will change the GameState to whatever parameter we enter from the enum.
    {
        // Change the current state
        currentState = newState;

    }

    */
}
