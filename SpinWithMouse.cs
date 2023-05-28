// Decompiled with JetBrains decompiler
// Type: SpinWithMouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
  private Transform mTrans;
  public float speed = 1f;
  public Transform target;

  private void OnDrag(Vector2 delta)
  {
    UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
    if (Object.op_Inequality((Object) this.target, (Object) null))
      this.target.localRotation = Quaternion.op_Multiply(Quaternion.Euler(0.0f, -0.5f * delta.x * this.speed, 0.0f), this.target.localRotation);
    else
      this.mTrans.localRotation = Quaternion.op_Multiply(Quaternion.Euler(0.0f, -0.5f * delta.x * this.speed, 0.0f), this.mTrans.localRotation);
  }

  private void Start() => this.mTrans = ((Component) this).transform;
}
