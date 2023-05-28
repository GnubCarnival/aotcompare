// Decompiled with JetBrains decompiler
// Type: UIButtonOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
  public float duration = 0.2f;
  public Vector3 hover = Vector3.zero;
  private bool mHighlighted;
  private Vector3 mPos;
  private bool mStarted;
  public Vector3 pressed = new Vector3(2f, -2f);
  public Transform tweenTarget;

  private void OnDisable()
  {
    if (!this.mStarted || !Object.op_Inequality((Object) this.tweenTarget, (Object) null))
      return;
    TweenPosition component = ((Component) this.tweenTarget).GetComponent<TweenPosition>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.position = this.mPos;
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
    TweenPosition.Begin(((Component) this.tweenTarget).gameObject, this.duration, !isOver ? this.mPos : Vector3.op_Addition(this.mPos, this.hover)).method = UITweener.Method.EaseInOut;
    this.mHighlighted = isOver;
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenPosition.Begin(((Component) this.tweenTarget).gameObject, this.duration, !isPressed ? (!UICamera.IsHighlighted(((Component) this).gameObject) ? this.mPos : Vector3.op_Addition(this.mPos, this.hover)) : Vector3.op_Addition(this.mPos, this.pressed)).method = UITweener.Method.EaseInOut;
  }

  private void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if (Object.op_Equality((Object) this.tweenTarget, (Object) null))
      this.tweenTarget = ((Component) this).transform;
    this.mPos = this.tweenTarget.localPosition;
  }
}
