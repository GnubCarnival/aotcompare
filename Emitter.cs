// Decompiled with JetBrains decompiler
// Type: Emitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

public class Emitter
{
  private float EmitDelayTime;
  private float EmitLoop;
  private float EmitterElapsedTime;
  private bool IsFirstEmit = true;
  private Vector3 LastClientPos = Vector3.zero;
  public EffectLayer Layer;

  public Emitter(EffectLayer owner)
  {
    this.Layer = owner;
    this.EmitLoop = (float) this.Layer.EmitLoop;
    this.LastClientPos = this.Layer.ClientTransform.position;
  }

  protected int EmitByDistance()
  {
    Vector3 vector3 = Vector3.op_Subtraction(this.Layer.ClientTransform.position, this.LastClientPos);
    if ((double) ((Vector3) ref vector3).magnitude < (double) this.Layer.DiffDistance)
      return 0;
    this.LastClientPos = this.Layer.ClientTransform.position;
    return 1;
  }

  protected int EmitByRate()
  {
    int num1 = Random.Range(0, 100);
    if (num1 >= 0 && (double) num1 > (double) this.Layer.ChanceToEmit)
      return 0;
    this.EmitDelayTime += Time.deltaTime;
    if ((double) this.EmitDelayTime < (double) this.Layer.EmitDelay && !this.IsFirstEmit)
      return 0;
    this.EmitterElapsedTime += Time.deltaTime;
    if ((double) this.EmitterElapsedTime >= (double) this.Layer.EmitDuration)
    {
      if ((double) this.EmitLoop > 0.0)
        --this.EmitLoop;
      this.EmitterElapsedTime = 0.0f;
      this.EmitDelayTime = 0.0f;
      this.IsFirstEmit = false;
    }
    if ((double) this.EmitLoop == 0.0 || this.Layer.AvailableNodeCount == 0)
      return 0;
    int num2 = (int) ((double) this.EmitterElapsedTime * (double) this.Layer.EmitRate) - (this.Layer.ActiveENodes.Length - this.Layer.AvailableNodeCount);
    int num3 = num2 <= this.Layer.AvailableNodeCount ? num2 : this.Layer.AvailableNodeCount;
    return num3 <= 0 ? 0 : num3;
  }

  public Vector3 GetEmitRotation(EffectNode node)
  {
    Vector3 zero = Vector3.zero;
    if (this.Layer.EmitType == 2)
      return !this.Layer.SyncClient ? Vector3.op_Subtraction(node.Position, Vector3.op_Addition(this.Layer.ClientTransform.position, this.Layer.EmitPoint)) : Vector3.op_Subtraction(node.Position, this.Layer.EmitPoint);
    if (this.Layer.EmitType == 3)
    {
      Vector3 vector3_1 = this.Layer.SyncClient ? Vector3.op_Subtraction(node.Position, this.Layer.EmitPoint) : Vector3.op_Subtraction(node.Position, Vector3.op_Addition(this.Layer.ClientTransform.position, this.Layer.EmitPoint));
      Vector3 vector3_2 = Vector3.RotateTowards(vector3_1, this.Layer.CircleDir, (float) (90 - this.Layer.AngleAroundAxis) * ((float) Math.PI / 180f), 1f);
      return Quaternion.op_Multiply(Quaternion.FromToRotation(vector3_1, vector3_2), vector3_1);
    }
    if (!this.Layer.IsRandomDir)
      return this.Layer.OriVelocityAxis;
    Quaternion quaternion1 = Quaternion.Euler(0.0f, 0.0f, (float) this.Layer.AngleAroundAxis);
    Quaternion quaternion2 = Quaternion.Euler(0.0f, (float) Random.Range(0, 360), 0.0f);
    return Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.FromToRotation(Vector3.up, this.Layer.OriVelocityAxis), quaternion2), quaternion1), Vector3.up);
  }

  public int GetNodes() => this.Layer.IsEmitByDistance ? this.EmitByDistance() : this.EmitByRate();

  public void Reset()
  {
    this.EmitterElapsedTime = 0.0f;
    this.EmitDelayTime = 0.0f;
    this.IsFirstEmit = true;
    this.EmitLoop = (float) this.Layer.EmitLoop;
  }

  public void SetEmitPosition(EffectNode node)
  {
    Vector3 pos = Vector3.zero;
    if (this.Layer.EmitType == 1)
    {
      Vector3 emitPoint = this.Layer.EmitPoint;
      float num1 = Random.Range(emitPoint.x - this.Layer.BoxSize.x / 2f, emitPoint.x + this.Layer.BoxSize.x / 2f);
      float num2 = Random.Range(emitPoint.y - this.Layer.BoxSize.y / 2f, emitPoint.y + this.Layer.BoxSize.y / 2f);
      float num3 = Random.Range(emitPoint.z - this.Layer.BoxSize.z / 2f, emitPoint.z + this.Layer.BoxSize.z / 2f);
      pos.x = num1;
      pos.y = num2;
      pos.z = num3;
      if (!this.Layer.SyncClient)
        pos = Vector3.op_Addition(this.Layer.ClientTransform.position, pos);
    }
    else if (this.Layer.EmitType == 0)
    {
      pos = this.Layer.EmitPoint;
      if (!this.Layer.SyncClient)
        pos = Vector3.op_Addition(this.Layer.ClientTransform.position, this.Layer.EmitPoint);
    }
    else if (this.Layer.EmitType == 2)
    {
      Vector3 vector3_1 = this.Layer.EmitPoint;
      if (!this.Layer.SyncClient)
        vector3_1 = Vector3.op_Addition(this.Layer.ClientTransform.position, this.Layer.EmitPoint);
      Vector3 vector3_2 = Vector3.op_Multiply(Vector3.up, this.Layer.Radius);
      pos = Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.Euler((float) Random.Range(0, 360), (float) Random.Range(0, 360), (float) Random.Range(0, 360)), vector3_2), vector3_1);
    }
    else if (this.Layer.EmitType == 4)
    {
      Vector3 vector3_3 = Vector3.op_Addition(this.Layer.EmitPoint, Vector3.op_Multiply(Quaternion.op_Multiply(this.Layer.ClientTransform.localRotation, Vector3.forward), this.Layer.LineLengthLeft));
      Vector3 vector3_4 = Vector3.op_Subtraction(Vector3.op_Addition(this.Layer.EmitPoint, Vector3.op_Multiply(Quaternion.op_Multiply(this.Layer.ClientTransform.localRotation, Vector3.forward), this.Layer.LineLengthRight)), vector3_3);
      float num4 = (float) (node.Index + 1) / (float) this.Layer.MaxENodes;
      float num5 = ((Vector3) ref vector3_4).magnitude * num4;
      pos = Vector3.op_Addition(vector3_3, Vector3.op_Multiply(((Vector3) ref vector3_4).normalized, num5));
      if (!this.Layer.SyncClient)
        pos = this.Layer.ClientTransform.TransformPoint(pos);
    }
    else if (this.Layer.EmitType == 3)
    {
      Vector3 vector3_5 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 360f * ((float) (node.Index + 1) / (float) this.Layer.MaxENodes), 0.0f), Vector3.op_Multiply(Vector3.right, this.Layer.Radius));
      Vector3 vector3_6 = Quaternion.op_Multiply(Quaternion.FromToRotation(Vector3.up, this.Layer.CircleDir), vector3_5);
      pos = this.Layer.SyncClient ? Vector3.op_Addition(vector3_6, this.Layer.EmitPoint) : Vector3.op_Addition(Vector3.op_Addition(this.Layer.ClientTransform.position, vector3_6), this.Layer.EmitPoint);
    }
    node.SetLocalPosition(pos);
  }
}
