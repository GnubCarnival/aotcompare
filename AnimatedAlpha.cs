// Decompiled with JetBrains decompiler
// Type: AnimatedAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class AnimatedAlpha : MonoBehaviour
{
  public float alpha = 1f;
  private UIPanel mPanel;
  private UIWidget mWidget;

  private void Awake()
  {
    this.mWidget = ((Component) this).GetComponent<UIWidget>();
    this.mPanel = ((Component) this).GetComponent<UIPanel>();
    this.Update();
  }

  private void Update()
  {
    if (Object.op_Inequality((Object) this.mWidget, (Object) null))
      this.mWidget.alpha = this.alpha;
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
      return;
    this.mPanel.alpha = this.alpha;
  }
}
