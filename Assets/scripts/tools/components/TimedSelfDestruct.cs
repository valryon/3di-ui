using UnityEngine;
using System.Collections;

public class TimedSelfDestruct : MonoBehaviour
{
  public float delay = 1f;

  void Start()
  {
    Destroy(this.gameObject, delay);
  }

}