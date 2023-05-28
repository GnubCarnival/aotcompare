// Decompiled with JetBrains decompiler
// Type: TweenAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenAlpha : UITweener
{
  public float from = 1f;
  private UIPanel mPanel;
  private Transform mTrans;
  private UIWidget mWidget;
  public float to = 1f;

  private void Awake()
  {
    this.mPanel = ((Component) this).GetComponent<UIPanel>();
    if (!Object.op_Equality((Object) this.mPanel, (Object) null))
      return;
    this.mWidget = ((Component) this).GetComponentInChildren<UIWidget>();
  }

  public static TweenAlpha Begin(GameObject go, float duration, float alpha)
  {
    TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
    tweenAlpha.from = tweenAlpha.alpha;
    tweenAlpha.to = alpha;
    if ((double) duration <= 0.0)
    {
      tweenAlpha.Sample(1f, true);
      ((Behaviour) tweenAlpha).enabled = false;
    }
    return tweenAlpha;
  }

  protected override void OnUpdate(float factor, bool isFinished) => this.alpha = Mathf.Lerp(this.from, this.to, factor);

  public float alpha
  {
    get
    {
      if (Object.op_Inequality((Object) this.mWidget, (Object) null))
        return this.mWidget.alpha;
      return Object.op_Inequality((Object) this.mPanel, (Object) null) ? this.mPanel.alpha : 0.0f;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mWidget, (Object) null))
      {
        this.mWidget.alpha = value;
      }
      else
      {
        if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
          return;
        this.mPanel.alpha = value;
      }
    }
  }
}
