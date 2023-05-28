// Decompiled with JetBrains decompiler
// Type: LookAtTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : MonoBehaviour
{
  public int level;
  private Transform mTrans;
  public float speed = 8f;
  public Transform target;

  private void LateUpdate()
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(this.target.position, this.mTrans.position);
    if ((double) ((Vector3) ref vector3).magnitude <= 1.0 / 1000.0)
      return;
    this.mTrans.rotation = Quaternion.Slerp(this.mTrans.rotation, Quaternion.LookRotation(vector3), Mathf.Clamp01(this.speed * Time.deltaTime));
  }

  private void Start() => this.mTrans = ((Component) this).transform;
}
