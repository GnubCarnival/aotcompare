// Decompiled with JetBrains decompiler
// Type: SupportLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class SupportLogger : MonoBehaviour
{
  public bool LogTrafficStats = true;

  public void Start()
  {
    if (!Object.op_Equality((Object) GameObject.Find("PunSupportLogger"), (Object) null))
      return;
    GameObject gameObject = new GameObject("PunSupportLogger");
    Object.DontDestroyOnLoad((Object) gameObject);
    gameObject.AddComponent<SupportLogging>().LogTrafficStats = this.LogTrafficStats;
  }
}
