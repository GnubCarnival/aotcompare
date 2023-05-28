// Decompiled with JetBrains decompiler
// Type: UI.BaseScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace UI
{
  internal abstract class BaseScaler : MonoBehaviour
  {
    protected virtual void Awake() => this.ApplyScale();

    public abstract void ApplyScale();
  }
}
