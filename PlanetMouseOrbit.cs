// Decompiled with JetBrains decompiler
// Type: PlanetMouseOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit")]
public class PlanetMouseOrbit : MonoBehaviour
{
  public float distance = 10f;
  public Transform target;
  private float x;
  public float xSpeed = 250f;
  private float y;
  public int yMaxLimit = 80;
  public int yMinLimit = -20;
  public float ySpeed = 120f;
  public int zoomRate = 25;

  public static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }

  public void Main()
  {
  }

  public void Start()
  {
    Vector3 eulerAngles = ((Component) this).transform.eulerAngles;
    this.x = eulerAngles.y;
    this.y = eulerAngles.x;
  }

  public void Update()
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    this.x += (float) ((double) Input.GetAxis("Mouse X") * (double) this.xSpeed * 0.019999999552965164);
    this.y -= (float) ((double) Input.GetAxis("Mouse Y") * (double) this.ySpeed * 0.019999999552965164);
    this.distance += (float) -((double) Input.GetAxis("Mouse ScrollWheel") * (double) Time.deltaTime) * (float) this.zoomRate * Mathf.Abs(this.distance);
    this.y = PlanetMouseOrbit.ClampAngle(this.y, (float) this.yMinLimit, (float) this.yMaxLimit);
    Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0.0f);
    Vector3 vector3 = Vector3.op_Addition(Quaternion.op_Multiply(quaternion, new Vector3(0.0f, 0.0f, -this.distance)), this.target.position);
    ((Component) this).transform.rotation = quaternion;
    ((Component) this).transform.position = vector3;
  }
}
