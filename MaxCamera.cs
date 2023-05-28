// Decompiled with JetBrains decompiler
// Type: MaxCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class MaxCamera : MonoBehaviour
{
  private float currentDistance;
  private Quaternion currentRotation;
  private float desiredDistance;
  private Quaternion desiredRotation;
  public float distance = 5f;
  public float maxDistance = 20f;
  public float minDistance = 0.6f;
  public float panSpeed = 0.3f;
  private Vector3 position;
  private Quaternion rotation;
  public Transform target;
  public Vector3 targetOffset;
  private float xDeg;
  public float xSpeed = 200f;
  private float yDeg;
  public int yMaxLimit = 80;
  public int yMinLimit = -80;
  public float ySpeed = 200f;
  public float zoomDampening = 5f;
  public int zoomRate = 40;

  private static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }

  public void Init()
  {
    if (Object.op_Equality((Object) this.target, (Object) null))
      this.target = new GameObject("Cam Target")
      {
        transform = {
          position = Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, this.distance))
        }
      }.transform;
    this.distance = Vector3.Distance(((Component) this).transform.position, this.target.position);
    this.currentDistance = this.distance;
    this.desiredDistance = this.distance;
    this.position = ((Component) this).transform.position;
    this.rotation = ((Component) this).transform.rotation;
    this.currentRotation = ((Component) this).transform.rotation;
    this.desiredRotation = ((Component) this).transform.rotation;
    this.xDeg = Vector3.Angle(Vector3.right, ((Component) this).transform.right);
    this.yDeg = Vector3.Angle(Vector3.up, ((Component) this).transform.up);
  }

  private void LateUpdate()
  {
    if (Input.GetMouseButton(2) && Input.GetKey((KeyCode) 308) && Input.GetKey((KeyCode) 306))
      this.desiredDistance -= (float) ((double) Input.GetAxis("Mouse Y") * (double) Time.deltaTime * (double) this.zoomRate * 0.125) * Mathf.Abs(this.desiredDistance);
    else if (Input.GetMouseButton(0) && Input.GetKey((KeyCode) 308))
    {
      this.xDeg += (float) ((double) Input.GetAxis("Mouse X") * (double) this.xSpeed * 0.019999999552965164);
      this.yDeg -= (float) ((double) Input.GetAxis("Mouse Y") * (double) this.ySpeed * 0.019999999552965164);
      this.yDeg = MaxCamera.ClampAngle(this.yDeg, (float) this.yMinLimit, (float) this.yMaxLimit);
      this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0.0f);
      this.currentRotation = ((Component) this).transform.rotation;
      this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, Time.deltaTime * this.zoomDampening);
      ((Component) this).transform.rotation = this.rotation;
    }
    this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * (float) this.zoomRate * Mathf.Abs(this.desiredDistance);
    this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
    this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * this.zoomDampening);
    this.position = Vector3.op_Subtraction(this.target.position, Vector3.op_Addition(Vector3.op_Multiply(Quaternion.op_Multiply(this.rotation, Vector3.forward), this.currentDistance), this.targetOffset));
    ((Component) this).transform.position = this.position;
  }

  private void OnEnable() => this.Init();

  private void Start() => this.Init();
}
