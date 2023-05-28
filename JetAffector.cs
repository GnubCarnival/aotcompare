// Decompiled with JetBrains decompiler
// Type: JetAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class JetAffector : Affector
{
  protected float MaxAcceleration;
  protected float MinAcceleration;

  public JetAffector(float min, float max, EffectNode node)
    : base(node)
  {
    this.MinAcceleration = min;
    this.MaxAcceleration = max;
  }

  public override void Update()
  {
    if ((double) Mathf.Abs(this.Node.Acceleration) >= 1E-06)
      return;
    this.Node.Acceleration = Random.Range(this.MinAcceleration, this.MaxAcceleration);
  }
}
