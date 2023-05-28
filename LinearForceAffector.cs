// Decompiled with JetBrains decompiler
// Type: LinearForceAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LinearForceAffector : Affector
{
  protected Vector3 Force;

  public LinearForceAffector(Vector3 force, EffectNode node)
    : base(node)
  {
    this.Force = force;
  }

  public override void Update()
  {
    EffectNode node = this.Node;
    node.Velocity = Vector3.op_Addition(node.Velocity, Vector3.op_Multiply(this.Force, Time.deltaTime));
  }
}
