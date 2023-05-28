// Decompiled with JetBrains decompiler
// Type: TweenFOV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Field of View")]
[RequireComponent(typeof (Camera))]
public class TweenFOV : UITweener
{
  public float from;
  private Camera mCam;
  public float to;

  public static TweenFOV Begin(GameObject go, float duration, float to)
  {
    TweenFOV tweenFov = UITweener.Begin<TweenFOV>(go, duration);
    tweenFov.from = tweenFov.fov;
    tweenFov.to = to;
    if ((double) duration <= 0.0)
    {
      tweenFov.Sample(1f, true);
      ((Behaviour) tweenFov).enabled = false;
    }
    return tweenFov;
  }

  protected override void OnUpdate(float factor, bool isFinished) => this.cachedCamera.fieldOfView = (float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor);

  public Camera cachedCamera
  {
    get
    {
      if (Object.op_Equality((Object) this.mCam, (Object) null))
        this.mCam = ((Component) this).camera;
      return this.mCam;
    }
  }

  public float fov
  {
    get => this.cachedCamera.fieldOfView;
    set => this.cachedCamera.fieldOfView = value;
  }
}
