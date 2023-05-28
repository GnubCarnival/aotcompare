// Decompiled with JetBrains decompiler
// Type: UI.BasePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
  internal class BasePopup : HeadedPanel
  {
    protected float _currentAnimationValue;
    protected HashSet<Transform> _staticTransforms = new HashSet<Transform>();

    protected virtual float MinTweenScale => 0.3f;

    protected virtual float MaxTweenScale => 1f;

    protected virtual float MinFadeAlpha => 0.0f;

    protected virtual float MaxFadeAlpha => 1f;

    protected virtual float AnimationTime => 0.1f;

    protected virtual PopupAnimation PopupAnimationType => PopupAnimation.Tween;

    public override void Show()
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      base.Show();
      ((Component) this).transform.SetAsLastSibling();
      this.StopAllCoroutines();
      if (this.PopupAnimationType == PopupAnimation.Tween)
      {
        this.StartCoroutine(this.TweenIn());
      }
      else
      {
        if (this.PopupAnimationType != PopupAnimation.Fade)
          return;
        this.StartCoroutine(this.FadeIn());
      }
    }

    public override void Hide()
    {
      if (!((Component) this).gameObject.activeSelf)
        return;
      this.HideAllPopups();
      this.StopAllCoroutines();
      if (this.PopupAnimationType == PopupAnimation.Tween)
        this.StartCoroutine(this.TweenOut());
      else if (this.PopupAnimationType == PopupAnimation.Fade)
      {
        this.StartCoroutine(this.FadeOut());
      }
      else
      {
        if (this.PopupAnimationType != PopupAnimation.None)
          return;
        this.FinishHide();
      }
    }

    protected virtual void FinishHide() => ((Component) this).gameObject.SetActive(false);

    protected IEnumerator TweenIn()
    {
      this._currentAnimationValue = this.MinTweenScale;
      while ((double) this._currentAnimationValue < (double) this.MaxTweenScale)
      {
        this.SetTransformScale(this._currentAnimationValue);
        this._currentAnimationValue += this.GetAnimmationSpeed(this.MinTweenScale, this.MaxTweenScale) * Time.unscaledDeltaTime;
        yield return (object) null;
      }
      this.SetTransformScale(this.MaxTweenScale);
    }

    protected IEnumerator TweenOut()
    {
      this._currentAnimationValue = this.MaxTweenScale;
      while ((double) this._currentAnimationValue > (double) this.MinTweenScale)
      {
        this.SetTransformScale(this._currentAnimationValue);
        this._currentAnimationValue -= this.GetAnimmationSpeed(this.MinTweenScale, this.MaxTweenScale) * Time.unscaledDeltaTime;
        yield return (object) null;
      }
      this.SetTransformScale(this.MinTweenScale);
      this.FinishHide();
    }

    protected IEnumerator FadeIn()
    {
      this._currentAnimationValue = this.MinFadeAlpha;
      while ((double) this._currentAnimationValue < (double) this.MaxFadeAlpha)
      {
        this.SetTransformAlpha(this._currentAnimationValue);
        this._currentAnimationValue += this.GetAnimmationSpeed(this.MinFadeAlpha, this.MaxFadeAlpha) * Time.unscaledDeltaTime;
        yield return (object) null;
      }
      this.SetTransformAlpha(this.MaxFadeAlpha);
    }

    protected IEnumerator FadeOut()
    {
      this._currentAnimationValue = this.MaxFadeAlpha;
      while ((double) this._currentAnimationValue > (double) this.MinFadeAlpha)
      {
        this.SetTransformAlpha(this._currentAnimationValue);
        this._currentAnimationValue -= this.GetAnimmationSpeed(this.MinFadeAlpha, this.MaxFadeAlpha) * Time.unscaledDeltaTime;
        yield return (object) null;
      }
      this.SetTransformAlpha(this.MinFadeAlpha);
      this.FinishHide();
    }

    protected void SetTransformScale(float scale)
    {
      ((Component) this).transform.localScale = this.GetVectorFromScale(scale);
      foreach (Transform staticTransform in this._staticTransforms)
      {
        float num = 1f;
        IgnoreScaler component = ((Component) staticTransform).GetComponent<IgnoreScaler>();
        if (Object.op_Inequality((Object) component, (Object) null))
          num = component.Scale;
        staticTransform.localScale = this.GetVectorFromScale(num / Mathf.Max(scale, 0.1f));
      }
    }

    protected void SetTransformAlpha(float alpha) => ((Component) ((Component) this).transform).GetComponent<CanvasGroup>().alpha = alpha;

    private Vector3 GetVectorFromScale(float scale) => new Vector3(scale, scale, scale);

    private float GetAnimmationSpeed(float min, float max) => (max - min) / this.AnimationTime;
  }
}
