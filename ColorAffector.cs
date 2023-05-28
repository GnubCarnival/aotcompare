// Decompiled with JetBrains decompiler
// Type: ColorAffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

public class ColorAffector : Affector
{
  protected Color[] ColorArr;
  protected float ElapsedTime;
  protected float GradualLen;
  protected bool IsNodeLife;
  protected COLOR_GRADUAL_TYPE Type;

  public ColorAffector(
    Color[] colorArr,
    float gradualLen,
    COLOR_GRADUAL_TYPE type,
    EffectNode node)
    : base(node)
  {
    this.ColorArr = colorArr;
    this.Type = type;
    this.GradualLen = gradualLen;
    if ((double) this.GradualLen >= 0.0)
      return;
    this.IsNodeLife = true;
  }

  public override void Reset() => this.ElapsedTime = 0.0f;

  public override void Update()
  {
    this.ElapsedTime += Time.deltaTime;
    if (this.IsNodeLife)
      this.GradualLen = this.Node.GetLifeTime();
    if ((double) this.GradualLen <= 0.0)
      return;
    if ((double) this.ElapsedTime > (double) this.GradualLen)
    {
      if (this.Type == COLOR_GRADUAL_TYPE.CLAMP)
        return;
      if (this.Type == COLOR_GRADUAL_TYPE.LOOP)
      {
        this.ElapsedTime = 0.0f;
      }
      else
      {
        Color[] colorArray = new Color[this.ColorArr.Length];
        this.ColorArr.CopyTo((Array) colorArray, 0);
        for (int index = 0; index < colorArray.Length / 2; ++index)
        {
          this.ColorArr[colorArray.Length - index - 1] = colorArray[index];
          this.ColorArr[index] = colorArray[colorArray.Length - index - 1];
        }
        this.ElapsedTime = 0.0f;
      }
    }
    else
    {
      int index1 = (int) ((double) (this.ColorArr.Length - 1) * ((double) this.ElapsedTime / (double) this.GradualLen));
      if (index1 == this.ColorArr.Length - 1)
        --index1;
      int index2 = index1 + 1;
      float num1 = this.GradualLen / (float) (this.ColorArr.Length - 1);
      float num2 = (this.ElapsedTime - num1 * (float) index1) / num1;
      this.Node.Color = Color.Lerp(this.ColorArr[index1], this.ColorArr[index2], num2);
    }
  }
}
