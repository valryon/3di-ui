using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
  public GameObject settings;
  private Animator canvasAnimator;

  void Start()
  {
    canvasAnimator = GetComponent<Animator>();

    settings.SetActive(false);
    SettingsScript.ApplySettings();
  }

  public void ShowSettings()
  {
    settings.SetActive(true);
  }

  public void LaunchGame()
  {
    canvasAnimator.SetTrigger("StartPlay");
    StartCoroutine(FadeToScene());
  }

  private IEnumerator FadeToScene()
  {
    yield return new WaitForSeconds(2f);
    UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
  }

  public void ExitGame()
  {
    Application.Quit();
  }
};
