using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
  #region Members

  [Header("Gameplay: shoot")]
  public ObjectToShootScript[] objectsToShootPrefabs;
  public float minCooldown = 0.25f;
  public float maxCooldown = 1f;
  public int time = 60;

  [Header("Gameplay: gun")]
  public GameObject gun;
  public int barrelSize = 6;
  public int currentBarrel;
  public int lives = 3;

  [Header("UI")]
  public GameUIScript ui;
  public GameObject settings;

  [HideInInspector]
  public float timeLeft;
  [HideInInspector]
  public long score;

  private float cooldown;
  private Vector3 gunTarget;

  #endregion

  #region Timeline

  void Start()
  {
    currentBarrel = barrelSize;
    timeLeft = time;
    score = 0;
  }

  void Update()
  {
    // Paused? Ignore
    if (settings.activeInHierarchy) return;

    // Back from pause
    if (settings.activeInHierarchy == false && Time.timeScale == 0)
    {
      Time.timeScale = 1f;
    }

    // Time
    timeLeft -= Time.deltaTime;
    if(timeLeft <= 0f)
    {
      GameOver();
    }

    // Cooldown for stuff spawn
    if (cooldown > 0)
    {
      cooldown -= Time.deltaTime;
    }
    else
    {
      cooldown = Random.Range(minCooldown, maxCooldown);
      SpawnStuff();
    }

    // Move gun to mouse/finger
    MoveGun();

    Shoot();
  }

  private void UpdateUI()
  {
  }

  #endregion

  #region Gameplay

  private void MoveGun()
  {
    bool moveGun = false;
    Vector3 screenGunTarget = Vector3.zero;
    if (Input.touchCount > 0)
    {
      moveGun = true;
      screenGunTarget = Input.touches[0].position;
    }
    else if (Input.mousePresent)
    {
      moveGun = true;
      screenGunTarget = Input.mousePosition;
    }

    if (moveGun)
    {
      gunTarget = Camera.main.ScreenToWorldPoint(screenGunTarget);
      gunTarget.y = Mathf.Clamp(gunTarget.y, -4.75f, -2.5f);
      gunTarget.z = gun.transform.position.z;

      // This is an inexact but popular way to use Lerp
      gun.transform.position = Vector3.Lerp(gun.transform.position, gunTarget, Time.deltaTime * 6.5f);
    }
  }

  private void Shoot()
  {

  }

  private IEnumerator ShootAnimation()
  {
    yield return null;
  }

  private void SpawnStuff()
  {
    // Pick 1 randomly
    GameObject prefab = objectsToShootPrefabs[Random.Range(0, objectsToShootPrefabs.Length)].gameObject;

    GameObject objectToShoot = Instantiate(prefab);
    objectToShoot.transform.position = new Vector3(Random.Range(-7f, 7f), -5.5f, 0f);

    Rigidbody2D rbody2d = objectToShoot.GetComponent<Rigidbody2D>();
    rbody2d.velocity = (new Vector2(Random.Range(-2f, 2f), Random.Range(6, 25)));
  }

  private void GameOver()
  {

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
