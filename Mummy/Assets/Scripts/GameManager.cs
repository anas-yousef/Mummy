//using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public MummyPaper tp;
    public Transform all;

    [Header("Levels")]
    // todo need to update here every new level 
    public GameObject[] levels = new GameObject[2];
    
    [Header("Panels")] 
    public GameObject startPanel;
    public GameObject settingsPanel;
    public GameObject winPanel;
    public GameObject mainSettingsPanel;
    public GameObject howToPanel;

    public GameObject mainMenuFirstButton, mainSettingsFirstButton, settingFirstButton, winFirstButton; 


    private int _curLevel;
    private GameObject _curLevelMap;
    private Transform _curStartLocation;
    private bool _mainMenu; 

    void Start()
    {
        // Manage panels. 
        startPanel.SetActive(true);
        player.SetActive(false);
        
        _mainMenu = true; 
    }

    private void Update()
    {
        // Main menu
        // Return from main settings to main menu. 
        if (Input.GetKeyDown(KeyCode.Escape) && mainSettingsPanel.activeSelf && _mainMenu)
        {
            // manage panels. 
            mainSettingsPanel.SetActive(false);
            BackToMainMenu();
        }
        // Return from how to. 
        else if (Input.GetKeyDown(KeyCode.Escape) && howToPanel.activeSelf)
        {
            howToPanel.SetActive(false);
            BackToMainMenu();
        }
        
        // While in game. 
        // Return from main settings to setting. 
        else if (Input.GetKeyDown(KeyCode.Escape) && mainSettingsPanel.activeSelf && !_mainMenu)
        {
            // manage panels. 
            settingsPanel.SetActive(true);
            mainSettingsPanel.SetActive(false);
            
            // chose button marked on screen. 
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingFirstButton);
        }

        // Goto options- while in Game. 
        else if (Input.GetKeyDown(KeyCode.Escape) && !startPanel.activeSelf && !mainSettingsPanel.activeSelf &&
                 !howToPanel.activeSelf && !settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(true);
            Time.timeScale = 0.0f;
            
            // chose button marked on screen. 
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingFirstButton);
        }
        
        // Return from options- while in Game. 
        else if (Input.GetKeyDown(KeyCode.Escape) && settingsPanel.activeSelf && !mainSettingsPanel.activeSelf)
        {
            BackToLevel();
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
        trans.parent = all; 
        
        // Locate the player
        _curStartLocation = trans.Find("Player Start Location");
        player.transform.localPosition = new Vector3(_curStartLocation.position.x, _curStartLocation.position.y, 0);        
        
        player.SetActive(true);

        _mainMenu = false; 
    }

    /**
     * After finishing a level- move to the next one!
     * if there are no more levels- move to Win screen
     */
    public void SwitchLevel()
    {
        // Destroy current level.
        Destroy(_curLevelMap);
        
        if (_curLevel + 1 < levels.Length)
        {
            _curLevel += 1;
            // Get the next level.
            _curLevelMap = Instantiate(levels[_curLevel]);

            // relocate the player
            Transform trans = _curLevelMap.transform;
            trans.parent = all;
            _curStartLocation = trans.Find("Player Start Location");
            
            player.transform.localPosition = new Vector3(_curStartLocation.position.x, _curStartLocation.position.y, 0);
        }
        else
        {
            Debug.Log("need to win");
            winPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(winFirstButton);
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
        //call restart player in player shooting
        player.GetComponent<PlayerShooting>().RestartPlayer();
        player.transform.localPosition = new Vector3(_curStartLocation.position.x, _curStartLocation.position.y, 0);
        // TODO: stop the throw
        tp.StopThrow();

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
        
        // chose button marked on screen. 
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);

        _mainMenu = true; 
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
    
    public void BackToLevel()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1.0f;
   
    }
    public void PressHowTo()
    {
        howToPanel.SetActive(true);
        startPanel.SetActive(false);

    }
    
    public void PressMainSettings()
    {
        settingsPanel.SetActive(false);
        startPanel.SetActive(false);

        mainSettingsPanel.SetActive(true);
        
        // chose button marked on screen. 
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainSettingsFirstButton);
        
    }
    
    
}
