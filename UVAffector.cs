// Decompiled with JetBrains decompiler
// Type: UVAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class UVAffector : Affector
{
  protected float ElapsedTime;
  protected UVAnimation Frames;
  protected float UVTime;

  public UVAffector(UVAnimation frame, float time, EffectNode node)
    : base(node)
  {
    this.Frames = frame;
    this.UVTime = time;
  }

  public override void Reset()
  {
    this.ElapsedTime = 0.0f;
    this.Frames.curFrame = 0;
  }

  public override void Update()
  {
    this.ElapsedTime += Time.deltaTime;
    float num = (double) this.UVTime > 0.0 ? this.UVTime / (float) this.Frames.frames.Length : this.Node.GetLifeTime() / (float) this.Frames.frames.Length;
    if ((double) this.ElapsedTime < (double) num)
      return;
    Vector2 zero1 = Vector2.zero;
    Vector2 zero2 = Vector2.zero;
    this.Frames.GetNextFrame(ref zero1, ref zero2);
    this.Node.LowerLeftUV = zero1;
    this.Node.UVDimensions = zero2;
    this.ElapsedTime -= num;
  }
}
