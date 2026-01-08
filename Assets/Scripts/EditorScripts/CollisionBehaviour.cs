using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionBehaviour : MonoBehaviour
{
    private LevelCompleteBehaviour levelCompleteUI;

    private GameFailBehaviour gameFailUI;

    private GridManager gridManager;

    void Start()
    {
        gameFailUI = Object.FindFirstObjectByType<GameFailBehaviour>(FindObjectsInactive.Include);

        // Auto-find the LevelCompleteBehaviour in the scene
        levelCompleteUI = FindFirstObjectByType<LevelCompleteBehaviour>(FindObjectsInactive.Include);

        gridManager = Object.FindFirstObjectByType<GridManager>();

        if (levelCompleteUI == null)
        {
            Debug.LogWarning("LevelCompleteBehaviour not found in scene!");
        }

        if (gameFailUI == null)
        {
            Debug.LogWarning("GameFailBehaviour not found in scene!");
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                gridManager.RespawnPlayer();
                return;
            }
            Debug.Log("Game Over");
            gameFailUI.ShowFail();
            return;
        }

        if (collision.gameObject.tag == "Rotatable")
        {
            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                gridManager.RespawnPlayer();
                return;
            }
            Debug.Log("Collision occurred with object");
            Debug.Log("Game Over");
            gameFailUI.ShowFail();
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Goal") {
            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                gridManager.RespawnPlayer();
                return;
            }
            levelCompleteUI.ShowLevelComplete();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Direction") {
            Vector3 characterHitPoint = transform.position;
            Vector3 triggerHitPoint = other.transform.InverseTransformPoint(characterHitPoint);

            if (Mathf.Abs(triggerHitPoint.x) < 0.1f && Mathf.Abs(triggerHitPoint.z) < 0.1f) {
                // Hit near the center
                transform.forward = other.transform.forward;
            }
        } else if (other.gameObject.tag == "Goal") {
            levelCompleteUI.ShowLevelComplete();
        } else if (other.gameObject.tag == "Switch") {
            Switch switchScript = other.GetComponent<Switch>();
            if (switchScript != null) {
                switchScript.SetPressed();
            }
        } else if (other.gameObject.tag == "Spikes") {
            SpikeBehaviour spikeBehaviour = other.GetComponent<SpikeBehaviour>();
            if (spikeBehaviour != null && spikeBehaviour.IsOn()) {
                if (SceneManager.GetActiveScene().name == "LevelEditor") {
                    gridManager.RespawnPlayer();
                    return;
                }
                Debug.Log("Game Over");
                gameFailUI.ShowFail();
                Destroy(this.gameObject);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
// Check if player touched a door
    Door door = other.GetComponent<Door>();
    if (door != null)
    {
        if (door.IsOpen())
        {
// Door is open, level completed
            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                gridManager.RespawnPlayer();
                return;
            }
            levelCompleteUI.ShowLevelComplete();
        }
        else
        {
// Door is closed, game over
            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                gridManager.RespawnPlayer();
                return;
            }
            gameFailUI.ShowFail();
        }
        return;
    }

    if (other.gameObject.tag == "Spikes") {
        SpikeBehaviour spikeBehaviour = other.GetComponent<SpikeBehaviour>();
        if (spikeBehaviour != null && spikeBehaviour.IsOn()) {
            if (SceneManager.GetActiveScene().name == "LevelEditor") {
                gridManager.RespawnPlayer();
                return;
            }
            Debug.Log("Game Over");
            gameFailUI.ShowFail();
            Destroy(this.gameObject);
            return;
        }
    }

        if (other.CompareTag("Acid") || other.CompareTag("Lava"))
        {
            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                gridManager.RespawnPlayer();
                return;
            }
            Debug.Log("Game Over");
            gameFailUI.ShowFail();
            Destroy(this.gameObject);
        }
    }
}
