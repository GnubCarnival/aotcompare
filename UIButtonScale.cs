// Decompiled with JetBrains decompiler
// Type: UIButtonScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
  public float duration = 0.2f;
  public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
  private bool mHighlighted;
  private Vector3 mScale;
  private bool mStarted;
  public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);
  public Transform tweenTarget;

  private void OnDisable()
  {
    if (!this.mStarted || !Object.op_Inequality((Object) this.tweenTarget, (Object) null))
      return;
    TweenScale component = ((Component) this.tweenTarget).GetComponent<TweenScale>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.scale = this.mScale;
    ((Behaviour) component).enabled = false;
  }

  private void OnEnable()
  {
    if (!this.mStarted || !this.mHighlighted)
      return;
    this.OnHover(UICamera.IsHighlighted(((Component) this).gameObject));
  }

  private void OnHover(bool isOver)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenScale.Begin(((Component) this.tweenTarget).gameObject, this.duration, !isOver ? this.mScale : Vector3.Scale(this.mScale, this.hover)).method = UITweener.Method.EaseInOut;
    this.mHighlighted = isOver;
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenScale.Begin(((Component) this.tweenTarget).gameObject, this.duration, !isPressed ? (!UICamera.IsHighlighted(((Component) this).gameObject) ? this.mScale : Vector3.Scale(this.mScale, this.hover)) : Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOut;
  }

  private void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if (Object.op_Equality((Object) this.tweenTarget, (Object) null))
      this.tweenTarget = ((Component) this).transform;
    this.mScale = this.tweenTarget.localScale;
  }
}
