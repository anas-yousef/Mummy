using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;

    [Header("Levels")]
    // todo need to update here every new level 
    public GameObject[] levels = new GameObject[2];
    
    private int _curLevel;
    private GameObject _curLevelMap;
    private Transform _curStartLocation;
    
    [Header("Panels")] 
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject settingsPanel;

    void Start()
    {
        // Manage panels. 
        startPanel.SetActive(true);
        winPanel.SetActive(false);
        player.SetActive(false);
    }

    private void Update()
    {
        
        // Goto options. 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            settingsPanel.SetActive(true);
        }
    }

    /**
     * Start the first level of the game.
     */
    public void PressStart()
    {
        // Manage panels.
        startPanel.SetActive(false);

        // Get first level.
        _curLevel = 0;
        _curLevelMap = Instantiate(levels[_curLevel]);
        Transform trans = _curLevelMap.transform;
        
        // Locate the player
        _curStartLocation = trans.Find("Player Start Location");
        player.transform.localPosition = new Vector3(_curStartLocation.position.x, _curStartLocation.position.y, 0);        
        
        player.SetActive(true);
    }

    /**
     * After finishing a level- move to the next one!
     * if there are no more levels- move to Win screen
     */
    public void SwitchLevel()
    {
        // Destroy current level.
        Destroy(_curLevelMap);
        
        // No more levels. 
        if (_curLevel + 1 == levels.Length)
        {
            Debug.Log("should win");
            winPanel.SetActive(true);
            player.SetActive(false);
        }

        else
        {
            _curLevel += 1;
            // Get the next level.
            _curLevelMap = Instantiate(levels[_curLevel]);

            // relocate the player
            Transform trans = _curLevelMap.transform;
            _curStartLocation = trans.Find("Player Start Location");
            
            player.transform.localPosition = new Vector3(_curStartLocation.position.x, _curStartLocation.position.y, 0); 
        }
    }

    /**
     * Restart the current level.
     */
    public void RestartLevel()
    {
        // Start time.
        Time.timeScale = 1.0f;

        // Disable settings panel in case exists.
        settingsPanel.SetActive(false);
        
        // Restart the map.
        Destroy(_curLevelMap);
        _curLevelMap = Instantiate(levels[_curLevel]);

        // relocate the player
        Transform trans = _curLevelMap.transform;
        _curStartLocation = trans.Find("Player Start Location");
        player.transform.localPosition = new Vector3(_curStartLocation.position.x, _curStartLocation.position.y, 0);
        
    }
    
    /**
     * Restart the game.
     */
    public void BackToMainMenu()
    {
        // Destrory current level. 
        Destroy(_curLevelMap);

        // Start time.
        Time.timeScale = 1.0f;

        // Manage panels.
        settingsPanel.SetActive(false);
        winPanel.SetActive(false);
        startPanel.SetActive(true);

        // Back to start settings. 
        player.SetActive(false);
        _curLevel = 0;
    }
    
    /**
     * Quit the game.
     */
    public void quit()
    {
        // Quit. 
        Debug.Log("quitApp");
        Application.Quit();
    
    }
    
    /**
     * Go back to the level.
     */
    public void BackToLevel()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1.0f;
   
    }

}
