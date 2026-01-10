using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionBehaviour : MonoBehaviour
{
    [Header("UI References")]
    public LevelCompleteBehaviour levelCompleteUI;
    public GameFailBehaviour gameFailUI;

    [Header("Visual Settings")]
    [SerializeField] float colorLerpSpeed = 5f;
    [SerializeField] Color highlightColor = Color.green;

    [Header("Audio")]
    public AudioClip tileSound;
    public AudioClip eatSound;

    // Private references
    private GridManager gridManager;
    public AudioSource source;
    private CharacterMovement movement;
    
    // State Tracking
    private Dictionary<Renderer, Color[]> highlightedDirections = new Dictionary<Renderer, Color[]>();
    private Dictionary<Renderer, Coroutine> activeCoroutines = new Dictionary<Renderer, Coroutine>();
    private bool isSpeedTile = false;
    private float initSpeed;
    private int score = 0;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        // Finding Managers and UI
        gameFailUI = Object.FindFirstObjectByType<GameFailBehaviour>(FindObjectsInactive.Include);
        levelCompleteUI = FindFirstObjectByType<LevelCompleteBehaviour>(FindObjectsInactive.Include);
        gridManager = Object.FindFirstObjectByType<GridManager>();
        
        // Component References
        movement = GetComponent<CharacterMovement>();
        
        if (movement != null)
            initSpeed = movement.speed;

        // Validation
        if (levelCompleteUI == null) Debug.LogWarning("LevelCompleteBehaviour not found!");
        if (gameFailUI == null) Debug.LogWarning("GameFailBehaviour not found!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isLevelEditor = SceneManager.GetActiveScene().name == "LevelEditor";

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Rotatable"))
        {
            // if in level editor, respawn instead of game over
            if (isLevelEditor) { gridManager.RespawnPlayer(); return; }
            
            Debug.Log("Game Over");
            gameFailUI.ShowFail();
            
            if (collision.gameObject.CompareTag("Rotatable"))
                Destroy(this.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            // if in level editor, respawn instead of level complete
            if (isLevelEditor) { gridManager.RespawnPlayer(); return; }
            levelCompleteUI.ShowLevelComplete();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Determine if the character hit the center of the trigger
        Vector3 characterHitPoint = transform.position;
        Vector3 triggerHitPoint = other.transform.InverseTransformPoint(characterHitPoint);
        bool hitCenter = Mathf.Abs(triggerHitPoint.x) < 0.1f && Mathf.Abs(triggerHitPoint.z) < 0.1f;

        // Tile Interactions
        if (other.gameObject.CompareTag("SpeedTile"))
        {
            if (hitCenter) transform.forward = other.transform.forward;
            movement.speed = initSpeed + 20f;
            isSpeedTile = true;
        }
        else if (other.gameObject.CompareTag("Direction") || other.gameObject.CompareTag("Food"))
        {
            if (hitCenter)
            {
                transform.forward = other.transform.forward;
                HighlightDirection(other.transform.GetChild(0).GetComponent<Collider>());
            }
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            levelCompleteUI.ShowLevelComplete();
        }
        else if (other.gameObject.CompareTag("Switch"))
        {
            Switch switchScript = other.GetComponent<Switch>();
            if (switchScript != null) switchScript.SetPressed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isLevelEditor = SceneManager.GetActiveScene().name == "LevelEditor";

        // Sound Effects
        if (other.CompareTag("Direction") && tileSound != null)
        {
            source.PlayOneShot(tileSound);
        }

        if (other.CompareTag("Food"))
        {
            source.PlayOneShot(eatSound);
            score++;
            GameObject scoreObj = GameObject.FindGameObjectWithTag("Score");
            if (scoreObj != null) scoreObj.GetComponent<Text>().text = "Score: " + score;
            
            FoodHandler fh = other.GetComponent<FoodHandler>();
            if (fh != null) fh.enabled = true;
        }

        // Hazards (Spikes, Acid, Lava)
        if (other.CompareTag("Acid") || other.CompareTag("Lava") || other.CompareTag("Spikes"))
        {
            if (other.CompareTag("Spikes"))
            {
                SpikeBehaviour sb = other.GetComponent<SpikeBehaviour>();
                if (sb == null || !sb.IsOn()) return; // Don't die if spikes are off
            }

            if (isLevelEditor) { gridManager.RespawnPlayer(); return; }
            
            gameFailUI.ShowFail();
            Destroy(this.gameObject);
            return;
        }

        // Door Logic
        Door door = other.GetComponent<Door>();
        if (door != null)
        {
            if (isLevelEditor) { gridManager.RespawnPlayer(); return; }

            if (door.IsOpen()) levelCompleteUI.ShowLevelComplete();
            else gameFailUI.ShowFail();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isSpeedTile && other.CompareTag("SpeedTile"))
        {
            movement.speed = initSpeed;
            isSpeedTile = false;
        }

        if (other.CompareTag("Direction"))
        {
            ResetDirectionColor(other.transform.GetChild(0).GetComponent<Collider>());
        }
    }

    // ================= DIRECTION COLOR HANDLING =================

    void HighlightDirection(Collider direction)
    {
        Renderer rend = direction.GetComponent<Renderer>() ?? direction.GetComponentInChildren<Renderer>();
        if (rend == null) return;

        if (!highlightedDirections.ContainsKey(rend))
        {
            Color[] originalColors = new Color[rend.materials.Length];
            for (int i = 0; i < rend.materials.Length; i++)
                originalColors[i] = rend.materials[i].color;
            highlightedDirections[rend] = originalColors;
        }

        if (activeCoroutines.ContainsKey(rend) && activeCoroutines[rend] != null)
            StopCoroutine(activeCoroutines[rend]);

        activeCoroutines[rend] = StartCoroutine(LerpColor(rend, highlightColor));
    }

    void ResetDirectionColor(Collider direction)
    {
        Renderer rend = direction.GetComponent<Renderer>() ?? direction.GetComponentInChildren<Renderer>();
        if (rend == null || !highlightedDirections.ContainsKey(rend)) return;

        if (activeCoroutines.ContainsKey(rend) && activeCoroutines[rend] != null)
            StopCoroutine(activeCoroutines[rend]);

        activeCoroutines[rend] = StartCoroutine(LerpColor(rend, highlightedDirections[rend], removeAfter: true));
    }

    IEnumerator LerpColor(Renderer rend, Color target, bool removeAfter = false)
    {
        Material[] mats = rend.materials;
        while (true)
        {
            bool done = true;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = Color.Lerp(mats[i].color, target, Time.deltaTime * colorLerpSpeed);
                if (Vector4.Distance(mats[i].color, target) > 0.01f) done = false;
            }
            if (done) break;
            yield return null;
        }
        if (removeAfter) { activeCoroutines.Remove(rend); highlightedDirections.Remove(rend); }
    }

    IEnumerator LerpColor(Renderer rend, Color[] targets, bool removeAfter)
    {
        Material[] mats = rend.materials;
        while (true)
        {
            bool done = true;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = Color.Lerp(mats[i].color, targets[i], Time.deltaTime * colorLerpSpeed);
                if (Vector4.Distance(mats[i].color, targets[i]) > 0.01f) done = false;
            }
            if (done) break;
            yield return null;
        }
        if (removeAfter) { activeCoroutines.Remove(rend); highlightedDirections.Remove(rend); }
    }
}