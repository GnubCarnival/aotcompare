// Decompiled with JetBrains decompiler
// Type: TweenScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Scale")]
public class TweenScale : UITweener
{
  public Vector3 from = Vector3.one;
  private UITable mTable;
  private Transform mTrans;
  public Vector3 to = Vector3.one;
  public bool updateTable;

  public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
  {
    TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
    tweenScale.from = tweenScale.scale;
    tweenScale.to = scale;
    if ((double) duration <= 0.0)
    {
      tweenScale.Sample(1f, true);
      ((Behaviour) tweenScale).enabled = false;
    }
    return tweenScale;
  }

  protected override void OnUpdate(float factor, bool isFinished)
  {
    this.cachedTransform.localScale = Vector3.op_Addition(Vector3.op_Multiply(this.from, 1f - factor), Vector3.op_Multiply(this.to, factor));
    if (!this.updateTable)
      return;
    if (Object.op_Equality((Object) this.mTable, (Object) null))
    {
      this.mTable = NGUITools.FindInParents<UITable>(((Component) this).gameObject);
      if (Object.op_Equality((Object) this.mTable, (Object) null))
      {
        this.updateTable = false;
        return;
      }
    }
    this.mTable.repositionNow = true;
  }

  public Transform cachedTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.mTrans, (Object) null))
        this.mTrans = ((Component) this).transform;
      return this.mTrans;
    }
  }

  public Vector3 scale
  {
    get => this.cachedTransform.localScale;
    set => this.cachedTransform.localScale = value;
  }
}
