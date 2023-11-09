using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateTransitions : MonoBehaviour
{
    public GameManager gameManager; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) 
        {
            gameManager.ActivateTitleScreen();
        }

        if (Input.GetKeyDown(KeyCode.X)) 
        {
            gameManager.ActivateMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.C)) 
        {
            gameManager.ActivateOptionsScreen();
        }

        if (Input.GetKeyDown(KeyCode.V)) 
        {
            gameManager.ActivateCreditsScreen();
        }

        if (Input.GetKeyDown(KeyCode.B)) 
        {
            gameManager.ActivateGameplayScreen();
        }

        if (Input.GetKeyDown(KeyCode.N)) 
        {
            gameManager.ActivatePauseScreen();
        }

        if (Input.GetKeyDown(KeyCode.M)) 
        {
            gameManager.ActivateGameOverScreen();
        }
    }
}
