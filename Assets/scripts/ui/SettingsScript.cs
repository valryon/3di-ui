using System.Collections;
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

  public void HideSettings()
  {
    gameObject.SetActive(false);
  }
}
