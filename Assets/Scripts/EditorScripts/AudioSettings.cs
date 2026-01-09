using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private const string MuteKey = "SoundMuted";

    void Start()
    {
        bool isMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

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

        PlayerPrefs.Save();
    }
}