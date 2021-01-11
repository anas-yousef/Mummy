using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;

    public PlayerController playerController;

    [Header("Levels")]
    // todo need to update here every new level 
    public Grid[] levels = new Grid[1];
    public Transform[] playerStartLocation = new Transform[1];

    private int _curLevel; 
    
    [Header("Panels")] 
    public GameObject startPanel;
    public GameObject winPanel;

    void Start()
    {
        startPanel.SetActive(true);
        winPanel.SetActive(false);
        player.SetActive(false);
        _curLevel = 0;
    }

    /**
     * Start the first level of the game.
     */
    public void PressStart()
    {
        Instantiate(levels[_curLevel]);
        player.SetActive(true);
        startPanel.SetActive(false);
        player.transform.localPosition = new Vector3(playerStartLocation[_curLevel].position.x, 
            playerStartLocation[_curLevel].position.y, 1);
        // todo locate player in the start location 
    }

    /**
     * After finishing a level- move to the next one!
     */
    public void SwitchLevel()
    {
        // Destroy current level.
        Destroy(levels[_curLevel]);
        
        // No more levels. 
        if (_curLevel + 1 == levels.Length)
        {
            Debug.Log("should win");
            winPanel.SetActive(true);
            player.SetActive(false);
        }

        else
        {
            // Get the next level.
            Instantiate(levels[_curLevel + 1]);
        
            // relocate the player
            player.transform.localPosition = new Vector3(playerStartLocation[_curLevel].position.x, 
                playerStartLocation[_curLevel].position.y, 1);   
        }
    }

    public void RestartLevel()
    {
       
        // relocate the player
        player.transform.localPosition = new Vector3(playerStartLocation[_curLevel].position.x, 
                                                     playerStartLocation[_curLevel].position.y, 1);
        
        // Stop it's movement. 
        playerController.StopMovement();

    }

}
