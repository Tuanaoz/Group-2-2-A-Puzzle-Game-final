using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    
    private bool isOpen = false;
    public CameraMovement mainCamera;
    public GameObject mainMenuButtons;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenSettingsUI() {
        this.gameObject.SetActive(true);
        isOpen = true;

        // Hide main menu buttons
        if (mainMenuButtons != null)
            mainMenuButtons.SetActive(false);

        if (mainCamera != null)
            mainCamera.StopMovement();
    }

    public void CloseSettingsUI() {
        this.gameObject.SetActive(false);
        isOpen = false;

        // Show main menu buttons
        if (mainMenuButtons != null)
            mainMenuButtons.SetActive(true);

        if (mainCamera != null)
            mainCamera.StartMovement();
    }

    public void ToggleSettingsUI() {
        if (isOpen) {
            CloseSettingsUI();
        } else {
            OpenSettingsUI();
        }
    }
}
