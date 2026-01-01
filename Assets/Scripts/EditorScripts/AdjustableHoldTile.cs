using UnityEngine;

public class AdjustableHoldTile : MonoBehaviour
{
    public float holdTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HoldPlayer controller = other.GetComponent<HoldPlayer>();
            if (controller != null)
            {
                controller.StartCoroutine(controller.HoldMovement(holdTime));
            }
        }
    }
}
