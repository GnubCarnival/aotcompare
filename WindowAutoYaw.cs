// Decompiled with JetBrains decompiler
// Type: WindowAutoYaw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : MonoBehaviour
{
  private Transform mTrans;
  public Camera uiCamera;
  public int updateOrder;
  public float yawAmount = 20f;

  private void CoroutineUpdate(float delta)
  {
    if (!Object.op_Inequality((Object) this.uiCamera, (Object) null))
      return;
    this.mTrans.localRotation = Quaternion.Euler(0.0f, (float) ((double) this.uiCamera.WorldToViewportPoint(this.mTrans.position).x * 2.0 - 1.0) * this.yawAmount, 0.0f);
  }

  private void OnDisable() => this.mTrans.localRotation = Quaternion.identity;

  private void Start()
  {
    if (Object.op_Equality((Object) this.uiCamera, (Object) null))
      this.uiCamera = NGUITools.FindCameraForLayer(((Component) this).gameObject.layer);
    this.mTrans = ((Component) this).transform;
    UpdateManager.AddCoroutine((MonoBehaviour) this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
  }
}
