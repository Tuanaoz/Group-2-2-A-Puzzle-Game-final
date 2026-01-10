using UnityEngine;
using UnityEngine.SceneManagement;

//Reload current scene
public class RetryButton : MonoBehaviour
{
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}