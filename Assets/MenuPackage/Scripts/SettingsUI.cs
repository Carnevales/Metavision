using UnityEngine;
using UnityEngine.UI;

/// <summary>Referências do painel de configurações de áudio.</summary>
public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private PanelManager panelManager;

    public Slider MasterVolumeSlider => masterVolumeSlider;
    public Slider MusicVolumeSlider => musicVolumeSlider;
    public Slider SfxVolumeSlider => sfxVolumeSlider;

    public void Close()
    {
        if (panelManager != null) panelManager.ShowLobby();
    }
}
