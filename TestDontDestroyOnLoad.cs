// Decompiled with JetBrains decompiler
// Type: TestDontDestroyOnLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class TestDontDestroyOnLoad : MonoBehaviour
{
  private void Awake() => Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);
}
