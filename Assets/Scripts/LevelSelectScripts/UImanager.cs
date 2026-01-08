using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject levelLayer;

    public GameObject settingsPanel;
    public GameObject levelSelectionPanel;
    public GameObject popUpPanel;
    public GameObject tutorialLevelSelectPanel;

    bool panelOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelOpen)
                CloseAllPanels();
            else
                OpenSettings();
        }
    }
//Open panel close other
    public void OpenSettings()
    {
        CloseAllPanels();

        panelOpen = true;
        levelLayer.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenLevelSelection()
    {
        CloseAllPanels();

        panelOpen = true;
        levelLayer.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }

    public void OpenTutorialLevelSelect()
    {
        CloseAllPanels();

        panelOpen = true;
        levelLayer.SetActive(false);
        tutorialLevelSelectPanel.SetActive(true);
    }

// Close the panel make level layer active
    public void CloseAllPanels()
    {
        panelOpen = false;

        settingsPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
        levelLayer.SetActive(true);
        tutorialLevelSelectPanel.SetActive(false);
        popUpPanel.SetActive(false);
    }
//Scene Manager
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}