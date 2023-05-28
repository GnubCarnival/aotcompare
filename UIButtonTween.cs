// Decompiled with JetBrains decompiler
// Type: UIButtonTween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Tween")]
public class UIButtonTween : MonoBehaviour
{
  public string callWhenFinished;
  public DisableCondition disableWhenFinished;
  public GameObject eventReceiver;
  public EnableCondition ifDisabledOnPlay;
  public bool includeChildren;
  private bool mHighlighted;
  private bool mStarted;
  private UITweener[] mTweens;
  public UITweener.OnFinished onFinished;
  public AnimationOrTween.Direction playDirection = AnimationOrTween.Direction.Forward;
  public bool resetOnPlay;
  public AnimationOrTween.Trigger trigger;
  public int tweenGroup;
  public GameObject tweenTarget;

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

  public void Play(bool forward)
  {
    GameObject go = Object.op_Inequality((Object) this.tweenTarget, (Object) null) ? this.tweenTarget : ((Component) this).gameObject;
    if (!NGUITools.GetActive(go))
    {
      if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
        return;
      NGUITools.SetActive(go, true);
    }
    this.mTweens = !this.includeChildren ? go.GetComponents<UITweener>() : go.GetComponentsInChildren<UITweener>();
    if (this.mTweens.Length == 0)
    {
      if (this.disableWhenFinished == DisableCondition.DoNotDisable)
        return;
      NGUITools.SetActive(this.tweenTarget, false);
    }
    else
    {
      bool flag = false;
      if (this.playDirection == AnimationOrTween.Direction.Reverse)
        forward = !forward;
      int index = 0;
      for (int length = this.mTweens.Length; index < length; ++index)
      {
        UITweener mTween = this.mTweens[index];
        if (mTween.tweenGroup == this.tweenGroup)
        {
          if (!flag && !NGUITools.GetActive(go))
          {
            flag = true;
            NGUITools.SetActive(go, true);
          }
          if (this.playDirection == AnimationOrTween.Direction.Toggle)
            mTween.Toggle();
          else
            mTween.Play(forward);
          if (this.resetOnPlay)
            mTween.Reset();
          mTween.onFinished = this.onFinished;
          if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.callWhenFinished))
          {
            mTween.eventReceiver = this.eventReceiver;
            mTween.callWhenFinished = this.callWhenFinished;
          }
        }
      }
    }
  }

  private void Start()
  {
    this.mStarted = true;
    if (!Object.op_Equality((Object) this.tweenTarget, (Object) null))
      return;
    this.tweenTarget = ((Component) this).gameObject;
  }

  private void Update()
  {
    if (this.disableWhenFinished == DisableCondition.DoNotDisable || this.mTweens == null)
      return;
    bool flag1 = true;
    bool flag2 = true;
    int index = 0;
    for (int length = this.mTweens.Length; index < length; ++index)
    {
      UITweener mTween = this.mTweens[index];
      if (mTween.tweenGroup == this.tweenGroup)
      {
        if (((Behaviour) mTween).enabled)
        {
          flag1 = false;
          break;
        }
        if (mTween.direction != (AnimationOrTween.Direction) this.disableWhenFinished)
          flag2 = false;
      }
    }
    if (!flag1)
      return;
    if (flag2)
      NGUITools.SetActive(this.tweenTarget, false);
    this.mTweens = (UITweener[]) null;
  }
}
