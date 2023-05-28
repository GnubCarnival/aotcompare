// Decompiled with JetBrains decompiler
// Type: ScaleAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class ScaleAffector : Affector
{
  protected float DeltaX;
  protected float DeltaY;
  protected AnimationCurve ScaleXCurve;
  protected AnimationCurve ScaleYCurve;
  protected RSTYPE Type;

  public ScaleAffector(float x, float y, EffectNode node)
    : base(node)
  {
    this.Type = RSTYPE.SIMPLE;
    this.DeltaX = x;
    this.DeltaY = y;
  }

  public ScaleAffector(AnimationCurve curveX, AnimationCurve curveY, EffectNode node)
    : base(node)
  {
    this.Type = RSTYPE.CURVE;
    this.ScaleXCurve = curveX;
    this.ScaleYCurve = curveY;
  }

  public override void Update()
  {
    float elapsedTime = this.Node.GetElapsedTime();
    if (this.Type == RSTYPE.CURVE)
    {
      if (this.ScaleXCurve != null)
        this.Node.Scale.x = this.ScaleXCurve.Evaluate(elapsedTime);
      if (this.ScaleYCurve == null)
        return;
      this.Node.Scale.y = this.ScaleYCurve.Evaluate(elapsedTime);
    }
    else
    {
      if (this.Type != RSTYPE.SIMPLE)
        return;
      float num1 = this.Node.Scale.x + this.DeltaX * Time.deltaTime;
      float num2 = this.Node.Scale.y + this.DeltaY * Time.deltaTime;
      if ((double) num1 * (double) this.Node.Scale.x > 0.0)
        this.Node.Scale.x = num1;
      if ((double) num2 * (double) this.Node.Scale.y <= 0.0)
        return;
      this.Node.Scale.y = num2;
    }
  }
}
