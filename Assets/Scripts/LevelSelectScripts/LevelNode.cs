using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNode : MonoBehaviour
{
    public int levelIndex;
    public GameObject lockIcon;
    public Button levelButton;
    public GameObject UnlockedIcon;
    public SaveLoadManager saveLoadManager;

    void Start()
    {
// Check if this level is unlocked based on player progress
        bool unlocked = levelIndex <= ProgressManager.HighestUnlockedLevel;
// Enable or disable button interaction
        levelButton.interactable = unlocked;

//Shows locked or unlocked icon based on highest level completed
        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);
        if (UnlockedIcon != null)
            UnlockedIcon.SetActive(unlocked);

        levelButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
// Do nothing if the level is locked
        if (!levelButton.interactable)
            return;

        string levelName;

// First level uses the temp level data
        if (levelIndex == 1)
        {
            levelName = "tempLevelData";
        }
        else
        {
// Load core levels based on their index
            levelName = "Level_0" + levelIndex;
        }
        LevelLoadRequest.RequestedLevelName = levelName;
        SceneManager.LoadScene("PlayLevel");
    }
}