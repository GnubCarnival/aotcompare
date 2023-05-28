// Decompiled with JetBrains decompiler
// Type: PanWithMouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : IgnoreTimeScale
{
  public Vector2 degrees = new Vector2(5f, 3f);
  private Vector2 mRot = Vector2.zero;
  private Quaternion mStart;
  private Transform mTrans;
  public float range = 1f;

  private void Start()
  {
    this.mTrans = ((Component) this).transform;
    this.mStart = this.mTrans.localRotation;
  }

  private void Update()
  {
    float num1 = this.UpdateRealTimeDelta();
    Vector3 mousePosition = Input.mousePosition;
    float num2 = (float) Screen.width * 0.5f;
    float num3 = (float) Screen.height * 0.5f;
    if ((double) this.range < 0.10000000149011612)
      this.range = 0.1f;
    this.mRot = Vector2.Lerp(this.mRot, new Vector2(Mathf.Clamp((mousePosition.x - num2) / num2 / this.range, -1f, 1f), Mathf.Clamp((mousePosition.y - num3) / num3 / this.range, -1f, 1f)), num1 * 5f);
    this.mTrans.localRotation = Quaternion.op_Multiply(this.mStart, Quaternion.Euler(-this.mRot.y * this.degrees.y, this.mRot.x * this.degrees.x, 0.0f));
  }
}
