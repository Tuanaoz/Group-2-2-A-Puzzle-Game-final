using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public Slider masterVolumeSlider;
    private const string MasterVolumeKey = "MasterVolume";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        AudioListener.volume = savedVolume;

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = savedVolume;
            masterVolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        PlayerPrefs.Save();
    }
}