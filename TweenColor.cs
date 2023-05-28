// Decompiled with JetBrains decompiler
// Type: TweenColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Color")]
public class TweenColor : UITweener
{
  public Color from = Color.white;
  private Light mLight;
  private Material mMat;
  private Transform mTrans;
  private UIWidget mWidget;
  public Color to = Color.white;

  private void Awake()
  {
    this.mWidget = ((Component) this).GetComponentInChildren<UIWidget>();
    Renderer renderer = ((Component) this).renderer;
    if (Object.op_Inequality((Object) renderer, (Object) null))
      this.mMat = renderer.material;
    this.mLight = ((Component) this).light;
  }

  public static TweenColor Begin(GameObject go, float duration, Color color)
  {
    TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
    tweenColor.from = tweenColor.color;
    tweenColor.to = color;
    if ((double) duration <= 0.0)
    {
      tweenColor.Sample(1f, true);
      ((Behaviour) tweenColor).enabled = false;
    }
    return tweenColor;
  }

  protected override void OnUpdate(float factor, bool isFinished) => this.color = Color.Lerp(this.from, this.to, factor);

  public Color color
  {
    get
    {
      if (Object.op_Inequality((Object) this.mWidget, (Object) null))
        return this.mWidget.color;
      if (Object.op_Inequality((Object) this.mLight, (Object) null))
        return this.mLight.color;
      return Object.op_Inequality((Object) this.mMat, (Object) null) ? this.mMat.color : Color.black;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mWidget, (Object) null))
        this.mWidget.color = value;
      if (Object.op_Inequality((Object) this.mMat, (Object) null))
        this.mMat.color = value;
      if (!Object.op_Inequality((Object) this.mLight, (Object) null))
        return;
      this.mLight.color = value;
      ((Behaviour) this.mLight).enabled = (double) value.r + (double) value.g + (double) value.b > 0.0099999997764825821;
    }
  }
}
