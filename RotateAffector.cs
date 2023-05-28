// Decompiled with JetBrains decompiler
// Type: RotateAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RotateAffector : Affector
{
  protected float Delta;
  protected AnimationCurve RotateCurve;
  protected RSTYPE Type;

  public RotateAffector(float delta, EffectNode node)
    : base(node)
  {
    this.Type = RSTYPE.SIMPLE;
    this.Delta = delta;
  }

  public RotateAffector(AnimationCurve curve, EffectNode node)
    : base(node)
  {
    this.Type = RSTYPE.CURVE;
    this.RotateCurve = curve;
  }

  public override void Update()
  {
    float elapsedTime = this.Node.GetElapsedTime();
    if (this.Type == RSTYPE.CURVE)
    {
      this.Node.RotateAngle = (float) (int) this.RotateCurve.Evaluate(elapsedTime);
    }
    else
    {
      if (this.Type != RSTYPE.SIMPLE)
        return;
      this.Node.RotateAngle += this.Delta * Time.deltaTime;
    }
  }
}
