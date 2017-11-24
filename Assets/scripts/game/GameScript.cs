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
  public Sprite gunSpriteNormal;
  public Sprite gunSpriteShoot;
  public int barrelSize = 6;
  public int currentBarrel;
  public int lives = 3;

  [Header("Effects")]
  public GameObject hitEffect;

  [Header("UI")]
  public GameUIScript ui;
  public GameObject settings;

  [HideInInspector]
  public float timeLeft;
  [HideInInspector]
  public long score;

  private float cooldown;
  private Vector3 gunTarget;
  private bool isReloading;

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
    if (timeLeft <= 0f)
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

  #endregion

  #region Gameplay

  private Vector3 GetScreenGunTarget()
  {
    Vector3 screenGunTarget = Vector3.zero;
    if (Input.touchCount > 0)
    {
      screenGunTarget = Input.touches[0].position;
    }
    else
    {
      screenGunTarget = Input.mousePosition;
    }

    return screenGunTarget;
  }

  private void MoveGun()
  {
    // No movement allowed if reloading
    if (isReloading) return;

    bool moveGun = (Input.mousePresent || Input.touchCount > 0);
    Vector3 gunScreentarget = GetScreenGunTarget();

    if (moveGun)
    {
      gunTarget = Camera.main.ScreenToWorldPoint(gunScreentarget);
      gunTarget.y = Mathf.Clamp(gunTarget.y - 2.5f, -4.75f, -2.5f);
      gunTarget.z = gun.transform.position.z;

      // This is an inexact but popular way to use Lerp
      gun.transform.position = Vector3.Lerp(gun.transform.position, gunTarget, Time.deltaTime * 6.5f);
    }
  }

  private void Shoot()
  {
    // No shooting allowed if reloading
    if (isReloading) return;

    bool shoot = false;
    bool reload = false;

    // Clic
    shoot |= Input.GetMouseButtonDown(0);
    reload |= Input.GetMouseButtonDown(1);

    // Tap
    if (Input.touchCount == 1)
    {
      shoot |= Input.touches[0].phase == TouchPhase.Began;
    }
    else if (Input.touchCount == 2)
    {
      reload = true;
    }

    if (reload)
    {
      isReloading = true;
      StartCoroutine(ReloadAnimation());
    }
    else if (shoot)
    {
      if (currentBarrel > 0)
      {
        currentBarrel--;

        // Play anim
        StartCoroutine(ShootAnimation());

        // Raycast
        LookForHit();
      }
      else
      {
        SoundBank.Play("no-ammo", Vector3.zero);
      }
    }
  }

  private IEnumerator ShootAnimation()
  {
    SpriteRenderer s = gun.GetComponent<SpriteRenderer>();
    s.sprite = gunSpriteShoot;
    SoundBank.Play("shoot", Vector3.zero);
    CameraShaker.Shake(0.1f, 0.1f, 2f);

    yield return new WaitForSeconds(0.1f);

    s.sprite = gunSpriteNormal;
  }

  private IEnumerator ReloadAnimation()
  {
    SoundBank.Play("reload", Vector3.zero);

    // Move gun up/down then down/down using linear interpolators
    Vector3 from = gun.transform.position;
    Vector3 to = gun.transform.position + new Vector3(0, -3f, 0);

    float duration = 0.25f;
    float t = 0;
    while (t < duration)
    {
      float delta = (t / duration);
      gun.transform.position = Vector3.Lerp(from, to, delta);
      yield return new WaitForEndOfFrame();
      t += Time.deltaTime;
    }
    t = 0;
    while (t < duration)
    {
      float delta = (t / duration);
      gun.transform.position = Vector3.Lerp(to, from, delta);
      yield return new WaitForEndOfFrame();
      t += Time.deltaTime;
    }

    currentBarrel = barrelSize;
    isReloading = false;
  }

  private void LookForHit()
  {
    Vector3 gunScreentarget = GetScreenGunTarget();
    Vector3 target = Camera.main.ScreenToWorldPoint(gunScreentarget);

    foreach (RaycastHit2D hit in Physics2D.CircleCastAll(target, 0.5f, new Vector2(1, 0)))
    {
      ObjectToShootScript ots = hit.transform.GetComponent<ObjectToShootScript>();
      if (ots != null)
      {
        // Particles
        Instantiate(hitEffect, target, Quaternion.identity);

        // Damage logic
        if (ots.Damage())
        {
          if (ots.isExplosive == false)
          {
            score += ots.points;
          }
          else
          {
            score -= ots.points;
          }
        }

        break; // One hit per bullet?
      }
    }

  }

  private void SpawnStuff()
  {
    // Pick 1 randomly
    GameObject prefab = objectsToShootPrefabs[Random.Range(0, objectsToShootPrefabs.Length)].gameObject;

    GameObject objectToShoot = Instantiate(prefab);
    objectToShoot.transform.position = new Vector3(Random.Range(-7f, 7f), -5.5f, 0f);

    Rigidbody2D rbody2d = objectToShoot.GetComponent<Rigidbody2D>();
    rbody2d.velocity = (new Vector2(Random.Range(-2f, 2f), Random.Range(6, 25)));

    SoundBank.Play("woosh", transform.position);
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
