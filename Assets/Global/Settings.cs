﻿using UnityEngine;
using MK.Glow.Legacy;

public class Settings : MonoBehaviour
{
    // Settings variables
    public static float soundVolume = 1f;
    public AudioSource music;
    public MKGlowLite glowing;
    public Interface ui;
    public int glowMode = 2;

    // Cursor vars
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    public void SaveSettings()
    {
        // Save user settings
        SaveSystem.SaveSettings(Screen.width, Screen.height, music.volume, soundVolume, Screen.fullScreen, glowMode);
    }

    public void LoadSettings()
    {
        // Load settings from file
        SettingsData settings = SaveSystem.LoadSettings();
        if (settings == null)
        {
            SetMusic(1f);
            SetSound(1f);
            SetScreenmode(true);
            SetShaderMode(3);
            SetResolution(8);
            return;
        }

        try { soundVolume = settings.sound; }
        catch { soundVolume = 1f; }

        SetMusic(settings.volume);
        SetScreenmode(settings.fullscreen);
        SetShaderMode(settings.glowMode);

        Screen.SetResolution(settings.width, settings.height, Screen.fullScreen);
    }

    public void EnableControls()
    {
        ui.SettingsOpen = false;
        ui.ControlsOpen = true;
        ui.SetOverlayStatus("Settings", false);
        ui.SetOverlayStatus("Controls", true);
    }

    public void DisableControls()
    {
        ui.SettingsOpen = true;
        ui.ControlsOpen = false;
        ui.SetOverlayStatus("Controls", false);
        ui.SetOverlayStatus("Settings", true);
    }

    public void SetMusic(float a)
    {
        music.volume = a/2;
    }

    public void SetSound(float a)
    {
        soundVolume = a;
    }

    public float GetSound()
    {
        return soundVolume;
    }

    public void SetResolution(int a)
    {
        if (a == 1) Screen.SetResolution(1280, 720, Screen.fullScreen);
        else if (a == 2) Screen.SetResolution(1280, 800, Screen.fullScreen);
        else if (a == 3) Screen.SetResolution(1366, 768, Screen.fullScreen);
        else if (a == 4) Screen.SetResolution(1440, 900, Screen.fullScreen);
        else if (a == 5) Screen.SetResolution(1600, 900, Screen.fullScreen);
        else if (a == 6) Screen.SetResolution(1680, 1050, Screen.fullScreen);
        else if (a == 7) Screen.SetResolution(1920, 1080, Screen.fullScreen);
        else if (a == 8) Screen.SetResolution(2560, 1440, Screen.fullScreen);
    }

    public void SetScreenmode(bool a)
    {
        Screen.fullScreen = a;
    }

    public void SetShaderMode(int a)
    {
        if (a == 1) glowing.enabled = false;
        else glowing.enabled = true;
        glowMode = a;
    }

    public void EnableMenuAndPaused()
    {
        ui.SettingsOpen = true;
        ui.SetOverlayStatus("Settings", true);
        ui.SetOverlayStatus("Paused", false);
    }

    public void DisableMenuAndPaused()
    {
        ui.SettingsOpen = false;
        ui.SetOverlayStatus("Settings", false);
        ui.SetOverlayStatus("Paused", true);
    }

    public void EnableMenu()
    {
        CanvasGroup cg = transform.GetComponent<CanvasGroup>();
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.alpha = 1f;
    }

    public void DisableMenu()
    {
        CanvasGroup cg = transform.GetComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = 0f;
    }
}
