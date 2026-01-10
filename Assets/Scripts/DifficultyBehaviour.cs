using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DifficultyBehaviour
{
    public static bool hardDifficulty = false;

    // Toggle difficulty setting
    public static void ToggleDifficulty()
    {
        hardDifficulty = !hardDifficulty;
    }
}
