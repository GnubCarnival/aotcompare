// Decompiled with JetBrains decompiler
// Type: TweenRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Rotation")]
public class TweenRotation : UITweener
{
  public Vector3 from;
  private Transform mTrans;
  public Vector3 to;

  public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
  {
    TweenRotation tweenRotation1 = UITweener.Begin<TweenRotation>(go, duration);
    TweenRotation tweenRotation2 = tweenRotation1;
    Quaternion rotation = tweenRotation1.rotation;
    Vector3 eulerAngles = ((Quaternion) ref rotation).eulerAngles;
    tweenRotation2.from = eulerAngles;
    tweenRotation1.to = ((Quaternion) ref rot).eulerAngles;
    if ((double) duration <= 0.0)
    {
      tweenRotation1.Sample(1f, true);
      ((Behaviour) tweenRotation1).enabled = false;
    }
    return tweenRotation1;
  }

  protected override void OnUpdate(float factor, bool isFinished) => this.cachedTransform.localRotation = Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor);

  public Transform cachedTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.mTrans, (Object) null))
        this.mTrans = ((Component) this).transform;
      return this.mTrans;
    }
  }

  public Quaternion rotation
  {
    get => this.cachedTransform.localRotation;
    set => this.cachedTransform.localRotation = value;
  }
}
