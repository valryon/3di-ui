using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
  public GameObject settings;

  void Start()
  {
    settings.SetActive(false);
  }

  public void ShowSettings()
  {
    settings.SetActive(true);
  }

  public void LaunchGame()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
  }

  public void ExitGame()
  {
    Application.Quit();
  }
};