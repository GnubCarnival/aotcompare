// Decompiled with JetBrains decompiler
// Type: TweenOrthoSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Orthographic Size")]
[RequireComponent(typeof (Camera))]
public class TweenOrthoSize : UITweener
{
  public float from;
  private Camera mCam;
  public float to;

  public static TweenOrthoSize Begin(GameObject go, float duration, float to)
  {
    TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
    tweenOrthoSize.from = tweenOrthoSize.orthoSize;
    tweenOrthoSize.to = to;
    if ((double) duration <= 0.0)
    {
      tweenOrthoSize.Sample(1f, true);
      ((Behaviour) tweenOrthoSize).enabled = false;
    }
    return tweenOrthoSize;
  }

  protected override void OnUpdate(float factor, bool isFinished) => this.cachedCamera.orthographicSize = (float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor);

  public Camera cachedCamera
  {
    get
    {
      if (Object.op_Equality((Object) this.mCam, (Object) null))
        this.mCam = ((Component) this).camera;
      return this.mCam;
    }
  }

  public float orthoSize
  {
    get => this.cachedCamera.orthographicSize;
    set => this.cachedCamera.orthographicSize = value;
  }
}
