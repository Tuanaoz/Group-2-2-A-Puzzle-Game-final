using UnityEngine;

public class CustomWorld : MonoBehaviour
{
    public LockedPopup lockedPopup;
    public GameObject customWorldPanel;

    public void OnClick()
    {
// If all main levels are not completed, show warning
        if (!ProgressManager.AllLevelsCompleted())
        {
            Debug.Log("All main levels not completed. Cannot access custom world.");
            if (lockedPopup != null) {
                lockedPopup.Show();
            }

            return;
        }

//open custom world panel when all levels completed
        if (customWorldPanel != null) {
            customWorldPanel.SetActive(true);
        }
    }
}
