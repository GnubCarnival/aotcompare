// Decompiled with JetBrains decompiler
// Type: NGUIDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
  private static NGUIDebug mInstance = (NGUIDebug) null;
  private static List<string> mLines = new List<string>();

  public static void DrawBounds(Bounds b)
  {
    Vector3 center = ((Bounds) ref b).center;
    Vector3 vector3_1 = Vector3.op_Subtraction(((Bounds) ref b).center, ((Bounds) ref b).extents);
    Vector3 vector3_2 = Vector3.op_Addition(((Bounds) ref b).center, ((Bounds) ref b).extents);
    Debug.DrawLine(new Vector3(vector3_1.x, vector3_1.y, center.z), new Vector3(vector3_2.x, vector3_1.y, center.z), Color.red);
    Debug.DrawLine(new Vector3(vector3_1.x, vector3_1.y, center.z), new Vector3(vector3_1.x, vector3_2.y, center.z), Color.red);
    Debug.DrawLine(new Vector3(vector3_2.x, vector3_1.y, center.z), new Vector3(vector3_2.x, vector3_2.y, center.z), Color.red);
    Debug.DrawLine(new Vector3(vector3_1.x, vector3_2.y, center.z), new Vector3(vector3_2.x, vector3_2.y, center.z), Color.red);
  }

  public static void Log(string text)
  {
    if (Application.isPlaying)
    {
      if (NGUIDebug.mLines.Count > 20)
        NGUIDebug.mLines.RemoveAt(0);
      NGUIDebug.mLines.Add(text);
      if (!Object.op_Equality((Object) NGUIDebug.mInstance, (Object) null))
        return;
      GameObject gameObject = new GameObject("_NGUI Debug");
      NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
      Object.DontDestroyOnLoad((Object) gameObject);
    }
    else
      Debug.Log((object) text);
  }

  private void OnGUI()
  {
    int index = 0;
    for (int count = NGUIDebug.mLines.Count; index < count; ++index)
      GUILayout.Label(NGUIDebug.mLines[index], new GUILayoutOption[0]);
  }
}
