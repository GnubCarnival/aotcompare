// Decompiled with JetBrains decompiler
// Type: SpringPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : IgnoreTimeScale
{
  public string callWhenFinished;
  public GameObject eventReceiver;
  public bool ignoreTimeScale;
  private float mThreshold;
  private Transform mTrans;
  public SpringPosition.OnFinished onFinished;
  public float strength = 10f;
  public Vector3 target = Vector3.zero;
  public bool worldSpace;

  public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
  {
    SpringPosition springPosition = go.GetComponent<SpringPosition>();
    if (Object.op_Equality((Object) springPosition, (Object) null))
      springPosition = go.AddComponent<SpringPosition>();
    springPosition.target = pos;
    springPosition.strength = strength;
    springPosition.onFinished = (SpringPosition.OnFinished) null;
    if (!((Behaviour) springPosition).enabled)
    {
      springPosition.mThreshold = 0.0f;
      ((Behaviour) springPosition).enabled = true;
    }
    return springPosition;
  }

  private void Start() => this.mTrans = ((Component) this).transform;

  private void Update()
  {
    float deltaTime = !this.ignoreTimeScale ? Time.deltaTime : this.UpdateRealTimeDelta();
    if (this.worldSpace)
    {
      if ((double) this.mThreshold == 0.0)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.target, this.mTrans.position);
        this.mThreshold = ((Vector3) ref vector3).magnitude * (1f / 1000f);
      }
      this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
      Vector3 vector3_1 = Vector3.op_Subtraction(this.target, this.mTrans.position);
      if ((double) this.mThreshold < (double) ((Vector3) ref vector3_1).magnitude)
        return;
      this.mTrans.position = this.target;
      if (this.onFinished != null)
        this.onFinished(this);
      if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.callWhenFinished))
        this.eventReceiver.SendMessage(this.callWhenFinished, (object) this, (SendMessageOptions) 1);
      ((Behaviour) this).enabled = false;
    }
    else
    {
      if ((double) this.mThreshold == 0.0)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.target, this.mTrans.localPosition);
        this.mThreshold = ((Vector3) ref vector3).magnitude * (1f / 1000f);
      }
      this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
      Vector3 vector3_2 = Vector3.op_Subtraction(this.target, this.mTrans.localPosition);
      if ((double) this.mThreshold < (double) ((Vector3) ref vector3_2).magnitude)
        return;
      this.mTrans.localPosition = this.target;
      if (this.onFinished != null)
        this.onFinished(this);
      if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.callWhenFinished))
        this.eventReceiver.SendMessage(this.callWhenFinished, (object) this, (SendMessageOptions) 1);
      ((Behaviour) this).enabled = false;
    }
  }

  public delegate void OnFinished(SpringPosition spring);
}
