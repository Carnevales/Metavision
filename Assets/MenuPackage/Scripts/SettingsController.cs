using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>Aplica e persiste os volumes dos sliders usando PlayerPrefs.</summary>
public class SettingsController : MonoBehaviour
{
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private AudioMixer audioMixer;
    [Header("AudioMixer exposed parameters")]
    [SerializeField] private string musicParameter = "MusicVolume";
    [SerializeField] private string sfxParameter = "SfxVolume";

    private const string MusicKey = "Menu.MusicVolume";
    private const string SfxKey = "Menu.SfxVolume";

    private void Awake()
    {
        if (settingsUI == null) return;
        SetupSlider(settingsUI.MusicVolumeSlider, MusicKey, SetMusicVolume);
        SetupSlider(settingsUI.SfxVolumeSlider, SfxKey, SetSfxVolume);
    }

    private void SetupSlider(Slider slider, string key, UnityEngine.Events.UnityAction<float> callback)
    {
        if (slider == null) return;
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(key, 1f));
        callback(slider.value);
        slider.onValueChanged.AddListener(callback);
    }
    public void SetMusicVolume(float value) => SetVolume(musicParameter, MusicKey, value);
    public void SetSfxVolume(float value) => SetVolume(sfxParameter, SfxKey, value);

    private void SetVolume(string mixerParameter, string preferenceKey, float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(preferenceKey, value);
        PlayerPrefs.Save();

        if (audioMixer != null && !string.IsNullOrWhiteSpace(mixerParameter))
            audioMixer.SetFloat(mixerParameter, LinearToDecibels(value));
    }

    private static float LinearToDecibels(float value)
    {
        return value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
    }
}
