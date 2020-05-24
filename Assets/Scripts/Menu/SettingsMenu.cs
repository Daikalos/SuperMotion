using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Toggle m_FullscreenToggle;
    public TMP_Dropdown m_ResolutionDropdown;
    public TMP_Dropdown m_TextureQualityDropdown;
    public TMP_Dropdown m_ShadowQualityDropdown;
    public TMP_Dropdown m_AntiAliasingDropdown;
    public TMP_Dropdown m_VSyncDropdown;
    public Slider m_MouseSensSlider;
    public Slider m_MasterVolumeSlider;
    public Slider m_MusicVolumeSlider;
    public Slider m_EffectsVolumeSlider;
    public TMP_Text m_MasterVolumeText;
    public TMP_Text m_MusicVolumeText;
    public TMP_Text m_EffectsVolumeText;
    public TMP_Text m_MouseSensText;
    public Button m_ApplyButton;

    void Start()
    {
        m_MasterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChange(); });
        m_MusicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        m_EffectsVolumeSlider.onValueChanged.AddListener(delegate { OnEffectsVolumeChange(); });
        m_MouseSensSlider.onValueChanged.AddListener(delegate { OnMouseSensChange(); });
        m_ApplyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
    }

    private void OnMasterVolumeChange()
    {
        m_MasterVolumeText.text = "Master Volume = " + (int)(m_MasterVolumeSlider.value * 100) + "%";
    }

    private void OnMusicVolumeChange()
    {
        m_MusicVolumeText.text = "Music Volume = " + (int)(m_MusicVolumeSlider.value * 100) + "%";
    }

    private void OnEffectsVolumeChange()
    {
        m_EffectsVolumeText.text = "Sound Effects Volume = " + (int)(m_EffectsVolumeSlider.value * 100) + "%";
    }

    private void OnMouseSensChange()
    {
        m_MouseSensText.text = "Mouse Sensitivity = " + m_MouseSensSlider.value;
    }

    private void OnApplyButtonClick()
    {
        SettingsManager.Instance.SaveSettings(SaveGameSettings());
        SettingsManager.Instance.LoadSettings();
    }

    private GameSettings SaveGameSettings()
    {
        GameSettings currentSettings = new GameSettings();

        currentSettings.IsFullscreen = m_FullscreenToggle.isOn;
        currentSettings.ResolutionIndex = m_ResolutionDropdown.value;
        currentSettings.TextureQuality = (5 - m_TextureQualityDropdown.value);
        currentSettings.ShadowQuality = m_ShadowQualityDropdown.value;
        currentSettings.AntiAliasing = m_AntiAliasingDropdown.value;
        currentSettings.VerticalSync = m_VSyncDropdown.value;

        currentSettings.MasterVolume = m_MasterVolumeSlider.value;
        currentSettings.MusicVolume = m_MusicVolumeSlider.value;
        currentSettings.EffectsVolume = m_EffectsVolumeSlider.value;
        currentSettings.MouseSensitivity = m_MouseSensSlider.value;

        return currentSettings;
    }

    public void UpdateSettingsMenu()
    {
        GameSettings gameSettings = SettingsManager.Instance.GameSettings;

        m_FullscreenToggle.isOn = gameSettings.IsFullscreen;

        m_ResolutionDropdown.value = gameSettings.ResolutionIndex;
        m_TextureQualityDropdown.value = (5 - gameSettings.TextureQuality);
        m_ShadowQualityDropdown.value = gameSettings.ShadowQuality;
        m_AntiAliasingDropdown.value = gameSettings.AntiAliasing;
        m_VSyncDropdown.value = gameSettings.VerticalSync;

        if (m_ResolutionDropdown.options.Count == 0)
        {
            foreach (Resolution resolution in SettingsManager.Instance.Resolutions)
            {
                m_ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            }
        }
        m_ResolutionDropdown.value = gameSettings.ResolutionIndex;

        m_ResolutionDropdown.RefreshShownValue();

        m_MasterVolumeSlider.value = gameSettings.MasterVolume;
        m_MasterVolumeText.text = "Master Volume = " + (int)(m_MasterVolumeSlider.value * 100) + "%";

        m_MusicVolumeSlider.value = gameSettings.MusicVolume;
        m_MusicVolumeText.text = "Music Volume = " + (int)(m_MusicVolumeSlider.value * 100) + "%";

        m_EffectsVolumeSlider.value = gameSettings.EffectsVolume;
        m_EffectsVolumeText.text = "Sound Effects Volume = " + (int)(m_EffectsVolumeSlider.value * 100) + "%";

        m_MouseSensSlider.value = gameSettings.MouseSensitivity;
        m_MouseSensText.text = "Mouse Sensitivity = " + m_MouseSensSlider.value;
    }
}
