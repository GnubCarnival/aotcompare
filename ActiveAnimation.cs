// Decompiled with JetBrains decompiler
// Type: ActiveAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation")]
[RequireComponent(typeof (Animation))]
public class ActiveAnimation : IgnoreTimeScale
{
  public string callWhenFinished;
  public GameObject eventReceiver;
  private Animation mAnim;
  private AnimationOrTween.Direction mDisableDirection;
  private AnimationOrTween.Direction mLastDirection;
  private bool mNotify;
  public ActiveAnimation.OnFinished onFinished;

  private void Play(string clipName, AnimationOrTween.Direction playDirection)
  {
    if (!Object.op_Inequality((Object) this.mAnim, (Object) null))
      return;
    ((Behaviour) this).enabled = true;
    ((Behaviour) this.mAnim).enabled = false;
    if (playDirection == AnimationOrTween.Direction.Toggle)
      playDirection = this.mLastDirection == AnimationOrTween.Direction.Forward ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward;
    if (string.IsNullOrEmpty(clipName))
    {
      if (!this.mAnim.isPlaying)
        this.mAnim.Play();
    }
    else if (!this.mAnim.IsPlaying(clipName))
      this.mAnim.Play(clipName);
    foreach (AnimationState animationState in this.mAnim)
    {
      if (string.IsNullOrEmpty(clipName) || animationState.name == clipName)
      {
        float num = Mathf.Abs(animationState.speed);
        animationState.speed = num * (float) playDirection;
        if (playDirection == AnimationOrTween.Direction.Reverse && (double) animationState.time == 0.0)
          animationState.time = animationState.length;
        else if (playDirection == AnimationOrTween.Direction.Forward && (double) animationState.time == (double) animationState.length)
          animationState.time = 0.0f;
      }
    }
    this.mLastDirection = playDirection;
    this.mNotify = true;
    this.mAnim.Sample();
  }

  public static ActiveAnimation Play(Animation anim, AnimationOrTween.Direction playDirection) => ActiveAnimation.Play(anim, (string) null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);

  public static ActiveAnimation Play(Animation anim, string clipName, AnimationOrTween.Direction playDirection) => ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);

  public static ActiveAnimation Play(
    Animation anim,
    string clipName,
    AnimationOrTween.Direction playDirection,
    EnableCondition enableBeforePlay,
    DisableCondition disableCondition)
  {
    if (!NGUITools.GetActive(((Component) anim).gameObject))
    {
      if (enableBeforePlay != EnableCondition.EnableThenPlay)
        return (ActiveAnimation) null;
      NGUITools.SetActive(((Component) anim).gameObject, true);
      UIPanel[] componentsInChildren = ((Component) anim).gameObject.GetComponentsInChildren<UIPanel>();
      int index = 0;
      for (int length = componentsInChildren.Length; index < length; ++index)
        componentsInChildren[index].Refresh();
    }
    ActiveAnimation activeAnimation = ((Component) anim).GetComponent<ActiveAnimation>();
    if (Object.op_Equality((Object) activeAnimation, (Object) null))
      activeAnimation = ((Component) anim).gameObject.AddComponent<ActiveAnimation>();
    activeAnimation.mAnim = anim;
    activeAnimation.mDisableDirection = (AnimationOrTween.Direction) disableCondition;
    activeAnimation.eventReceiver = (GameObject) null;
    activeAnimation.callWhenFinished = (string) null;
    activeAnimation.onFinished = (ActiveAnimation.OnFinished) null;
    activeAnimation.Play(clipName, playDirection);
    return activeAnimation;
  }

  public void Reset()
  {
    if (!Object.op_Inequality((Object) this.mAnim, (Object) null))
      return;
    foreach (AnimationState animationState in this.mAnim)
    {
      if (this.mLastDirection == AnimationOrTween.Direction.Reverse)
        animationState.time = animationState.length;
      else if (this.mLastDirection == AnimationOrTween.Direction.Forward)
        animationState.time = 0.0f;
    }
  }

  private void Update()
  {
    float num1 = this.UpdateRealTimeDelta();
    if ((double) num1 == 0.0)
      return;
    if (Object.op_Inequality((Object) this.mAnim, (Object) null))
    {
      bool flag = false;
      foreach (AnimationState animationState in this.mAnim)
      {
        if (this.mAnim.IsPlaying(animationState.name))
        {
          float num2 = animationState.speed * num1;
          animationState.time += num2;
          if ((double) num2 < 0.0)
          {
            if ((double) animationState.time > 0.0)
              flag = true;
            else
              animationState.time = 0.0f;
          }
          else if ((double) animationState.time < (double) animationState.length)
            flag = true;
          else
            animationState.time = animationState.length;
        }
      }
      this.mAnim.Sample();
      if (flag)
        return;
      ((Behaviour) this).enabled = false;
      if (!this.mNotify)
        return;
      this.mNotify = false;
      if (this.onFinished != null)
        this.onFinished(this);
      if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.callWhenFinished))
        this.eventReceiver.SendMessage(this.callWhenFinished, (object) this, (SendMessageOptions) 1);
      if (this.mDisableDirection == AnimationOrTween.Direction.Toggle || this.mLastDirection != this.mDisableDirection)
        return;
      NGUITools.SetActive(((Component) this).gameObject, false);
    }
    else
      ((Behaviour) this).enabled = false;
  }

  public bool isPlaying
  {
    get
    {
      if (Object.op_Inequality((Object) this.mAnim, (Object) null))
      {
        foreach (AnimationState animationState in this.mAnim)
        {
          if (this.mAnim.IsPlaying(animationState.name))
          {
            if (this.mLastDirection == AnimationOrTween.Direction.Forward)
            {
              if ((double) animationState.time < (double) animationState.length)
                return true;
            }
            else if (this.mLastDirection != AnimationOrTween.Direction.Reverse || (double) animationState.time > 0.0)
              return true;
          }
        }
      }
      return false;
    }
  }

  public delegate void OnFinished(ActiveAnimation anim);
}
