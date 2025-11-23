using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{

    private bool isOpen = false;
    public CameraMovement mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenSettingsUI() {
        this.gameObject.SetActive(true);
        isOpen = true;
        if (!mainCamera)
            return;
        mainCamera.StopMovement();
    }

    public void CloseSettingsUI() {
        this.gameObject.SetActive(false);
        isOpen = false;
        if (!mainCamera)
            return;
        mainCamera.StartMovement();
    }

    public void ToggleSettingsUI() {
        if (isOpen) {
            CloseSettingsUI();
            isOpen = false;
            if (!mainCamera)
                return;
            mainCamera.StartMovement();
        } else {
            OpenSettingsUI();
            isOpen = true;
            if (!mainCamera)
                return;
            mainCamera.StopMovement();
        }
    }
}
