// Decompiled with JetBrains decompiler
// Type: STransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public struct STransform
{
  public Vector3 position;
  public Quaternion rotation;

  public void Reset()
  {
    this.position = Vector3.zero;
    this.rotation = Quaternion.identity;
  }

  public void LookAt(Vector3 target, Vector3 up) => this.rotation = Quaternion.LookRotation(Vector3.op_Subtraction(target, this.position), up);
}
