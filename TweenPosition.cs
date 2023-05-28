// Decompiled with JetBrains decompiler
// Type: TweenPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPosition : UITweener
{
  public Vector3 from;
  private Transform mTrans;
  public Vector3 to;

  public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
  {
    TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
    tweenPosition.from = tweenPosition.position;
    tweenPosition.to = pos;
    if ((double) duration <= 0.0)
    {
      tweenPosition.Sample(1f, true);
      ((Behaviour) tweenPosition).enabled = false;
    }
    return tweenPosition;
  }

  protected override void OnUpdate(float factor, bool isFinished) => this.cachedTransform.localPosition = Vector3.op_Addition(Vector3.op_Multiply(this.from, 1f - factor), Vector3.op_Multiply(this.to, factor));

  public Transform cachedTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.mTrans, (Object) null))
        this.mTrans = ((Component) this).transform;
      return this.mTrans;
    }
  }

  public Vector3 position
  {
    get => this.cachedTransform.localPosition;
    set => this.cachedTransform.localPosition = value;
  }
}
