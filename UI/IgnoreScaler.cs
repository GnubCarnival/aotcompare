// Decompiled with JetBrains decompiler
// Type: UI.IgnoreScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

namespace UI
{
  internal class IgnoreScaler : BaseScaler
  {
    public float Scale = 1f;

    public override void ApplyScale()
    {
      float num = SettingsManager.UISettings.UIMasterScale.Value;
      RectTransform component = ((Component) this).GetComponent<RectTransform>();
      this.Scale = 1f / num;
      ((Transform) component).localScale = Vector2.op_Implicit(new Vector2(this.Scale, this.Scale));
    }
  }
}
