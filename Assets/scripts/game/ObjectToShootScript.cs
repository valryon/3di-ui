using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToShootScript : MonoBehaviour
{
  public int hp = 1;
  public int points = 1;
  public bool isExplosive = false;
  public string destroySound;
  public GameObject explosionPrefab;

  private SpriteRenderer sprite;

  void Awake()
  {
    sprite = GetComponent<SpriteRenderer>();

    // Self destruction security
    Destroy(this.gameObject, 7f);
  }

  public bool Damage()
  {
    hp -= 1;
    if (hp <= 0)
    {
      StartCoroutine(DestroyAnimation());
      return true;
    }
    else
    {
      StartCoroutine(HitAnimation());
      return false;
    }
  }

  private IEnumerator DestroyAnimation()
  {
    yield return HitAnimation();

    SoundBank.Play(destroySound, transform.position);

    if(explosionPrefab != null)
    {
      Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    yield return new WaitForEndOfFrame();
    Destroy(this.gameObject);
  }

  private IEnumerator HitAnimation()
  {
    SoundBank.Play("hit", transform.position);
    yield return new WaitForEndOfFrame();
    sprite.color = Color.red;
    yield return new WaitForEndOfFrame();
    sprite.color = Color.black;
    yield return new WaitForEndOfFrame();
    sprite.color = Color.white;
    yield return new WaitForEndOfFrame();
  }

}
