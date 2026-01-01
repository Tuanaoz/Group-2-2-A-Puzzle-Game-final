using UnityEngine;

public class Switch : MonoBehaviour
{
    public Door connectedToDoor;
    public Vector2Int moveDirection;
    private bool isPressed = false;

    public void SetPressed(bool state)
    {
        if (isPressed == state) return;
        isPressed = state;
        connectedToDoor.SetOpen(isPressed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SetPressed(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        SetPressed(false);
    }
}
