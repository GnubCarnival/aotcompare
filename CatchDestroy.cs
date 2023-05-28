// Decompiled with JetBrains decompiler
// Type: CatchDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CatchDestroy : MonoBehaviour
{
  public GameObject target;

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    Object.Destroy((Object) this.target);
  }
}
