// Decompiled with JetBrains decompiler
// Type: CameraShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CameraShake : MonoBehaviour
{
  private float decay;
  private float duration;
  private bool flip;
  private float R;

  private void FixedUpdate()
  {
  }

  private void shakeUpdate()
  {
    if ((double) this.duration <= 0.0)
      return;
    this.duration -= Time.deltaTime;
    if (this.flip)
    {
      Transform transform = ((Component) this).gameObject.transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.up, this.R));
    }
    else
    {
      Transform transform = ((Component) this).gameObject.transform;
      transform.position = Vector3.op_Subtraction(transform.position, Vector3.op_Multiply(Vector3.up, this.R));
    }
    this.flip = !this.flip;
    this.R *= this.decay;
  }

  private void Start()
  {
  }

  public void startShake(float R, float duration, float decay = 0.95f)
  {
    if ((double) this.duration >= (double) duration)
      return;
    this.R = R;
    this.duration = duration;
    this.decay = decay;
  }

  private void Update()
  {
  }
}
