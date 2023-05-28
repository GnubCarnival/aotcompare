// Decompiled with JetBrains decompiler
// Type: BTN_rotate_character
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_rotate_character : MonoBehaviour
{
  public GameObject camera;
  private float distance = 3f;
  public GameObject hero;
  private bool isRotate;

  private void OnPress(bool press)
  {
    if (press)
      this.isRotate = true;
    else
      this.isRotate = false;
  }

  private void Update()
  {
    this.distance -= Input.GetAxis("Mouse ScrollWheel") * 0.05f;
    this.distance = Mathf.Clamp(this.distance, 0.8f, 3.5f);
    this.camera.transform.position = this.hero.transform.position;
    Transform transform1 = this.camera.transform;
    transform1.position = Vector3.op_Addition(transform1.position, Vector3.op_Multiply(Vector3.up, 1.1f));
    if (this.isRotate)
    {
      float num1 = Input.GetAxis("Mouse X") * 2.5f;
      float num2 = (float) (-(double) Input.GetAxis("Mouse Y") * 2.5);
      this.camera.transform.RotateAround(this.camera.transform.position, Vector3.up, num1);
      this.camera.transform.RotateAround(this.camera.transform.position, this.camera.transform.right, num2);
    }
    Transform transform2 = this.camera.transform;
    transform2.position = Vector3.op_Subtraction(transform2.position, Vector3.op_Multiply(this.camera.transform.forward, this.distance));
  }
}
