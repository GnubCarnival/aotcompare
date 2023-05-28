// Decompiled with JetBrains decompiler
// Type: CameraFacingBillboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
  public CameraFacingBillboard.Axis axis;
  private Camera referenceCamera;
  public bool reverseFace;

  private void Awake()
  {
    if (!Object.op_Equality((Object) this.referenceCamera, (Object) null))
      return;
    this.referenceCamera = Camera.main;
  }

  public Vector3 GetAxis(CameraFacingBillboard.Axis refAxis)
  {
    switch (refAxis)
    {
      case CameraFacingBillboard.Axis.down:
        return Vector3.down;
      case CameraFacingBillboard.Axis.left:
        return Vector3.left;
      case CameraFacingBillboard.Axis.right:
        return Vector3.right;
      case CameraFacingBillboard.Axis.forward:
        return Vector3.forward;
      case CameraFacingBillboard.Axis.back:
        return Vector3.back;
      default:
        return Vector3.up;
    }
  }

  private void Update() => ((Component) this).transform.LookAt(Vector3.op_Addition(((Component) this).transform.position, Quaternion.op_Multiply(((Component) this.referenceCamera).transform.rotation, !this.reverseFace ? Vector3.back : Vector3.forward)), Quaternion.op_Multiply(((Component) this.referenceCamera).transform.rotation, this.GetAxis(this.axis)));

  public enum Axis
  {
    up,
    down,
    left,
    right,
    forward,
    back,
  }
}
