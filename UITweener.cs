// Decompiled with JetBrains decompiler
// Type: UITweener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public abstract class UITweener : IgnoreTimeScale
{
  public AnimationCurve animationCurve;
  public string callWhenFinished;
  public float delay;
  public float duration;
  public GameObject eventReceiver;
  public bool ignoreTimeScale;
  private float mAmountPerDelta;
  private float mDuration;
  public UITweener.Method method;
  private float mFactor;
  private bool mStarted;
  private float mStartTime;
  public UITweener.OnFinished onFinished;
  public bool steeperCurves;
  public UITweener.Style style;
  public int tweenGroup;

  protected UITweener()
  {
    this.animationCurve = new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f, 0.0f, 1f),
      new Keyframe(1f, 1f, 1f, 0.0f)
    });
    this.ignoreTimeScale = true;
    this.duration = 1f;
    this.mAmountPerDelta = 1f;
  }

  public static T Begin<T>(GameObject go, float duration) where T : UITweener
  {
    T obj = go.GetComponent<T>();
    if (Object.op_Equality((Object) (object) obj, (Object) null))
      obj = go.AddComponent<T>();
    obj.mStarted = false;
    obj.duration = duration;
    obj.mFactor = 0.0f;
    obj.mAmountPerDelta = Mathf.Abs(obj.mAmountPerDelta);
    obj.style = UITweener.Style.Once;
    Keyframe[] keyframeArray = new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f, 0.0f, 1f),
      new Keyframe(1f, 1f, 1f, 0.0f)
    };
    obj.animationCurve = new AnimationCurve(keyframeArray);
    obj.eventReceiver = (GameObject) null;
    obj.callWhenFinished = (string) null;
    obj.onFinished = (UITweener.OnFinished) null;
    ((Behaviour) (object) obj).enabled = true;
    return obj;
  }

  private float BounceLogic(float val)
  {
    if ((double) val < 0.36363598704338074)
    {
      val = 7.5685f * val * val;
      return val;
    }
    if ((double) val < 0.72727197408676147)
    {
      val = (float) (121.0 / 16.0 * (double) (val -= 0.545454f) * (double) val + 0.75);
      return val;
    }
    if ((double) val < 0.909089982509613)
    {
      val = (float) (121.0 / 16.0 * (double) (val -= 0.818181f) * (double) val + 15.0 / 16.0);
      return val;
    }
    val = (float) (121.0 / 16.0 * (double) (val -= 0.9545454f) * (double) val + 63.0 / 64.0);
    return val;
  }

  private void OnDisable() => this.mStarted = false;

  protected abstract void OnUpdate(float factor, bool isFinished);

  public void Play(bool forward)
  {
    this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
    if (!forward)
      this.mAmountPerDelta = -this.mAmountPerDelta;
    ((Behaviour) this).enabled = true;
  }

  public void Reset()
  {
    this.mStarted = false;
    this.mFactor = (double) this.mAmountPerDelta >= 0.0 ? 0.0f : 1f;
    this.Sample(this.mFactor, false);
  }

  public void Sample(float factor, bool isFinished)
  {
    float val = Mathf.Clamp01(factor);
    if (this.method == UITweener.Method.EaseIn)
    {
      val = 1f - Mathf.Sin((float) (1.570796012878418 * (1.0 - (double) val)));
      if (this.steeperCurves)
        val *= val;
    }
    else if (this.method == UITweener.Method.EaseOut)
    {
      val = Mathf.Sin(1.570796f * val);
      if (this.steeperCurves)
      {
        float num = 1f - val;
        val = (float) (1.0 - (double) num * (double) num);
      }
    }
    else if (this.method == UITweener.Method.EaseInOut)
    {
      val -= Mathf.Sin(val * 6.283185f) / 6.283185f;
      if (this.steeperCurves)
      {
        float num1 = (float) ((double) val * 2.0 - 1.0);
        double num2 = (double) Mathf.Sign(num1);
        float num3 = 1f - Mathf.Abs(num1);
        double num4 = 1.0 - (double) num3 * (double) num3;
        val = (float) (num2 * num4 * 0.5 + 0.5);
      }
    }
    else if (this.method == UITweener.Method.BounceIn)
      val = this.BounceLogic(val);
    else if (this.method == UITweener.Method.BounceOut)
      val = 1f - this.BounceLogic(1f - val);
    this.OnUpdate(this.animationCurve == null ? val : this.animationCurve.Evaluate(val), isFinished);
  }

  private void Start() => this.Update();

  public void Toggle()
  {
    this.mAmountPerDelta = (double) this.mFactor <= 0.0 ? Mathf.Abs(this.amountPerDelta) : -this.amountPerDelta;
    ((Behaviour) this).enabled = true;
  }

  private void Update()
  {
    float num1 = !this.ignoreTimeScale ? Time.deltaTime : this.UpdateRealTimeDelta();
    float num2 = !this.ignoreTimeScale ? Time.time : this.realTime;
    if (!this.mStarted)
    {
      this.mStarted = true;
      this.mStartTime = num2 + this.delay;
    }
    if ((double) num2 < (double) this.mStartTime)
      return;
    this.mFactor += this.amountPerDelta * num1;
    if (this.style == UITweener.Style.Loop)
    {
      if ((double) this.mFactor > 1.0)
        this.mFactor -= Mathf.Floor(this.mFactor);
    }
    else if (this.style == UITweener.Style.PingPong)
    {
      if ((double) this.mFactor > 1.0)
      {
        this.mFactor = (float) (1.0 - ((double) this.mFactor - (double) Mathf.Floor(this.mFactor)));
        this.mAmountPerDelta = -this.mAmountPerDelta;
      }
      else if ((double) this.mFactor < 0.0)
      {
        this.mFactor = -this.mFactor;
        this.mFactor -= Mathf.Floor(this.mFactor);
        this.mAmountPerDelta = -this.mAmountPerDelta;
      }
    }
    if (this.style == UITweener.Style.Once && ((double) this.mFactor > 1.0 || (double) this.mFactor < 0.0))
    {
      this.mFactor = Mathf.Clamp01(this.mFactor);
      this.Sample(this.mFactor, true);
      if (this.onFinished != null)
        this.onFinished(this);
      if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.callWhenFinished))
        this.eventReceiver.SendMessage(this.callWhenFinished, (object) this, (SendMessageOptions) 1);
      if (((double) this.mFactor != 1.0 || (double) this.mAmountPerDelta <= 0.0) && ((double) this.mFactor != 0.0 || (double) this.mAmountPerDelta >= 0.0))
        return;
      ((Behaviour) this).enabled = false;
    }
    else
      this.Sample(this.mFactor, false);
  }

  public float amountPerDelta
  {
    get
    {
      if ((double) this.mDuration != (double) this.duration)
      {
        this.mDuration = this.duration;
        this.mAmountPerDelta = Mathf.Abs((double) this.duration <= 0.0 ? 1000f : 1f / this.duration);
      }
      return this.mAmountPerDelta;
    }
  }

  public AnimationOrTween.Direction direction => (double) this.mAmountPerDelta < 0.0 ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward;

  public float tweenFactor => this.mFactor;

  public enum Method
  {
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
    BounceIn,
    BounceOut,
  }

  public delegate void OnFinished(UITweener tween);

  public enum Style
  {
    Once,
    Loop,
    PingPong,
  }
}
