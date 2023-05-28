// Decompiled with JetBrains decompiler
// Type: SpringPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : IgnoreTimeScale
{
  private UIDraggablePanel mDrag;
  private UIPanel mPanel;
  private float mThreshold;
  private Transform mTrans;
  public SpringPanel.OnFinished onFinished;
  public float strength = 10f;
  public Vector3 target = Vector3.zero;

  public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
  {
    SpringPanel springPanel = go.GetComponent<SpringPanel>();
    if (Object.op_Equality((Object) springPanel, (Object) null))
      springPanel = go.AddComponent<SpringPanel>();
    springPanel.target = pos;
    springPanel.strength = strength;
    springPanel.onFinished = (SpringPanel.OnFinished) null;
    if (!((Behaviour) springPanel).enabled)
    {
      springPanel.mThreshold = 0.0f;
      ((Behaviour) springPanel).enabled = true;
    }
    return springPanel;
  }

  private void Start()
  {
    this.mPanel = ((Component) this).GetComponent<UIPanel>();
    this.mDrag = ((Component) this).GetComponent<UIDraggablePanel>();
    this.mTrans = ((Component) this).transform;
  }

  private void Update()
  {
    float deltaTime = this.UpdateRealTimeDelta();
    if ((double) this.mThreshold == 0.0)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.target, this.mTrans.localPosition);
      this.mThreshold = ((Vector3) ref vector3).magnitude * 0.005f;
    }
    bool flag = false;
    Vector3 localPosition = this.mTrans.localPosition;
    Vector3 vector3_1 = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
    if ((double) this.mThreshold >= (double) Vector3.Magnitude(Vector3.op_Subtraction(vector3_1, this.target)))
    {
      vector3_1 = this.target;
      ((Behaviour) this).enabled = false;
      flag = true;
    }
    this.mTrans.localPosition = vector3_1;
    Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, localPosition);
    Vector4 clipRange = this.mPanel.clipRange;
    clipRange.x -= vector3_2.x;
    clipRange.y -= vector3_2.y;
    this.mPanel.clipRange = clipRange;
    if (Object.op_Inequality((Object) this.mDrag, (Object) null))
      this.mDrag.UpdateScrollbars(false);
    if (!flag || this.onFinished == null)
      return;
    this.onFinished();
  }

  public delegate void OnFinished();
}
