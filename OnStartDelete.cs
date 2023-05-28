// Decompiled with JetBrains decompiler
// Type: OnStartDelete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class OnStartDelete : MonoBehaviour
{
  private void Start() => Object.DestroyObject((Object) ((Component) this).gameObject);
}
