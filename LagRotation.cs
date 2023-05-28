// Decompiled with JetBrains decompiler
// Type: LagRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
  public bool ignoreTimeScale;
  private Quaternion mAbsolute;
  private Quaternion mRelative;
  private Transform mTrans;
  public float speed = 10f;
  public int updateOrder;

  private void CoroutineUpdate(float delta)
  {
    Transform parent = this.mTrans.parent;
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    this.mAbsolute = Quaternion.Slerp(this.mAbsolute, Quaternion.op_Multiply(parent.rotation, this.mRelative), delta * this.speed);
    this.mTrans.rotation = this.mAbsolute;
  }

  private void Start()
  {
    this.mTrans = ((Component) this).transform;
    this.mRelative = this.mTrans.localRotation;
    this.mAbsolute = this.mTrans.rotation;
    if (this.ignoreTimeScale)
      UpdateManager.AddCoroutine((MonoBehaviour) this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
    else
      UpdateManager.AddLateUpdate((MonoBehaviour) this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
  }
}
