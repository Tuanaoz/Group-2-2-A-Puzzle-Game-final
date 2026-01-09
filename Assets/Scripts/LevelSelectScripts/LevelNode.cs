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
        bool unlocked;
        bool isTutorial = levelIndex < 3;


        if (isTutorial)
        {
            unlocked = true;
        }
        else
        {
            unlocked = levelIndex <= ProgressManager.HighestUnlockedLevel;

            if (lockIcon != null)
                lockIcon.SetActive(!unlocked);
            if (UnlockedIcon != null)
                UnlockedIcon.SetActive(unlocked);
        }
        
        levelButton.interactable = unlocked;
        levelButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!levelButton.interactable)
            return;

        PlayerPrefs.SetInt("CurrentLevelID", levelIndex);
        PlayerPrefs.Save();

        string levelName;

        if (levelIndex == 0)
            levelName = "Tutorial - 1";
        else if (levelIndex == 1)
            levelName = "Tutorial - 2";
        else if (levelIndex == 2)
            levelName = "Tutorial - 3";
        else if (levelIndex == 3)
            levelName = "tempLevelData";
        else
            levelName = "Level_0" + levelIndex;

        LevelLoadRequest.RequestedLevelName = levelName;
        SceneManager.LoadScene("PlayLevel");
    }
}