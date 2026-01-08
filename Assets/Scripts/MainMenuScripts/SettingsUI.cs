using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    
    private bool isOpen = false;
    public CameraMovement mainCamera;
    public GameObject mainMenuButtons;
    public GameObject gameTitle;
    public GameObject levelSelectionPanel;
    public GameObject levelLayer;


// Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

// When settings panel opens only background is active
    public void OpenSettingsUI() {
        this.gameObject.SetActive(true);
        isOpen = true;

        if (mainMenuButtons != null)
            mainMenuButtons.SetActive(false);

        if (gameTitle != null)
            gameTitle.SetActive(false);

        if (mainCamera != null)
            mainCamera.StopMovement();

        if (levelLayer != null)
            levelLayer.SetActive(false);
    }

// When settings panel is closed other UI features get set active
    public void CloseSettingsUI() {
        this.gameObject.SetActive(false);
        isOpen = false;

        if (mainMenuButtons != null)
            mainMenuButtons.SetActive(true);

        if (gameTitle != null)
            gameTitle.SetActive(true);

        if (mainCamera != null)
            mainCamera.StartMovement();

        if (levelLayer != null)
            levelLayer.SetActive(true);
    }

    public void ToggleSettingsUI() {
        if (isOpen) {
            CloseSettingsUI();
        } else {
            OpenSettingsUI();
        }
    }

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}