// Decompiled with JetBrains decompiler
// Type: UIButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
  public Color disabledColor = Color.grey;

  protected override void OnEnable()
  {
    if (this.isEnabled)
      base.OnEnable();
    else
      this.UpdateColor(false, true);
  }

  public override void OnHover(bool isOver)
  {
    if (!this.isEnabled)
      return;
    base.OnHover(isOver);
  }

  public override void OnPress(bool isPressed)
  {
    if (!this.isEnabled)
      return;
    base.OnPress(isPressed);
  }

  public void UpdateColor(bool shouldBeEnabled, bool immediate)
  {
    if (!Object.op_Inequality((Object) this.tweenTarget, (Object) null))
      return;
    if (!this.mStarted)
    {
      this.mStarted = true;
      this.Init();
    }
    Color color = !shouldBeEnabled ? this.disabledColor : this.defaultColor;
    TweenColor tweenColor = TweenColor.Begin(this.tweenTarget, 0.15f, color);
    if (!immediate)
      return;
    tweenColor.color = color;
    ((Behaviour) tweenColor).enabled = false;
  }

  public bool isEnabled
  {
    get
    {
      Collider collider = ((Component) this).collider;
      return Object.op_Inequality((Object) collider, (Object) null) && collider.enabled;
    }
    set
    {
      Collider collider = ((Component) this).collider;
      if (!Object.op_Inequality((Object) collider, (Object) null) || collider.enabled == value)
        return;
      collider.enabled = value;
      this.UpdateColor(value, false);
    }
  }
}
