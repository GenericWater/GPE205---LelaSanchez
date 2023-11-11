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
    public List<PawnSpawnPoint> pawnSpawnPoints = new List<PawnSpawnPoint>(); // Create and inst
                                                                              // anite list to use during code

    //public List<AISpawnPoint> aiSpawnPoints = new List<AISpawnPoint>(); //public List<AISpawnPoint> AISpawnPoints;

    public AiDictionary[] aiTanks;

    private List<Room> rooms; // List of rooms

    public MapGenerator mapGenerator; // Call to mapGenerator should change so GameManager does not depend on mapGenertaor script
    public CameraFollow cameraFollow; // Refrence to CameraFollow script
 

    // Module 4 enum State list
    public enum GameState { TitleScreen, MainMenu, OptionsScreen, CreditsScreen, Gameplay, PauseScreen, GameOverScreen}
    public GameState currentState; // Var to call enum from list

    // Added myself | list to refrence the public game objects and initalized it 
    [HideInInspector]public List<GameObject> gameStatesList = new List<GameObject>(); // expose the gameStatesList in the Unity Inspector, your designers can directly add the GameObjects to the list within the Unity Editor. // IMPORTANT: MIGHT NOT NEED!
                                                                     // The Start method would only be necessary if you want to add some initial setup or logic in your script, which doesn't involve populating the list itself.
    // Module 4: Game States
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject PauseScreenStateObject; // Added Pause screen functionality
    public GameObject GameOverScreenStateObject;


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

        // Added myself | if all states are NOT null (Empy in Inspector)... *Not really incorporated into my code.*
         
        if (TitleScreenStateObject && MainMenuStateObject && OptionsScreenStateObject && CreditsScreenStateObject && GameplayStateObject && PauseScreenStateObject && GameOverScreenStateObject != null)
        {
            // Added myself |  add public GameObject in code, so designers don't worry about it for States to gameStatesList
            gameStatesList.Add(TitleScreenStateObject);
            gameStatesList.Add(MainMenuStateObject);
            gameStatesList.Add(OptionsScreenStateObject);
            gameStatesList.Add(CreditsScreenStateObject);
            gameStatesList.Add(GameplayStateObject);
            gameStatesList.Add(PauseScreenStateObject);
            gameStatesList.Add(GameOverScreenStateObject);
        }

        if (IsGameplayStateActive()) // Will SPAWN PLAYER!!!!!!!!!!!!!!!!!!!!!
        {
            ActivateGameplayScreen();
        }

        // Module 4: 
        DeactivateAllStates(); // First Deactivate all states
        //ChangeState(GameState.TitleScreen); // Will start the player on the Title Screen

        /*
        if (tankPawnPrefab != null)
        {
            ChangeState(GameState.TitleScreen); // Will start the player on the Title Screen
        }
        */


    }

    /*
    // Trued to Spawn player and AI... DID NOT WORK
    private void OnEnable()
    {
        // Added myself | if all states are NOT null (Empy in Inspector)...
        if (TitleScreenStateObject && MainMenuStateObject && OptionsScreenStateObject && CreditsScreenStateObject && GameplayStateObject && PauseScreenStateObject && GameOverScreenStateObject != null)
        {


            if (IsGameplayStateActive() == true) // TODO: If gameStatesList != null ...
            {
                SpawnPlayer(); // calling function I just made | Spawn player

                if (mapGenerator != null) // If a MapGenerator is present, run PopulateAiSpawn()
                {
                    mapGenerator.PopulateAiSpawn(); //Uncomment to generate map with MapGenerator
                }
            }


        }
    }




    public void Update() // Will spawn player on update with AI but will get a lot of errors
    {

    }

    */
    // To use our Singleton, we need to create a new object. We will want to name this object GameManager

    public void SpawnPlayer() // Get's a Random Spawn Point for Player
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

        // Test Initialization for Damage done
        newPawn.damageDone = 10.0f; // Might need to change to edit 
        newPawn.maxLives = 3; // Set the number of max lives

        // Hook them up!
        newController.pawn = newPawn;
        newPawn.controller = newController; // Module 4: ensures pawn is connected to controller

        
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
    

    // Module 4: Helper function to Deactivate all Game States
    private void DeactivateAllStates()
    {
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        PauseScreenStateObject.SetActive(false); // Ensure Pause is set to false
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
        if (IsGameplayStateActive() == true) // DID NOT SPAWN PLAYER EITHER
        {
            // Added myself | if all states are NOT null (Empy in Inspector)...
            if (TitleScreenStateObject && MainMenuStateObject && OptionsScreenStateObject && CreditsScreenStateObject && GameplayStateObject && PauseScreenStateObject && GameOverScreenStateObject != null)
            {


                if (IsGameplayStateActive() == true) // TODO: If gameStatesList != null ...
                {
                    SpawnPlayer(); // calling function I just made | Spawn player

                    if (mapGenerator != null) // If a MapGenerator is present, run PopulateAiSpawn()
                    {
                        mapGenerator.PopulateAiSpawn(); //Uncomment to generate map with MapGenerator
                    }
                }


            }
        }
    }

    public void ActivatePauseScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the Pause Screen
        PauseScreenStateObject.SetActive(true);

        // TODO: What needs to happen for Pause Screen

    }

    public void ActivateGameOverScreen()
    {
        // First we deactivate all states
        DeactivateAllStates();

        // Activate the Gameover Screen
        GameOverScreenStateObject.SetActive(true);
    }

    // Helper function to change states
    public void ChangeState(GameState newState) // Will change the GameState to whatever parameter we enter from the enum.
    {
        // Change the current state
        currentState = newState;

    }

    protected bool IsAnyGameStateActive()
    {
        foreach (GameObject gameState in gameStatesList)
        {
            if (gameState != null && gameState.activeInHierarchy) // will check if game state is currently active in Hierarchy in Editor
            {
                return true; // Return true if any game state is active
            }
        }

        return false;
    }

    protected bool IsGameplayStateActive()
    {
        if (GameplayStateObject != null && GameplayStateObject.activeInHierarchy)
        {
            return true; // Return true if the GameplayState is active && not null
        }

        return false;
    }

    /*
    // If gameplay state or any state is set to true... helper function

    // TODO: protected bool to check if gameStatesList (EXAMPLE BUT ANY STATE) GameOverScreenStateObject.SetActive(true); | Will probable use if( OR)
    protected bool IsGameStateSetTrue()
    {
        if(SetAllStatesToActive() == true)

        return; // Need return statement for a bool helper function
    }

    // Added myself | Sets all states to Active
    protected void SetAllStatesToActive() // 7 object States!
    {
        foreach (GameObject gameState in gameStatesList)
        {
            if (gameState != null) // Error prevention
            {
                gameState.SetActive(true);
            }
        }

        //TitleScreenStateObject.SetActive(true);
        //MainMenuStateObject.SetActive(true);
        //OptionsScreenStateObject.SetActive(true);
        //CreditsScreenStateObject.SetActive(true);
        //GameplayStateObject.SetActive(true);
        //PauseScreenStateObject.SetActive(true);
        //GameOverScreenStateObject.SetActive(true);

    }
    */


}
