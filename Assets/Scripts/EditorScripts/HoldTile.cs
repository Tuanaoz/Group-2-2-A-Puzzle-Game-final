using UnityEngine;

public class HoldTile : MonoBehaviour
{
    public float holdDuration = 2f;
// How long the player is stopped

// Checks if the tag is Player and stops player movement for a spesific time
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        HoldPlayer holdPlayer = other.GetComponent<HoldPlayer>();
        if (holdPlayer != null)
        {
            StartCoroutine(holdPlayer.HoldMovement(holdDuration));
        }
    }
}