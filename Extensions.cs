// Decompiled with JetBrains decompiler
// Type: Extensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine;

public static class Extensions
{
  public static bool AlmostEquals(this float target, float second, float floatDiff) => (double) Mathf.Abs(target - second) < (double) floatDiff;

  public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle) => (double) Quaternion.Angle(target, second) < (double) maxAngle;

  public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
  {
    Vector2 vector2 = Vector2.op_Subtraction(target, second);
    return (double) ((Vector2) ref vector2).sqrMagnitude < (double) sqrMagnitudePrecision;
  }

  public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
  {
    Vector3 vector3 = Vector3.op_Subtraction(target, second);
    return (double) ((Vector3) ref vector3).sqrMagnitude < (double) sqrMagnitudePrecision;
  }

  public static bool Contains(this int[] target, int nr)
  {
    if (target != null)
    {
      for (int index = 0; index < target.Length; ++index)
      {
        if (target[index] == nr)
          return true;
      }
    }
    return false;
  }

  public static PhotonView GetPhotonView(this GameObject go) => go.GetComponent<PhotonView>();

  public static PhotonView[] GetPhotonViewsInChildren(this GameObject go) => go.GetComponentsInChildren<PhotonView>(true);

  public static void Merge(this IDictionary target, IDictionary addHash)
  {
    if (addHash == null || target.Equals((object) addHash))
      return;
    foreach (object key in (IEnumerable) addHash.Keys)
      target[key] = addHash[key];
  }

  public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
  {
    if (addHash == null || target.Equals((object) addHash))
      return;
    foreach (object key in (IEnumerable) addHash.Keys)
    {
      if (key is string)
        target[key] = addHash[key];
    }
  }

  public static void StripKeysWithNullValues(this IDictionary original)
  {
    object[] objArray = new object[original.Count];
    int num = 0;
    foreach (object key in (IEnumerable) original.Keys)
      objArray[num++] = key;
    for (int index = 0; index < objArray.Length; ++index)
    {
      object key = objArray[index];
      if (original[key] == null)
        original.Remove(key);
    }
  }

  public static Hashtable StripToStringKeys(this IDictionary original)
  {
    Hashtable stringKeys = new Hashtable();
    foreach (DictionaryEntry dictionaryEntry in original)
    {
      if (dictionaryEntry.Key is string)
        stringKeys[dictionaryEntry.Key] = dictionaryEntry.Value;
    }
    return stringKeys;
  }

  public static string ToStringFull(this IDictionary origin) => SupportClass.DictionaryToString(origin, false);
}
