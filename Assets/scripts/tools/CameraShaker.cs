using UnityEngine;
/// <summary>
/// Shake shake shake the screen!
/// </summary>
public class CameraShaker : MonoBehaviour
{
  #region Members

  private static CameraShaker instance;
  private Transform camTransform;
  private float shakeDuration = 0f;
  private float shakeAmount = 0.7f;
  private float decreaseFactor = 1.0f;
  private Vector3 originalPos;

  #endregion

  #region Timeline

  void Awake()
  {
    if (instance != null) Destroy(instance);
    instance = this;
    camTransform = GetComponent(typeof(Transform)) as Transform;
  }
  void OnEnable()
  {
    originalPos = camTransform.localPosition;
  }
  void Update()
  {
    if (shakeDuration > 0)
    {
      // Little move of the camera position
      camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
      // Decrease move intensity with time
      shakeDuration -= Time.deltaTime * decreaseFactor;
    }
    else
    {
      shakeDuration = 0f;
      camTransform.localPosition = originalPos;
    }
  }
  #endregion

  #region Public methods

  public static void Shake(float duration, float force, float decreaseFactor = 1f)
  {
    if (PlayerPrefs.HasKey("shakes") && PlayerPrefs.GetInt("shakes") <= 0) return;

    if (instance == null)
    {
      instance = Camera.main.gameObject.AddComponent<CameraShaker>();
    }
    instance.shakeDuration = duration;
    instance.shakeAmount = force;
    instance.decreaseFactor = decreaseFactor;
  }

  #endregion
}