using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject contentPanel;
    public GameObject verticalScrollbar;

    public GameObject charactersGroup;
    public GameObject triggersGroup;
    public GameObject goalsGroup;
    public GameObject utilityGroup;
    public GameObject saveGame;

    private GameObject currentVisibleGroup = null;

    void Start()
    {
        charactersGroup.SetActive(false);
        triggersGroup.SetActive(false);
        goalsGroup.SetActive(false);
        utilityGroup.SetActive(false);
        saveGame.SetActive(false);

        contentPanel.SetActive(false);
        verticalScrollbar.SetActive(false);
    }

    public void ToggleCharacters()
    {
        ToggleGroup(charactersGroup);
    }

    public void ToggleTriggers()
    {
        ToggleGroup(triggersGroup);
    }

    public void ToggleGoals()
    {
        ToggleGroup(goalsGroup);
    }

    public void ToggleUtility()
    {
        ToggleGroup(utilityGroup);
    }

    public void ToggleSaveGame()
    {
        ToggleGroup(saveGame);
    }

    private void ToggleGroup(GameObject selectedGroup)
    {
        bool isSameGroup = (currentVisibleGroup == selectedGroup);

        charactersGroup.SetActive(false);
        triggersGroup.SetActive(false);
        goalsGroup.SetActive(false);
        utilityGroup.SetActive(false);
        saveGame.SetActive(false);

        if (isSameGroup)
        {
            contentPanel.SetActive(false);
            verticalScrollbar.SetActive(false);
            currentVisibleGroup = null;
        }
        else
        {
            selectedGroup.SetActive(true);
            contentPanel.SetActive(true);
            verticalScrollbar.SetActive(true);
            currentVisibleGroup = selectedGroup;
        }
    }
}
