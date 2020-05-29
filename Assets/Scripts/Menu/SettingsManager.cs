using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public struct GameSettings
{
    public bool IsFullscreen { get; set; }
    public int ResolutionIndex { get; set; }
    public int TextureQuality { get; set; }
    public int ShadowQuality { get; set; }
    public int AntiAliasing { get; set; }
    public int VerticalSync { get; set; }
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float EffectsVolume { get; set; }
    public float MouseSensitivity { get; set; }
}

public class SettingsManager : InitializeSingleton<SettingsManager>
{
    [SerializeField] private AudioMixer m_MainMixer = null;
    [SerializeField] private SettingsMenu m_SettingsMenu = null;

    private GameSettings m_GameSettings;
    private Resolution[] m_Resolutions;

    private string m_Fullscreen;
    private string m_ResolutionIndex;
    private string m_TextureQuality;
    private string m_ShadowQuality;
    private string m_AntiAliasing;
    private string m_VerticalSync;
    private string m_MasterVolume;
    private string m_MusicVolume;
    private string m_EffectsVolume;
    private string m_MouseSensitivity;

    public GameSettings GameSettings => m_GameSettings;
    public Resolution[] Resolutions => m_Resolutions;

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
        m_MusicVolume = "MusicVolume";
        m_EffectsVolume = "EffectsVolume";
        m_MouseSensitivity = "MouseSensitivity";

        m_Resolutions = Screen.resolutions;

        QualitySettings.SetQualityLevel(5);

        LoadSettings();
        m_SettingsMenu.UpdateSettingsMenu();
    }

    public void LoadSettings()
    {
        m_GameSettings.IsFullscreen = Convert.ToBoolean(PlayerPrefs.GetInt(m_Fullscreen, 0));
        m_GameSettings.TextureQuality = PlayerPrefs.GetInt(m_TextureQuality, QualitySettings.masterTextureLimit);
        m_GameSettings.ShadowQuality = PlayerPrefs.GetInt(m_ShadowQuality, (int)QualitySettings.shadowResolution);
        m_GameSettings.AntiAliasing = PlayerPrefs.GetInt(m_AntiAliasing, (int)(Mathf.Log(QualitySettings.antiAliasing) / Mathf.Log(2.0f)));
        m_GameSettings.VerticalSync = PlayerPrefs.GetInt(m_VerticalSync, QualitySettings.vSyncCount);
        m_GameSettings.ResolutionIndex = PlayerPrefs.GetInt(m_ResolutionIndex, m_Resolutions.Length - 1);
        m_GameSettings.MasterVolume = PlayerPrefs.GetFloat(m_MasterVolume, 0.75f);
        m_GameSettings.MusicVolume = PlayerPrefs.GetFloat(m_MusicVolume, 1.0f);
        m_GameSettings.EffectsVolume = PlayerPrefs.GetFloat(m_EffectsVolume, 1.0f);
        m_GameSettings.MouseSensitivity = PlayerPrefs.GetFloat(m_MouseSensitivity, 1.0f);

        Screen.fullScreen = m_GameSettings.IsFullscreen;
        Screen.SetResolution(m_Resolutions[m_GameSettings.ResolutionIndex].width, m_Resolutions[m_GameSettings.ResolutionIndex].height, m_GameSettings.IsFullscreen);
        QualitySettings.masterTextureLimit = m_GameSettings.TextureQuality;
        QualitySettings.shadowResolution = (ShadowResolution)m_GameSettings.ShadowQuality;
        QualitySettings.antiAliasing = (int)Mathf.Pow(2.0f, m_GameSettings.AntiAliasing);
        QualitySettings.vSyncCount = m_GameSettings.VerticalSync;
        m_MainMixer.SetFloat("MasterVolume", Mathf.Log(m_GameSettings.MasterVolume) * 20);
        m_MainMixer.SetFloat("MusicVolume", Mathf.Log(m_GameSettings.MusicVolume) * 20);
        m_MainMixer.SetFloat("EffectsVolume", Mathf.Log(m_GameSettings.EffectsVolume) * 20);
    }

    public void SaveSettings(GameSettings gameSettings)
    {
        m_GameSettings = gameSettings;

        PlayerPrefs.SetInt(m_Fullscreen, m_GameSettings.IsFullscreen ? 1 : 0);
        PlayerPrefs.SetInt(m_TextureQuality, m_GameSettings.TextureQuality);
        PlayerPrefs.SetInt(m_ShadowQuality, m_GameSettings.ShadowQuality);
        PlayerPrefs.SetInt(m_AntiAliasing, m_GameSettings.AntiAliasing);
        PlayerPrefs.SetInt(m_VerticalSync, m_GameSettings.VerticalSync);
        PlayerPrefs.SetInt(m_ResolutionIndex, m_GameSettings.ResolutionIndex);
        PlayerPrefs.SetFloat(m_MasterVolume, m_GameSettings.MasterVolume);
        PlayerPrefs.SetFloat(m_MusicVolume, m_GameSettings.MusicVolume);
        PlayerPrefs.SetFloat(m_EffectsVolume, m_GameSettings.EffectsVolume);
        PlayerPrefs.SetFloat(m_MouseSensitivity, m_GameSettings.MouseSensitivity);
    }
}
