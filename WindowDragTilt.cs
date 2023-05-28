// Decompiled with JetBrains decompiler
// Type: WindowDragTilt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
  public float degrees = 30f;
  private float mAngle;
  private bool mInit = true;
  private Vector3 mLastPos;
  private Transform mTrans;
  public int updateOrder;

  private void CoroutineUpdate(float delta)
  {
    if (this.mInit)
    {
      this.mInit = false;
      this.mTrans = ((Component) this).transform;
      this.mLastPos = this.mTrans.position;
    }
    Vector3 vector3 = Vector3.op_Subtraction(this.mTrans.position, this.mLastPos);
    this.mLastPos = this.mTrans.position;
    this.mAngle += vector3.x * this.degrees;
    this.mAngle = NGUIMath.SpringLerp(this.mAngle, 0.0f, 20f, delta);
    this.mTrans.localRotation = Quaternion.Euler(0.0f, 0.0f, -this.mAngle);
  }

  private void OnEnable() => this.mInit = true;

  private void Start() => UpdateManager.AddCoroutine((MonoBehaviour) this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
}
