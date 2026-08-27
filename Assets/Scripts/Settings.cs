using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider generalSlider;
    public Slider soundsSlider;
    public Slider musicSlider;

    public KeyCode CloseKey = KeyCode.Escape;

    public void Start()
    {
        generalSlider.value = PlayerPrefs.GetFloat("GeneralVolume", 1f) * 100;
        soundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume", 1f) * 100;
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f) * 100;
        GeneralChanged();
        SoundsChanged();
        MusicChanged();
    }

    public void GeneralChanged()
    {
        float volume = generalSlider.value / 100f;
        PlayerPrefs.SetFloat("GeneralVolume", volume);
        mixer.SetFloat("GeneralVolume", volume * 80 - 80);
    }

    public void SoundsChanged()
    {
        float volume = soundsSlider.value / 100f;
        PlayerPrefs.SetFloat("SoundsVolume", volume);
        mixer.SetFloat("SoundsVolume", volume * 80 - 80);
    }

    public void MusicChanged()
    {
        float volume = musicSlider.value / 100f;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        mixer.SetFloat("MusicVolume", volume * 80 - 80);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(CloseKey))
        {
            Close();
        }
    }
}
