// Decompiled with JetBrains decompiler
// Type: Spin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
  private Rigidbody mRb;
  private Transform mTrans;
  public Vector3 rotationsPerSecond = new Vector3(0.0f, 0.1f, 0.0f);

  public void ApplyDelta(float delta)
  {
    delta *= 360f;
    Quaternion quaternion = Quaternion.Euler(Vector3.op_Multiply(this.rotationsPerSecond, delta));
    if (Object.op_Equality((Object) this.mRb, (Object) null))
    {
      Transform mTrans = this.mTrans;
      mTrans.rotation = Quaternion.op_Multiply(mTrans.rotation, quaternion);
    }
    else
      this.mRb.MoveRotation(Quaternion.op_Multiply(this.mRb.rotation, quaternion));
  }

  private void FixedUpdate()
  {
    if (!Object.op_Inequality((Object) this.mRb, (Object) null))
      return;
    this.ApplyDelta(Time.deltaTime);
  }

  private void Start()
  {
    this.mTrans = ((Component) this).transform;
    this.mRb = ((Component) this).rigidbody;
  }

  private void Update()
  {
    if (!Object.op_Equality((Object) this.mRb, (Object) null))
      return;
    this.ApplyDelta(Time.deltaTime);
  }
}
