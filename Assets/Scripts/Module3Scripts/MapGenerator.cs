using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] gridPrefabs;
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    public int mapSeed; // Variable for mapSeed - Random
    public bool isMapOfTheDay; // Can base mapSeed on current day and time
    private Room[,] grid; // 2-D array of room - accesses room script

    public InputField mapSeedInputField;




    // Start is called before the first frame update
    void Start()
    {
        if (isMapOfTheDay) // Checks if isMapOfDay bool is checked (in inspector)
        {
            mapSeed = DateToInt(DateTime.Now.Date);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Tab)) // Just to test GenerateMap function; should impliment on Input 
        {
            GenerateMap();
       
    } */


    // Tried to Set the Seed using UI Input from Input Field
    public void UpdateMapSeedFromUIInputField()
    {
        //mapSeed = mapSeedInputField;
    }

    public void GenerateMapOfTheDaySeed()
    {
            mapSeed = DateToInt(DateTime.Now.Date);
    }

    // Return a random room from our gridPrefabs array
    public GameObject RandomRoomPrefab()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public void GenerateMap()
    {
        // Set our seed
        UnityEngine.Random.InitState(mapSeed);
        // Clear out the grid - "column" is our X, "row" is our Y
        grid = new Room[cols, rows];

        // For each grid row...
        for (int currentRow = 0; currentRow < rows; currentRow++) // iterates through all rows in grid collection
        {
            // for each column in that row...
            for (int currentCol = 0; currentCol < cols; currentCol++)  // currentCol = 0 is starting position in array
            {
                // Figure out the location for that particular room tile
                float xPosition = roomWidth * currentCol; // local var
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0f, zPosition);

                // Create a new tile at the appropriate location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Set its parent
                tempRoomObj.transform.parent = this.transform; // Relies on MapGenerator to be parent of this collection of tiles

                // Give it a meaningful name
                tempRoomObj.name = "Room_" + currentCol + ", " + currentRow; // Names room then displays col and row

                // Get the room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // Open the doors
                // If we are on bottom row, open North door
                if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }

                else if (currentRow == rows -1) // North most Row
                {
                    // Otherwise, if we are on the top row, open the south door
                    tempRoom.doorSouth.SetActive(false);
                }

                else
                {
                    // Otherwise we are in the middle so open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                // What about the East and West Rows???
                // If we are the Westmost column, open the east door
                if (currentCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }

                else if (currentCol == cols -1)
                {
                    // Otherwise if we are on the Eastmost column, open the west door
                    tempRoom.doorWest.SetActive(false);
                }

                else
                {
                    // Otherwise we are in the middle so open both doors
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }

                // Save the Room component to the grid array to access later
                grid[currentCol,currentRow] = tempRoom;
            } 
        }
    }

    // Helper function
    public int DateToInt (DateTime dateToUse)
    {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    // Was going to use this to return an array of the rooms created in the grid 
    public void PopulateAiSpawn() // Game Manager instantiates AI Controllers for each AI when gameplay begins.
    {
        // Get the room then run foreach loop here

        foreach (Room room in grid)
        {
            if (room == null) continue;
            // For each room in grid, run the GameManager's SpawnAI() using UnityEngine.Random within a range (list of possible tanks to choose from defined on GameManager inspector) ...
            // .. to a list held on Room script called aiSpawnPoints(holds all gameObjects with component AISpawnPoint attached | list) using UE Random.Range to pick a random aiSpawnPoint
            // .. based on the total count of spawn points; since its an empty gameObject, its .transform since its a transform we need
            AIController newAiTank = GameManager.instance.SpawnAI(UnityEngine.Random.Range(0,GameManager.instance.aiTanks.Length) , room.aiSpawnPoints[UnityEngine.Random.Range(0, room.aiSpawnPoints.Count)].transform);

            // way point list held on Room script to auto generate with room prefabs
            newAiTank.waypoints = room.patrolWaypoints;  // Used with AIController scripts that Patrol

        }
        
    }
    
}
