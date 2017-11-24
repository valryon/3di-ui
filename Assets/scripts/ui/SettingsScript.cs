using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
  public Slider soundVolume;
  public Slider musicVolume;
  public Toggle shakeToggle;
  public Dropdown resolutionDropdown;
  public Toggle fullscreenToggle;

  void OnEnable()
  {
    InitDefaultSettings();

    soundVolume.value = PlayerPrefs.GetFloat("sound");
    musicVolume.value = PlayerPrefs.GetFloat("music");
    shakeToggle.isOn = PlayerPrefs.GetInt("shakes") > 0;
    fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") > 0;

    resolutionDropdown.ClearOptions();
    resolutionDropdown.AddOptions(Screen.resolutions.Select(r => new Dropdown.OptionData(r.width + "x" + r.height)).ToList());
    resolutionDropdown.value = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
  }

  private static void InitDefaultSettings()
  {
    if (PlayerPrefs.HasKey("sound") == false)
    {
      PlayerPrefs.SetFloat("sound", 1f);
    }
    if (PlayerPrefs.HasKey("music") == false)
    {
      PlayerPrefs.SetFloat("music", 1f);
    }
    if (PlayerPrefs.HasKey("fullscreen") == false)
    {
      PlayerPrefs.SetInt("fullscreen", 1);
    }
    if (PlayerPrefs.HasKey("shakes") == false)
    {
      PlayerPrefs.SetInt("shakes", 1);
    }
    if (PlayerPrefs.HasKey("screenwidth") == false)
    {
      PlayerPrefs.SetInt("screenwidth", Screen.currentResolution.width);
    }
    if (PlayerPrefs.HasKey("screenheight") == false)
    {
      PlayerPrefs.SetInt("screenheight", Screen.currentResolution.height);
    }
  }

  public static void ApplySettings()
  {
    InitDefaultSettings();

    foreach (var a in FindObjectsOfType<AudioSource>())
    {
      a.volume = PlayerPrefs.GetFloat("music");
    }

    int width = PlayerPrefs.GetInt("screenwidth");
    int height = PlayerPrefs.GetInt("screenheight");
    bool fullscreen = PlayerPrefs.GetInt("fullscreen") > 0 ? true : false;
    Screen.SetResolution(width, height, fullscreen);
  }

  public void SetMusicVolume()
  {
    PlayerPrefs.SetFloat("music", musicVolume.value);
    ApplySettings();
  }

  public void SetSoundVolume()
  {
    PlayerPrefs.SetFloat("sound", soundVolume.value);
    ApplySettings();
  }

  public void SetFullscreen()
  {
    PlayerPrefs.SetInt("fullscreen", fullscreenToggle.isOn ? 1 : 0);
    ApplySettings();
  }

  public void SetShakes()
  {
    PlayerPrefs.SetInt("shakes", shakeToggle.isOn ? 1 : 0);
    ApplySettings();
  }

  public void SetResolution()
  {
    if (resolutionDropdown.value >= 0)
    {
      Resolution r = Screen.resolutions[resolutionDropdown.value];
      PlayerPrefs.SetInt("screenwidth", r.width);
      PlayerPrefs.SetInt("screenheight", r.height);
      ApplySettings();
    }
  }

  public void HideSettings()
  {
    gameObject.SetActive(false);
  }
}
