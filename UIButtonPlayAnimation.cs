// Decompiled with JetBrains decompiler
// Type: UIButtonPlayAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Play Animation")]
public class UIButtonPlayAnimation : MonoBehaviour
{
  public string callWhenFinished;
  public bool clearSelection;
  public string clipName;
  public DisableCondition disableWhenFinished;
  public GameObject eventReceiver;
  public EnableCondition ifDisabledOnPlay;
  private bool mHighlighted;
  private bool mStarted;
  public ActiveAnimation.OnFinished onFinished;
  public AnimationOrTween.Direction playDirection = AnimationOrTween.Direction.Forward;
  public bool resetOnPlay;
  public Animation target;
  public AnimationOrTween.Trigger trigger;

  private void OnActivate(bool isActive)
  {
    if (!((Behaviour) this).enabled || this.trigger != AnimationOrTween.Trigger.OnActivate && !(this.trigger == AnimationOrTween.Trigger.OnActivateTrue & isActive) && (this.trigger != AnimationOrTween.Trigger.OnActivateFalse || isActive))
      return;
    this.Play(isActive);
  }

  private void OnClick()
  {
    if (!((Behaviour) this).enabled || this.trigger != AnimationOrTween.Trigger.OnClick)
      return;
    this.Play(true);
  }

  private void OnDoubleClick()
  {
    if (!((Behaviour) this).enabled || this.trigger != AnimationOrTween.Trigger.OnDoubleClick)
      return;
    this.Play(true);
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
    if (this.trigger == AnimationOrTween.Trigger.OnHover || this.trigger == AnimationOrTween.Trigger.OnHoverTrue & isOver || this.trigger == AnimationOrTween.Trigger.OnHoverFalse && !isOver)
      this.Play(isOver);
    this.mHighlighted = isOver;
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled || this.trigger != AnimationOrTween.Trigger.OnPress && !(this.trigger == AnimationOrTween.Trigger.OnPressTrue & isPressed) && (this.trigger != AnimationOrTween.Trigger.OnPressFalse || isPressed))
      return;
    this.Play(isPressed);
  }

  private void OnSelect(bool isSelected)
  {
    if (!((Behaviour) this).enabled || this.trigger != AnimationOrTween.Trigger.OnSelect && !(this.trigger == AnimationOrTween.Trigger.OnSelectTrue & isSelected) && (this.trigger != AnimationOrTween.Trigger.OnSelectFalse || isSelected))
      return;
    this.Play(true);
  }

  private void Play(bool forward)
  {
    if (Object.op_Equality((Object) this.target, (Object) null))
      this.target = ((Component) this).GetComponentInChildren<Animation>();
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    if (this.clearSelection && Object.op_Equality((Object) UICamera.selectedObject, (Object) ((Component) this).gameObject))
      UICamera.selectedObject = (GameObject) null;
    int num = -(int) this.playDirection;
    ActiveAnimation activeAnimation = ActiveAnimation.Play(this.target, this.clipName, !forward ? (AnimationOrTween.Direction) num : this.playDirection, this.ifDisabledOnPlay, this.disableWhenFinished);
    if (!Object.op_Inequality((Object) activeAnimation, (Object) null))
      return;
    if (this.resetOnPlay)
      activeAnimation.Reset();
    activeAnimation.onFinished = this.onFinished;
    if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.callWhenFinished))
    {
      activeAnimation.eventReceiver = this.eventReceiver;
      activeAnimation.callWhenFinished = this.callWhenFinished;
    }
    else
      activeAnimation.eventReceiver = (GameObject) null;
  }

  private void Start() => this.mStarted = true;
}
