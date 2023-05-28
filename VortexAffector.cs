// Decompiled with JetBrains decompiler
// Type: VortexAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class VortexAffector : Affector
{
  protected Vector3 Direction;
  private float Magnitude;
  private bool UseCurve;
  private AnimationCurve VortexCurve;

  public VortexAffector(float mag, Vector3 dir, EffectNode node)
    : base(node)
  {
    this.Magnitude = mag;
    this.Direction = dir;
    this.UseCurve = false;
  }

  public VortexAffector(AnimationCurve vortexCurve, Vector3 dir, EffectNode node)
    : base(node)
  {
    this.VortexCurve = vortexCurve;
    this.Direction = dir;
    this.UseCurve = true;
  }

  public override void Update()
  {
    Vector3 vector3_1 = Vector3.op_Subtraction(this.Node.GetLocalPosition(), this.Node.Owner.EmitPoint);
    if ((double) ((Vector3) ref vector3_1).magnitude == 0.0)
      return;
    float num1 = Vector3.Dot(this.Direction, vector3_1);
    Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, Vector3.op_Multiply(num1, this.Direction));
    Vector3 zero = Vector3.zero;
    Vector3 vector3_3;
    if (Vector3.op_Equality(vector3_2, Vector3.zero))
    {
      vector3_3 = vector3_2;
    }
    else
    {
      Vector3 vector3_4 = Vector3.Cross(this.Direction, vector3_2);
      vector3_3 = ((Vector3) ref vector3_4).normalized;
    }
    float elapsedTime = this.Node.GetElapsedTime();
    float num2 = !this.UseCurve ? this.Magnitude : this.VortexCurve.Evaluate(elapsedTime);
    Vector3 vector3_5 = Vector3.op_Multiply(vector3_3, num2 * Time.deltaTime);
    EffectNode node = this.Node;
    node.Position = Vector3.op_Addition(node.Position, vector3_5);
  }
}
