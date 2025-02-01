using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (LevelManager.currentLevel.sceneName.Equals("MainMenu"))
        {
            GameObject slider = GameObject.FindGameObjectWithTag("PauseVolumeSlider");
            if (slider != null) 
            {
                slider.GetComponent<Slider>().value = volume;

            }
        }
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
}
