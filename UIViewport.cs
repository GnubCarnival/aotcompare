// Decompiled with JetBrains decompiler
// Type: UIViewport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("NGUI/UI/Viewport Camera")]
[ExecuteInEditMode]
public class UIViewport : MonoBehaviour
{
  public Transform bottomRight;
  public float fullSize = 1f;
  private Camera mCam;
  public Camera sourceCamera;
  public Transform topLeft;

  private void LateUpdate()
  {
    if (!Object.op_Inequality((Object) this.topLeft, (Object) null) || !Object.op_Inequality((Object) this.bottomRight, (Object) null))
      return;
    Vector3 screenPoint1 = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
    Vector3 screenPoint2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(screenPoint1.x / (float) Screen.width, screenPoint2.y / (float) Screen.height, (screenPoint2.x - screenPoint1.x) / (float) Screen.width, (screenPoint1.y - screenPoint2.y) / (float) Screen.height);
    float num = this.fullSize * ((Rect) ref rect).height;
    if (Rect.op_Inequality(rect, this.mCam.rect))
      this.mCam.rect = rect;
    if ((double) this.mCam.orthographicSize == (double) num)
      return;
    this.mCam.orthographicSize = num;
  }

  private void Start()
  {
    this.mCam = ((Component) this).camera;
    if (!Object.op_Equality((Object) this.sourceCamera, (Object) null))
      return;
    this.sourceCamera = Camera.main;
  }
}
