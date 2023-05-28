// Decompiled with JetBrains decompiler
// Type: LagPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Lag Position")]
public class LagPosition : MonoBehaviour
{
  public bool ignoreTimeScale;
  private Vector3 mAbsolute;
  private Vector3 mRelative;
  private Transform mTrans;
  public Vector3 speed = new Vector3(10f, 10f, 10f);
  public int updateOrder;

  private void CoroutineUpdate(float delta)
  {
    Transform parent = this.mTrans.parent;
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Addition(parent.position, Quaternion.op_Multiply(parent.rotation, this.mRelative));
    this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector3.x, Mathf.Clamp01(delta * this.speed.x));
    this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector3.y, Mathf.Clamp01(delta * this.speed.y));
    this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector3.z, Mathf.Clamp01(delta * this.speed.z));
    this.mTrans.position = this.mAbsolute;
  }

  private void OnEnable()
  {
    this.mTrans = ((Component) this).transform;
    this.mAbsolute = this.mTrans.position;
  }

  private void Start()
  {
    this.mTrans = ((Component) this).transform;
    this.mRelative = this.mTrans.localPosition;
    if (this.ignoreTimeScale)
      UpdateManager.AddCoroutine((MonoBehaviour) this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
    else
      UpdateManager.AddLateUpdate((MonoBehaviour) this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
  }
}
