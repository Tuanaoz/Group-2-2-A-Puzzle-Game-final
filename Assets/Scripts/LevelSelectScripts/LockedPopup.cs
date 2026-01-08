using UnityEngine;

public class LockedPopup : MonoBehaviour
{
    public GameObject popupRoot;
    public GameObject levelLayer;

 // Hides popup at start
    void Start()
    {
        popupRoot.SetActive(false);
    }

// Shows popup and hides level buttons
    public void Show()
    {
        levelLayer.SetActive(false);
        popupRoot.SetActive(true);
    }

// Closes popup and shows level buttons again
    public void Close()
    {
        popupRoot.SetActive(false);
        levelLayer.SetActive(true);
    }
}