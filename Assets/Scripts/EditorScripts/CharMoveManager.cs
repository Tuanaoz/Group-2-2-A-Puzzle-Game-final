using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharMoveManager : MonoBehaviour
{
    public Button buttonStart;
    public Button buttonPause;
    public Transform placementContainer;
    public GameObject FailPanel;
    private List<CharacterMovement> characterMovements = new List<CharacterMovement>();
    private bool gotCharacters = false;

    void Start() {
        buttonStart.gameObject.SetActive(true);
        buttonPause.gameObject.SetActive(false);
    }

// Checks if the object is character, gets movement script and adds  character to list
    private void GetAllCharacters() {
        foreach (Transform child in placementContainer) {
            if (!child.name.Contains("Character")) {
                continue;
            }
            CharacterMovement movementScript = child.GetComponent<CharacterMovement>();
            if (movementScript != null) {
                characterMovements.Add(movementScript);
            }
        }
    }

// Starts movement for all characters and updates buttons UI
    public void StartCharMovement() {
        if (!gotCharacters) {
            GetAllCharacters();
            gotCharacters = true;
        }
        foreach (CharacterMovement cm in characterMovements) {
            cm.StartMovement();
        }
        buttonStart.gameObject.SetActive(false);
        buttonPause.gameObject.SetActive(true);
    }

// Pauses movement for all characters and updates buttons UI
    public void PauseCharMovement() {
        foreach (CharacterMovement cm in characterMovements) {
            cm.PauseMovement();
        }
        buttonStart.gameObject.SetActive(true);
        buttonPause.gameObject.SetActive(false);
    }

// Clears the character list
    public void clearCharacters() {
        gotCharacters = false;
        characterMovements.Clear();
    }
    
// Resets all character positions and buttons UI also hides fail panel
    public void ResetLevel()
    {
        FailPanel.SetActive(false);
        foreach (CharacterMovement cm in characterMovements)
        {
            cm.ResetPosition();
        }
        buttonStart.gameObject.SetActive(true);
        buttonPause.gameObject.SetActive(false);
    }
}
