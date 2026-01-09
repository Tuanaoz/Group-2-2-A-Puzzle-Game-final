using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
public class CharacterData {
    public Vector3 position;
    public Quaternion rotation;

    public CharacterData(Vector3 pos, Quaternion rot) {
        position = pos;
        rotation = rot;
    }
}

[Serializable]
public class PlacedObjectData {
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;

    public PlacedObjectData(string name, Vector3 pos, Quaternion rot) {
        prefabName = name;
        position = pos;
        rotation = rot;
    }
}


[Serializable]
public class LevelData {
    public string levelName;
    public List<PlacedObjectData> placedObjects = new List<PlacedObjectData>();
    public List<CharacterData> characters = new List<CharacterData>();
    public Vector3 grounPosition;
    public Vector3 groundScale;
    public int groundThemeIndex;
    public Vector3 northArrowsPosition;
    public Vector3 southArrowsPosition;
    public Vector3 eastArrowsPosition;
    public Vector3 westArrowsPosition;
    public int bonusCollectibleCount;
    public int bonusCollected;
    public float cameraMinBoundX;
    public float cameraMaxBoundX;
    public float cameraMinBoundZ;
    public float cameraMaxBoundZ;
}

public class SaveLoadManager : MonoBehaviour
{
    public Transform placementContainer;
    public GameObject[] placeablePrefabs;
    public GridManager gridManager;
    public GroundTheme groundTheme;

    public Transform levelPanel;
    public Transform levelSelectionUI;

    public TMP_InputField levelNameInput;
    public CameraMovement mainCameraMovement;

    public OverwriteLevelUI overwriteLevelUI;
    public SaveLevelUI saveLevelUI;

    public Button New_Button;
    public Button Share_Button;

    public TMP_InputField pasteLevelInputField;

    private int charCount;
    private bool share = false;

    [Header("Level Settings")]
    public string saveFileName = "tempLevelData";
    private string levelFolder;
    private string loadedLevelName = null;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "PlayLevel" &&
            !string.IsNullOrEmpty(LevelLoadRequest.RequestedLevelName))
        {
            Debug.Log("Loading requested level: " + LevelLoadRequest.RequestedLevelName);
            if (LevelLoadRequest.IsCustomLevel) {
                Debug.Log("Loading custom level: " + LevelLoadRequest.RequestedLevelName);
                LoadCustomLevel(LevelLoadRequest.RequestedLevelName);
            } else {
                LoadLevelFromResources(LevelLoadRequest.RequestedLevelName);
            }
            // LevelLoadRequest.RequestedLevelName = null;
            return;
        }
        if (currentScene.name=="LevelEditor" && gridManager != null) {
            gridManager.UIToggle();
        }
        levelFolder = Application.persistentDataPath + "/CreatedLevels/";
        if (!Directory.Exists(levelFolder)) {
            Directory.CreateDirectory(levelFolder);
        }
        if (currentScene.name == "LevelEditor") {
            string[] levels = Directory.GetFiles(levelFolder, "*.json");
            foreach (string level in levels) {
                string levelName = Path.GetFileNameWithoutExtension(level);
                Debug.Log("Found level in CreatedLevels: " + levelName);
                GameObject prefab = Array.Find(placeablePrefabs, p => p.name == "Level_Button");
                GameObject btnObj = Instantiate(prefab, levelPanel);
                Button btn = btnObj.GetComponent<Button>();
                btn.GetComponentInChildren<TMP_Text>().text = levelName;
                btn.onClick.AddListener(() => LoadLevel(levelName));
                btn.onClick.AddListener(() => CopyLevelToClipboard(levelName));
            }
        } else if (currentScene.name == "LevelSelection") {
            PopulateCustomLevelPanel();
        }
    }

    public void PopulateCustomLevelPanel() {
        string customLevelFolder = Application.persistentDataPath + "/Custom/";

        if (!Directory.Exists(customLevelFolder)) {
            Directory.CreateDirectory(customLevelFolder);
        }

        string[] levels = Directory.GetFiles(customLevelFolder, "*.json");
        
        for(int i = 0; i < levels.Length; i++)
        {
            string levelPath = levels[i];
            string levelName = Path.GetFileNameWithoutExtension(levelPath);

            Debug.Log("Found level in Custom: " + levelName);
            GameObject prefab = Array.Find(placeablePrefabs, p => p.name == "Level_Button");
            GameObject btnObj = Instantiate(prefab, levelPanel);
            Button btn = btnObj.GetComponent<Button>();

            btn.GetComponentInChildren<TMP_Text>().text = levelName;

            btn.onClick.AddListener(() => PlayCustomLevel(levelName));
        }
    }

    public void PlayCustomLevel(string levelName) {
        LevelLoadRequest.RequestedLevelName = levelName;
        LevelLoadRequest.IsCustomLevel = true;
        SceneManager.LoadScene("PlayLevel");
    }

    public void LevelAlreadyExists() {
        levelFolder = Application.persistentDataPath + "/CreatedLevels/";
        if (!Directory.Exists(levelFolder)) {
            Directory.CreateDirectory(levelFolder);
        }

        if (File.Exists(levelFolder + saveFileName + ".json")) {
            Debug.Log("Level already exists: " + saveFileName);
            overwriteLevelUI.OpenOverwriteUI();
        } else {
            SaveLevel();
        }
    }

    public void SaveLevel() {
        charCount = 0;
        LevelData levelData = new LevelData();
        levelData.levelName = saveFileName;

        levelData.grounPosition = gridManager.getGroundPosition();
        levelData.groundScale = gridManager.getGroundScale();
        levelData.groundThemeIndex = groundTheme.currentThemeIndex;
        levelData.northArrowsPosition = gridManager.getNorthArrowsPosition();
        levelData.southArrowsPosition = gridManager.getSouthArrowsPosition();
        levelData.eastArrowsPosition = gridManager.getEastArrowsPosition();
        levelData.westArrowsPosition = gridManager.getWestArrowsPosition();

        levelData.cameraMinBoundX = mainCameraMovement.GetMinBoundX();
        levelData.cameraMaxBoundX = mainCameraMovement.GetMaxBoundX();
        levelData.cameraMinBoundZ = mainCameraMovement.GetMinBoundZ();
        levelData.cameraMaxBoundZ = mainCameraMovement.GetMaxBoundZ();

        foreach (Transform child in placementContainer) {

            String prefabName = child.gameObject.name.Replace("(Clone)", "").Trim();

            if (prefabName.Contains("Character")) {
                CharacterData charData = new CharacterData(
                    gridManager.getPlayerSpawnPosition(charCount),
                    gridManager.getPlayerSpawnRotation(charCount)
                );
                levelData.characters.Add(charData);
                charCount++;
                continue;
            }

            PlacedObjectData objData = new PlacedObjectData(
                prefabName,
                child.position,
                child.rotation
            );
            levelData.placedObjects.Add(objData);
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(levelFolder + saveFileName + ".json", json);

        WebGLFileSystem.Sync();

        overwriteLevelUI.CloseOverwriteUI();
        saveLevelUI.CloseSaveUI();
        if (gridManager.isUIOpen())
        {
            gridManager.UIToggle();
        }
    }

    public void LoadLevel(string levelName) {

        if (share)
            return;

        loadedLevelName = levelName;
        saveFileName = levelName;
        levelNameInput.text = levelName;
        string filePath = levelFolder + "/" + levelName + ".json";
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        LoadLevelFromData(levelData);
    }

    public void LoadLevelFromResources(string levelName) {
        TextAsset levelFile = Resources.Load<TextAsset>("Levels/" + levelName);
        if (levelFile == null) return;

        LevelData levelData = JsonUtility.FromJson<LevelData>(levelFile.text);
        LoadLevelFromData(levelData);
    }

    public void LoadCustomLevel(string levelName) {
        string levelFilePath = Application.persistentDataPath + "/Custom/" + levelName + ".json";
        if (!File.Exists(levelFilePath)) {
            Debug.LogError("Custom level file not found: " + levelFilePath);
            return;
        }

        string json = File.ReadAllText(levelFilePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        LoadLevelFromData(levelData); 
    }

    private void LoadLevelFromData(LevelData levelData) {
        foreach (Transform child in placementContainer) {
            Destroy(child.gameObject);
        }

        if (SceneManager.GetActiveScene().name == "LevelEditor") {
            gridManager.setArrowPositions(
                levelData.northArrowsPosition,
                levelData.southArrowsPosition,
                levelData.eastArrowsPosition,
                levelData.westArrowsPosition
            );
        }

        GameObject ground = GameObject.Find("Ground");
        ground.transform.position = levelData.grounPosition;
        ground.transform.localScale = levelData.groundScale;

        mainCameraMovement.setBounds(
            levelData.cameraMinBoundX,
            levelData.cameraMaxBoundX,
            levelData.cameraMinBoundZ,
            levelData.cameraMaxBoundZ
        );


        GroundTheme themeController=FindFirstObjectByType<GroundTheme>();
            if (themeController != null)
            {
                themeController.SetTheme(levelData.groundThemeIndex);
            }

        foreach (CharacterData charData in levelData.characters) {
            GameObject prefab = Array.Find(placeablePrefabs, p => p.name.Contains("Character"));
            if (prefab != null) {
                Instantiate(prefab, charData.position, charData.rotation, placementContainer);
                gridManager.addCharacterStartPosition(charData.position);
                gridManager.addCharacterStartRotation(charData.rotation);
            }
        }

        foreach (PlacedObjectData objData in levelData.placedObjects) {
            GameObject prefab = Array.Find(placeablePrefabs, p => p.name == objData.prefabName);
            if (prefab != null) {
                Instantiate(prefab, objData.position, objData.rotation, placementContainer);
            }
        }
        HideLevelSelectionUI();
    }

    public void HideLevelSelectionUI() {
        levelSelectionUI.gameObject.SetActive(false);
        if (gridManager!=null && SceneManager.GetActiveScene().name == "LevelEditor"){
            gridManager.UIToggle();
        }
        mainCameraMovement.StartMovement();
    }

    public void updateLevelName() {
        saveFileName = levelNameInput.text;
    }

    public bool IsEditingLoadedLevel()
    {
        return !string.IsNullOrEmpty(loadedLevelName);
    }

    public void OpenOverwriteFromEdit()
    {
        overwriteLevelUI.OpenOverwriteUI();
    }

    public void OpenSaveAsNew()
    {
        loadedLevelName = null;
        saveFileName = "";
        levelNameInput.text = "";

        saveLevelUI.gameObject.SetActive(true);
        gridManager.UIToggle();
    }

    public void ToggleShareMode() {
        share = !share;
    }

    public void CopyLevelToClipboard(string levelName)
    {

        if (!share)
            return;

        string filePath = levelFolder + "/" + levelName + ".json";
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        string minifyedJson = Regex.Replace(json, @"\s+", "");

        GUIUtility.systemCopyBuffer = minifyedJson;
        Debug.Log("Level data copied to clipboard for level: " + levelName);

        ToggleShareMode();
        New_Button.gameObject.SetActive(true);
        Share_Button.gameObject.SetActive(true);
    }

    private void SaveCustomLevel(LevelData levelData) {
        string folderPath = Application.persistentDataPath + "/Custom/";

        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = levelData.levelName;
        if (string.IsNullOrEmpty(fileName)) {
            fileName = "CustomLevel_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        string filePath = folderPath + fileName + ".json";

        if (File.Exists(filePath)) {
            Debug.LogWarning("Custom level already exists: " + fileName);
            return;
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Custom level saved: " + fileName);

    }

    public void LoadLevelFromJson() {
        string json = pasteLevelInputField.text;
        if (string.IsNullOrEmpty(json)) {
            Debug.LogWarning("No level data provided in input field.");
            return;
        }

        LevelData levelData;
        try {
            levelData = JsonUtility.FromJson<LevelData>(json);
        } catch (Exception e) {
            Debug.LogError("Failed to parse level data from json: " + e.Message);
            return;
        }

        if (levelData == null) {
            Debug.LogError("Parsed level data is null.");
            return;
        }

        SaveCustomLevel(levelData);

        LevelLoadRequest.RequestedLevelName = levelData.levelName;
        LevelLoadRequest.IsCustomLevel = true;
        SceneManager.LoadScene("PlayLevel");
    }
}