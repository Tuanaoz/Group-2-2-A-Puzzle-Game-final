using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private const string MuteKey = "SoundMuted";

// Check if sound was muted before, sets volume based on saved value
    void Start()
    {
        bool isMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

// Checks current state of the sound and turns on or off accordingly
    public void ToggleSound()
    {
        bool isMuted = AudioListener.volume == 0f;

        if (isMuted)
        {
            AudioListener.volume = 1f;
            PlayerPrefs.SetInt(MuteKey, 0);
        }
        else
        {
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt(MuteKey, 1);
        }

        PlayerPrefs.Save(); // Saves the changed settings
    }
}