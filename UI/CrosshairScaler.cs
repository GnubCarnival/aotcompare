// Decompiled with JetBrains decompiler
// Type: UI.CrosshairScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class CrosshairScaler : IgnoreScaler
  {
    public override void ApplyScale()
    {
      base.ApplyScale();
      float num1 = SettingsManager.UISettings.CrosshairScale.Value;
      RectTransform component = ((Component) this).GetComponent<RectTransform>();
      Vector3 localScale = ((Transform) component).localScale;
      ((Transform) component).localScale = Vector2.op_Implicit(new Vector2(localScale.x * num1, localScale.y * num1));
      int num2 = 16;
      if ((double) num1 > 1.0)
        num2 = (int) (16.0 * (double) num1);
      float num3 = 16f / (float) num2;
      ((Component) ((Component) this).transform.Find("DefaultLabel")).GetComponent<Text>().fontSize = num2;
      ((Transform) ((Component) ((Component) this).transform.Find("DefaultLabel")).GetComponent<RectTransform>()).localScale = Vector2.op_Implicit(new Vector2(num3, num3));
    }
  }
}
