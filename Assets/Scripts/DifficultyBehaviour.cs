using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DifficultyBehaviour
{
    public static bool hardDifficulty = false;

    public static void ToggleDifficulty()
    {
        hardDifficulty = !hardDifficulty;
    }
}
