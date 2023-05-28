// Decompiled with JetBrains decompiler
// Type: UIRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
  [HideInInspector]
  public bool automatic;
  public int manualHeight = 720;
  public int maximumHeight = 1536;
  public int minimumHeight = 320;
  private static List<UIRoot> mRoots = new List<UIRoot>();
  private Transform mTrans;
  public UIRoot.Scaling scalingStyle = UIRoot.Scaling.FixedSize;

  private void Awake()
  {
    this.mTrans = ((Component) this).transform;
    UIRoot.mRoots.Add(this);
    if (!this.automatic)
      return;
    this.scalingStyle = UIRoot.Scaling.PixelPerfect;
    this.automatic = false;
  }

  public static void Broadcast(string funcName)
  {
    int index = 0;
    for (int count = UIRoot.mRoots.Count; index < count; ++index)
    {
      UIRoot mRoot = UIRoot.mRoots[index];
      if (Object.op_Inequality((Object) mRoot, (Object) null))
        ((Component) mRoot).BroadcastMessage(funcName, (SendMessageOptions) 1);
    }
  }

  public static void Broadcast(string funcName, object param)
  {
    if (param == null)
    {
      Debug.LogError((object) "SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
    }
    else
    {
      int index = 0;
      for (int count = UIRoot.mRoots.Count; index < count; ++index)
      {
        UIRoot mRoot = UIRoot.mRoots[index];
        if (Object.op_Inequality((Object) mRoot, (Object) null))
          ((Component) mRoot).BroadcastMessage(funcName, param, (SendMessageOptions) 1);
      }
    }
  }

  public float GetPixelSizeAdjustment(int height)
  {
    height = Mathf.Max(2, height);
    if (this.scalingStyle == UIRoot.Scaling.FixedSize)
      return (float) this.manualHeight / (float) height;
    if (height < this.minimumHeight)
      return (float) this.minimumHeight / (float) height;
    return height > this.maximumHeight ? (float) this.maximumHeight / (float) height : 1f;
  }

  public static float GetPixelSizeAdjustment(GameObject go)
  {
    UIRoot inParents = NGUITools.FindInParents<UIRoot>(go);
    return !Object.op_Equality((Object) inParents, (Object) null) ? inParents.pixelSizeAdjustment : 1f;
  }

  private void OnDestroy() => UIRoot.mRoots.Remove(this);

  private void Start()
  {
    UIOrthoCamera componentInChildren = ((Component) this).GetComponentInChildren<UIOrthoCamera>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    Debug.LogWarning((object) "UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", (Object) componentInChildren);
    Camera component = ((Component) componentInChildren).gameObject.GetComponent<Camera>();
    ((Behaviour) componentInChildren).enabled = false;
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.orthographicSize = 1f;
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.mTrans, (Object) null))
      return;
    float activeHeight = (float) this.activeHeight;
    if ((double) activeHeight <= 0.0)
      return;
    float num = 2f / activeHeight;
    Vector3 localScale = this.mTrans.localScale;
    if ((double) Mathf.Abs(localScale.x - num) <= 1.4012984643248171E-45 && (double) Mathf.Abs(localScale.y - num) <= 1.4012984643248171E-45 && (double) Mathf.Abs(localScale.z - num) <= 1.4012984643248171E-45)
      return;
    this.mTrans.localScale = new Vector3(num, num, num);
  }

  public int activeHeight
  {
    get
    {
      int num = Mathf.Max(2, Screen.height);
      if (this.scalingStyle == UIRoot.Scaling.FixedSize)
        return this.manualHeight;
      if (num < this.minimumHeight)
        return this.minimumHeight;
      return num > this.maximumHeight ? this.maximumHeight : num;
    }
  }

  public static List<UIRoot> list => UIRoot.mRoots;

  public float pixelSizeAdjustment => this.GetPixelSizeAdjustment(Screen.height);

  public enum Scaling
  {
    PixelPerfect,
    FixedSize,
    FixedSizeOnMobiles,
  }
}
