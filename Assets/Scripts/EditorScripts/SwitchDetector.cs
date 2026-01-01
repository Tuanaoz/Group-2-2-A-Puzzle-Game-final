using UnityEngine;

public class SwitchDetector : MonoBehaviour
{
    private GridManager gridManager;

    void Start()
    {
        gridManager = Object.FindFirstObjectByType<GridManager>();
    }

    void Update()
    {
        Vector3 characterPos = transform.position;

        var switchTile = gridManager.GetSwitchAtWorld(characterPos);
        if (switchTile != null)
        {
            switchTile.SetPressed(true);
        }
    }
}
