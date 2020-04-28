using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct GameSettings
{
    public bool IsFullscreen { get; set; }
    public int ResolutionIndex { get; set; }
    public int TextureQuality { get; set; }
    public int ShadowQuality { get; set; }
    public int AntiAliasing { get; set; }
    public int VerticalSync { get; set; }
    public float MasterVolume { get; set; }
    public float MouseSensitivity { get; set; }
}

public class SettingsManager : MonoBehaviour
{
    public Toggle m_FullscreenToggle;
    public TMP_Dropdown m_ResolutionDropdown;
    public TMP_Dropdown m_TextureQualityDropdown;
    public TMP_Dropdown m_ShadowQualityDropdown;
    public TMP_Dropdown m_AntiAliasingDropdown;
    public TMP_Dropdown m_VSyncDropdown;
    public Slider m_MasterVolumeSlider;
    public Slider m_MouseSensSlider;
    public TMP_Text m_MasterVolumeText;
    public TMP_Text m_MouseSensText;
    public Button m_ApplyButton;
    public Resolution[] m_Resolutions;

    private GameSettings m_GameSettings;

    private string m_Fullscreen;
    private string m_ResolutionIndex;
    private string m_TextureQuality;
    private string m_ShadowQuality;
    private string m_AntiAliasing;
    private string m_VerticalSync;
    private string m_MasterVolume;
    private string m_MouseSensitivity;

    void Start()
    {
        m_GameSettings = new GameSettings();

        m_Fullscreen = "IsFullscreen";
        m_ResolutionIndex = "ResolutionIndex";
        m_TextureQuality = "TextureQuality";
        m_ShadowQuality = "ShadowQuality";
        m_AntiAliasing = "AntiAliasing";
        m_VerticalSync = "VerticalSync";
        m_MasterVolume = "MasterVolume";
        m_MouseSensitivity = "MouseSensitivity";

        m_FullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });
        m_ResolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        m_TextureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        m_ShadowQualityDropdown.onValueChanged.AddListener(delegate { OnShadowQualityChange(); });
        m_AntiAliasingDropdown.onValueChanged.AddListener(delegate { OnAntiAliasingChange(); });
        m_VSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        m_MasterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChange(); });
        m_MouseSensSlider.onValueChanged.AddListener(delegate { OnMouseSensChange(); });
        m_ApplyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
        
        m_Resolutions = Screen.resolutions;
        if (m_ResolutionDropdown.options.Count == 0)
        {
            foreach (Resolution resolution in m_Resolutions)
            {
                m_ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            }
        }

        //Predetermined values if PlayerPrefs does not exist
        QualitySettings.SetQualityLevel(5);
        m_MasterVolumeSlider.value = 0.4f;
        m_MouseSensSlider.value = 1.0f;

        LoadSettings();
    }

    private void OnFullScreenToggle()
    {
        m_GameSettings.IsFullscreen = Screen.fullScreen = m_FullscreenToggle.isOn;
        m_FullscreenToggle.graphic.color = new Color(
            m_FullscreenToggle.graphic.color.r,
            m_FullscreenToggle.graphic.color.g,
            m_FullscreenToggle.graphic.color.b,
            Convert.ToInt32(m_GameSettings.IsFullscreen));
    }

    private void OnResolutionChange()
    {
        Screen.SetResolution(m_Resolutions[m_ResolutionDropdown.value].width, m_Resolutions[m_ResolutionDropdown.value].height, Screen.fullScreen);
        m_GameSettings.ResolutionIndex = m_ResolutionDropdown.value;
    }

    private void OnTextureQualityChange()
    {
        m_GameSettings.TextureQuality = (5 - m_TextureQualityDropdown.value);
        QualitySettings.masterTextureLimit = m_GameSettings.TextureQuality;
    }

    private void OnShadowQualityChange()
    {
        m_GameSettings.ShadowQuality = m_ShadowQualityDropdown.value;
        QualitySettings.shadowResolution = (ShadowResolution)m_GameSettings.ShadowQuality;
    }

    private void OnAntiAliasingChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2f, m_AntiAliasingDropdown.value);
        m_GameSettings.AntiAliasing = m_AntiAliasingDropdown.value;
    }

    private void OnVSyncChange()
    {
        m_GameSettings.VerticalSync = m_VSyncDropdown.value;
        QualitySettings.vSyncCount = m_GameSettings.VerticalSync;
    }

    private void OnMasterVolumeChange()
    {
        AudioListener.volume = m_MasterVolumeSlider.value;
        m_GameSettings.MasterVolume = m_MasterVolumeSlider.value;

        m_MasterVolumeText.text = "Master Volume = " + (int)(m_MasterVolumeSlider.value * 100) + "%";
    }

    private void OnMouseSensChange()
    {
        m_GameSettings.MouseSensitivity = m_MouseSensSlider.value;

        m_MouseSensText.text = "Mouse Sensitivity = " + (m_MouseSensSlider.value * 10);
    }

    private void OnApplyButtonClick()
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(m_Fullscreen, m_GameSettings.IsFullscreen ? 1 : 0);
        PlayerPrefs.SetInt(m_TextureQuality, m_GameSettings.TextureQuality);
        PlayerPrefs.SetInt(m_ShadowQuality, m_GameSettings.ShadowQuality);
        PlayerPrefs.SetInt(m_AntiAliasing, m_GameSettings.AntiAliasing);
        PlayerPrefs.SetInt(m_VerticalSync, m_GameSettings.VerticalSync);
        PlayerPrefs.SetInt(m_ResolutionIndex, m_GameSettings.ResolutionIndex);
        PlayerPrefs.SetFloat(m_MasterVolume, m_GameSettings.MasterVolume);
        PlayerPrefs.SetFloat(m_MouseSensitivity, m_GameSettings.MouseSensitivity);
    }

    private void LoadSettings()
    {
        m_GameSettings.IsFullscreen = Convert.ToBoolean(PlayerPrefs.GetInt(m_Fullscreen));
        m_GameSettings.TextureQuality = PlayerPrefs.GetInt(m_TextureQuality);
        m_GameSettings.ShadowQuality = PlayerPrefs.GetInt(m_ShadowQuality);
        m_GameSettings.AntiAliasing = PlayerPrefs.GetInt(m_AntiAliasing);
        m_GameSettings.VerticalSync = PlayerPrefs.GetInt(m_VerticalSync);
        m_GameSettings.ResolutionIndex = PlayerPrefs.GetInt(m_ResolutionIndex);
        m_GameSettings.MasterVolume = PlayerPrefs.GetFloat(m_MasterVolume);
        m_GameSettings.MouseSensitivity = PlayerPrefs.GetFloat(m_MouseSensitivity);

        m_FullscreenToggle.isOn = m_GameSettings.IsFullscreen;
        m_FullscreenToggle.graphic.color = new Color(
            m_FullscreenToggle.graphic.color.r,
            m_FullscreenToggle.graphic.color.g,
            m_FullscreenToggle.graphic.color.b,
            Convert.ToInt32(m_GameSettings.IsFullscreen));

        m_ResolutionDropdown.value = m_GameSettings.ResolutionIndex;
        m_TextureQualityDropdown.value = (5 - m_GameSettings.TextureQuality);
        m_ShadowQualityDropdown.value = m_GameSettings.ShadowQuality;
        m_AntiAliasingDropdown.value = m_GameSettings.AntiAliasing;
        m_VSyncDropdown.value = m_GameSettings.VerticalSync;

        m_MasterVolumeSlider.value = m_GameSettings.MasterVolume;
        m_MasterVolumeText.text = "Master Volume = " + (int)(m_MasterVolumeSlider.value * 100) + "%";

        m_MouseSensSlider.value = m_GameSettings.MouseSensitivity;
        m_MouseSensText.text = "Mouse Sensitivity = " + m_MouseSensSlider.value;

        m_ResolutionDropdown.RefreshShownValue();

        Screen.fullScreen = m_GameSettings.IsFullscreen;
        Screen.SetResolution(m_Resolutions[m_GameSettings.ResolutionIndex].width, m_Resolutions[m_GameSettings.ResolutionIndex].height, Screen.fullScreen);
        QualitySettings.masterTextureLimit = m_GameSettings.TextureQuality;
        QualitySettings.shadowResolution = (ShadowResolution)m_GameSettings.ShadowQuality;
        QualitySettings.antiAliasing = (int)Mathf.Pow(2f, m_GameSettings.AntiAliasing);
        QualitySettings.vSyncCount = m_GameSettings.VerticalSync;
        AudioListener.volume = m_GameSettings.MasterVolume;
    }
}
