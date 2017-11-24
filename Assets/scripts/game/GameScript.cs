using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
  #region Members

  [Header("Gameplay")]
  public GameObject[] objectsToShoot;

  [Header("UI")]
  public GameObject settings;

  #endregion

  #region Timeline

  void Start()
  {

  }

  void Update()
  {
    // Back from pause
    if (settings.activeInHierarchy == false && Time.timeScale == 0)
    {
      Time.timeScale = 1f;
    }
  }

  #endregion

  #region Public methods

  public void ShowSettings()
  {
    Time.timeScale = 0;
    settings.SetActive(true);
  }

  public void BackToMenu()
  {

  }

  #endregion
}
