// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct SoundBankEntry
{
  public string eventName;
  public AudioClip clip;
}

public class SoundBank : MonoBehaviour
{
  private static SoundBank instance;

  public List<SoundBankEntry> sounds;

  private Dictionary<string, List<SoundBankEntry>> clips = new Dictionary<string, List<SoundBankEntry>>(System.StringComparer.InvariantCultureIgnoreCase);

  #region Timeline

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    // Fill the dictionary
    foreach (var s in sounds)
    {
      if (s.clip != null && string.IsNullOrEmpty(s.eventName) == false)
      {
        if (clips.ContainsKey(s.eventName) == false)
        {
          clips.Add(s.eventName, new List<SoundBankEntry>());
        }

        clips[s.eventName].Add(s);
      }
    }
  }

  #endregion

  #region Methods

  /// <summary>
  /// Play a sound with spatialization
  /// </summary>
  /// <param name="name"></param>
  /// <param name="position"></param>
  public static void Play(string name, Vector3 position)
  {
#if UNITY_EDITOR
    if (instance == null) instance = FindObjectOfType<SoundBank>();
#endif

    var sound = Get(name);
    if (sound != null)
    {
      AudioSource.PlayClipAtPoint(sound.Value.clip,
        new Vector3(position.x, position.y, Camera.main.transform.position.z), PlayerPrefs.GetFloat("sound"));
    }
    else
    {
      Debug.LogWarning("Missing AudioClip for [" + name + "]");
    }
  }

  public static SoundBankEntry? Get(string name)
  {
    if (instance == null)
    {
      Debug.LogError("Missing SoundBank in scene!");
      return null;
    }

    if (instance.clips.ContainsKey(name))
    {
      var s = instance.clips[name];
      SoundBankEntry? entry = null;
      if (s.Count > 1)
      {
        entry = s[Random.Range(0, s.Count)];
      }
      else
      {
        entry = s[0];
      }

      return entry;
    }

    Debug.LogWarning("Missing sound [" + name + "]");
    return null;
  }

  #endregion
}
