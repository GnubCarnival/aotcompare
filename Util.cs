// Decompiled with JetBrains decompiler
// Type: Util
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using UnityEngine;

public static class Util
{
  public static bool GetRandomBool() => (double) Random.Range(0.0f, 1f) > 0.5;

  public static float GetRandomSign() => !Util.GetRandomBool() ? -1f : 1f;

  public static Vector3 GetRandomDirection(bool flat = false)
  {
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    if (flat)
      vector3.y = 0.0f;
    return ((Vector3) ref vector3).normalized;
  }

  public static void DebugTimeSince(float start, string prefix = "") => Debug.Log((object) (prefix + ": " + (Time.realtimeSinceStartup - start).ToString()));

  public static IEnumerator WaitForFrames(int frames)
  {
    for (int i = 0; i < frames; ++i)
      yield return (object) new WaitForEndOfFrame();
  }
}
