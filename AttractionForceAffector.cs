// Decompiled with JetBrains decompiler
// Type: AttractionForceAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class AttractionForceAffector : Affector
{
  private AnimationCurve AttractionCurve;
  private float Magnitude;
  protected Vector3 Position;
  private bool UseCurve;

  public AttractionForceAffector(float magnitude, Vector3 pos, EffectNode node)
    : base(node)
  {
    this.Magnitude = magnitude;
    this.Position = pos;
    this.UseCurve = false;
  }

  public AttractionForceAffector(AnimationCurve curve, Vector3 pos, EffectNode node)
    : base(node)
  {
    this.AttractionCurve = curve;
    this.Position = pos;
    this.UseCurve = true;
  }

  public override void Update()
  {
    Vector3 vector3 = !this.Node.SyncClient ? Vector3.op_Subtraction(Vector3.op_Addition(this.Node.ClientTrans.position, this.Position), this.Node.GetLocalPosition()) : Vector3.op_Subtraction(this.Position, this.Node.GetLocalPosition());
    float elapsedTime = this.Node.GetElapsedTime();
    float num = !this.UseCurve ? this.Magnitude : this.AttractionCurve.Evaluate(elapsedTime);
    EffectNode node = this.Node;
    node.Velocity = Vector3.op_Addition(node.Velocity, Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3).normalized, num), Time.deltaTime));
  }
}
