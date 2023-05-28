// Decompiled with JetBrains decompiler
// Type: IgnoreTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Internal/Ignore TimeScale Behaviour")]
public class IgnoreTimeScale : MonoBehaviour
{
  private float mActual;
  private float mRt;
  private float mTimeDelta;
  private float mTimeStart;
  private bool mTimeStarted;

  protected virtual void OnEnable()
  {
    this.mTimeStarted = true;
    this.mTimeDelta = 0.0f;
    this.mTimeStart = Time.realtimeSinceStartup;
  }

  protected float UpdateRealTimeDelta()
  {
    this.mRt = Time.realtimeSinceStartup;
    if (this.mTimeStarted)
    {
      this.mActual += Mathf.Max(0.0f, this.mRt - this.mTimeStart);
      this.mTimeDelta = 1f / 1000f * Mathf.Round(this.mActual * 1000f);
      this.mActual -= this.mTimeDelta;
      if ((double) this.mTimeDelta > 1.0)
        this.mTimeDelta = 1f;
      this.mTimeStart = this.mRt;
    }
    else
    {
      this.mTimeStarted = true;
      this.mTimeStart = this.mRt;
      this.mTimeDelta = 0.0f;
    }
    return this.mTimeDelta;
  }

  public float realTime => this.mRt;

  public float realTimeDelta => this.mTimeDelta;
}
