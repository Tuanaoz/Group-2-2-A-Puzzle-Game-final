using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Checks if checkpoint was already used
    private bool triggered = false;

// Checks for correct tag, makes sure it was triggered once and sends location of the checkpoint
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            GridManager gridManager = FindAnyObjectByType<GridManager>();
            if (gridManager != null)
            {
                gridManager.TriggerCheckpoint(transform.position);
            }
        }
    }
}
