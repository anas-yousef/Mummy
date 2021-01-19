using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public MummyPaper tp;

    [Header("Levels")]
    // todo need to update here every new level 
    public GameObject[] levels = new GameObject[2];
    
    [Header("Sounds")]
    // todo need to update here every new level 
    public AudioSource[] sounds = new AudioSource[2];

    [Header("Panels")] 
    public GameObject startPanel;
    public GameObject settingsPanel;
    public GameObject[] levelFinishedPanel = new GameObject[3];

    private int _curLevel;
    private GameObject _curLevelMap;
    private Transform _curStartLocation;

    void Start()
    {
        // Manage panels. 
        startPanel.SetActive(true);
        player.SetActive(false);
    }

    private void Update()
    {
        
        // Goto options. 
        if (Input.GetKeyDown(KeyCode.Escape) && !startPanel.activeSelf && !levelFinishedPanel[0].activeSelf
            && levelFinishedPanel[1].activeSelf && levelFinishedPanel[2].activeSelf)
        {
            Time.timeScale = 0.0f;
            settingsPanel.SetActive(true);
        }
        
        // Goto options. 
        if (Input.GetKeyDown(KeyCode.M))
        {
            sounds[0].mute = !sounds[0].mute;
            sounds[1].mute = !sounds[1].mute;
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
    public void FinishLevel()
    {
        // Destroy current level.
        Destroy(_curLevelMap);
        
        // show next level panel 
        levelFinishedPanel[_curLevel].SetActive(true);

        if (_curLevel + 1 < levels.Length)
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

    public void StartNewLevel()
    {
        // show next level panel 
        levelFinishedPanel[_curLevel - 1].SetActive(false);
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
        // TODO: stop the throw
        // tp.StopThrow();

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
        levelFinishedPanel[_curLevel].SetActive(false);
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
