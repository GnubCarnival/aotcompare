// Decompiled with JetBrains decompiler
// Type: UIButtonRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
  public float duration = 0.2f;
  public Vector3 hover = Vector3.zero;
  private bool mHighlighted;
  private Quaternion mRot;
  private bool mStarted;
  public Vector3 pressed = Vector3.zero;
  public Transform tweenTarget;

  private void OnDisable()
  {
    if (!this.mStarted || !Object.op_Inequality((Object) this.tweenTarget, (Object) null))
      return;
    TweenRotation component = ((Component) this.tweenTarget).GetComponent<TweenRotation>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.rotation = this.mRot;
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
    TweenRotation.Begin(((Component) this.tweenTarget).gameObject, this.duration, !isOver ? this.mRot : Quaternion.op_Multiply(this.mRot, Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
    this.mHighlighted = isOver;
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenRotation.Begin(((Component) this.tweenTarget).gameObject, this.duration, !isPressed ? (!UICamera.IsHighlighted(((Component) this).gameObject) ? this.mRot : Quaternion.op_Multiply(this.mRot, Quaternion.Euler(this.hover))) : Quaternion.op_Multiply(this.mRot, Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
  }

  private void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if (Object.op_Equality((Object) this.tweenTarget, (Object) null))
      this.tweenTarget = ((Component) this).transform;
    this.mRot = this.tweenTarget.localRotation;
  }
}
