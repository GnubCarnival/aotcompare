// Decompiled with JetBrains decompiler
// Type: CameraForLeftEye
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CameraForLeftEye : MonoBehaviour
{
  private Camera camera;
  private Camera cameraRightEye;
  public GameObject rightEye;

  private void LateUpdate()
  {
    this.camera.aspect = this.cameraRightEye.aspect;
    this.camera.fieldOfView = this.cameraRightEye.fieldOfView;
  }

  private void Start()
  {
    this.camera = ((Component) this).GetComponent<Camera>();
    this.cameraRightEye = this.rightEye.GetComponent<Camera>();
  }
}
