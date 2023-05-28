// Decompiled with JetBrains decompiler
// Type: UI.VerticalLineScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace UI
{
  internal class VerticalLineScaler : BaseScaler
  {
    public override void ApplyScale()
    {
      float currentCanvasScale = UIManager.CurrentCanvasScale;
      RectTransform component = ((Component) this).GetComponent<RectTransform>();
      float num = 1f;
      if ((double) num * (double) currentCanvasScale < 1.0)
        num = 1f / currentCanvasScale;
      component.sizeDelta = new Vector2(num, component.sizeDelta.y);
    }
  }
}
