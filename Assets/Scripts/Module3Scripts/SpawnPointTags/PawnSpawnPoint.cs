using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawnPoint : MonoBehaviour
{
    // Will use gamemanager to spawn in pawn
    private void Awake()
    {
        GameManager.instance.pawnSpawnPoints.Add(this); 
    }
}
