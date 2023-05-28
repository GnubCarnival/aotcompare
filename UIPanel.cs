// Decompiled with JetBrains decompiler
// Type: UIPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Panel")]
public class UIPanel : MonoBehaviour
{
  public bool cullWhileDragging;
  public bool depthPass;
  public bool generateNormals;
  [SerializeField]
  [HideInInspector]
  private float mAlpha = 1f;
  private Camera mCam;
  private BetterList<Material> mChanged = new BetterList<Material>();
  private UIPanel[] mChildPanels;
  [SerializeField]
  [HideInInspector]
  private UIDrawCall.Clipping mClipping;
  [HideInInspector]
  [SerializeField]
  private Vector4 mClipRange = Vector4.zero;
  [HideInInspector]
  [SerializeField]
  private Vector2 mClipSoftness = new Vector2(40f, 40f);
  private BetterList<Color32> mCols = new BetterList<Color32>();
  private float mCullTime;
  [SerializeField]
  [HideInInspector]
  private UIPanel.DebugInfo mDebugInfo = UIPanel.DebugInfo.Gizmos;
  private bool mDepthChanged;
  private BetterList<UIDrawCall> mDrawCalls = new BetterList<UIDrawCall>();
  private GameObject mGo;
  private int mLayer = -1;
  private float mMatrixTime;
  private Vector2 mMax = Vector2.zero;
  private Vector2 mMin = Vector2.zero;
  private BetterList<Vector3> mNorms = new BetterList<Vector3>();
  private BetterList<Vector4> mTans = new BetterList<Vector4>();
  private static float[] mTemp = new float[4];
  private Transform mTrans;
  private float mUpdateTime;
  private BetterList<Vector2> mUvs = new BetterList<Vector2>();
  private BetterList<Vector3> mVerts = new BetterList<Vector3>();
  private BetterList<UIWidget> mWidgets = new BetterList<UIWidget>();
  public UIPanel.OnChangeDelegate onChange;
  public bool showInPanelTool = true;
  public bool widgetsAreStatic;
  [HideInInspector]
  public Matrix4x4 worldToLocal = Matrix4x4.identity;

  public void AddWidget(UIWidget w)
  {
    if (!Object.op_Inequality((Object) w, (Object) null) || this.mWidgets.Contains(w))
      return;
    this.mWidgets.Add(w);
    if (!this.mChanged.Contains(w.material))
      this.mChanged.Add(w.material);
    this.mDepthChanged = true;
  }

  private void Awake()
  {
    this.mGo = ((Component) this).gameObject;
    this.mTrans = ((Component) this).transform;
  }

  public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
  {
    float num1 = this.clipRange.z * 0.5f;
    float num2 = this.clipRange.w * 0.5f;
    Vector2 minRect;
    // ISSUE: explicit constructor call
    ((Vector2) ref minRect).\u002Ector(min.x, min.y);
    Vector2 maxRect;
    // ISSUE: explicit constructor call
    ((Vector2) ref maxRect).\u002Ector(max.x, max.y);
    Vector2 minArea;
    // ISSUE: explicit constructor call
    ((Vector2) ref minArea).\u002Ector(this.clipRange.x - num1, this.clipRange.y - num2);
    Vector2 maxArea;
    // ISSUE: explicit constructor call
    ((Vector2) ref maxArea).\u002Ector(this.clipRange.x + num1, this.clipRange.y + num2);
    if (this.clipping == UIDrawCall.Clipping.SoftClip)
    {
      minArea.x += this.clipSoftness.x;
      minArea.y += this.clipSoftness.y;
      maxArea.x -= this.clipSoftness.x;
      maxArea.y -= this.clipSoftness.y;
    }
    return Vector2.op_Implicit(NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea));
  }

  public bool ConstrainTargetToBounds(Transform target, bool immediate)
  {
    Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(this.cachedTransform, target);
    return this.ConstrainTargetToBounds(target, ref relativeWidgetBounds, immediate);
  }

  public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
  {
    Vector3 constrainOffset = this.CalculateConstrainOffset(Vector2.op_Implicit(((Bounds) ref targetBounds).min), Vector2.op_Implicit(((Bounds) ref targetBounds).max));
    if ((double) ((Vector3) ref constrainOffset).magnitude <= 0.0)
      return false;
    if (immediate)
    {
      Transform transform = target;
      transform.localPosition = Vector3.op_Addition(transform.localPosition, constrainOffset);
      ref Bounds local = ref targetBounds;
      ((Bounds) ref local).center = Vector3.op_Addition(((Bounds) ref local).center, constrainOffset);
      SpringPosition component = ((Component) target).GetComponent<SpringPosition>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = false;
    }
    else
    {
      SpringPosition springPosition = SpringPosition.Begin(((Component) target).gameObject, Vector3.op_Addition(target.localPosition, constrainOffset), 13f);
      springPosition.ignoreTimeScale = true;
      springPosition.worldSpace = false;
    }
    return true;
  }

  private void Fill(Material mat)
  {
    int index = 0;
    while (index < this.mWidgets.size)
    {
      UIWidget uiWidget = this.mWidgets.buffer[index];
      if (Object.op_Equality((Object) uiWidget, (Object) null))
      {
        this.mWidgets.RemoveAt(index);
      }
      else
      {
        if (Object.op_Equality((Object) uiWidget.material, (Object) mat) && uiWidget.isVisible)
        {
          if (Object.op_Equality((Object) uiWidget.panel, (Object) this))
          {
            if (this.generateNormals)
              uiWidget.WriteToBuffers(this.mVerts, this.mUvs, this.mCols, this.mNorms, this.mTans);
            else
              uiWidget.WriteToBuffers(this.mVerts, this.mUvs, this.mCols, (BetterList<Vector3>) null, (BetterList<Vector4>) null);
          }
          else
          {
            this.mWidgets.RemoveAt(index);
            continue;
          }
        }
        ++index;
      }
    }
    if (this.mVerts.size > 0)
    {
      UIDrawCall drawCall = this.GetDrawCall(mat, true);
      drawCall.depthPass = this.depthPass && this.mClipping == UIDrawCall.Clipping.None;
      drawCall.Set(this.mVerts, !this.generateNormals ? (BetterList<Vector3>) null : this.mNorms, !this.generateNormals ? (BetterList<Vector4>) null : this.mTans, this.mUvs, this.mCols);
    }
    else
    {
      UIDrawCall drawCall = this.GetDrawCall(mat, false);
      if (Object.op_Inequality((Object) drawCall, (Object) null))
      {
        this.mDrawCalls.Remove(drawCall);
        NGUITools.DestroyImmediate((Object) ((Component) drawCall).gameObject);
      }
    }
    this.mVerts.Clear();
    this.mNorms.Clear();
    this.mTans.Clear();
    this.mUvs.Clear();
    this.mCols.Clear();
  }

  public static UIPanel Find(Transform trans) => UIPanel.Find(trans, true);

  public static UIPanel Find(Transform trans, bool createIfMissing)
  {
    Transform transform = trans;
    UIPanel uiPanel;
    for (uiPanel = (UIPanel) null; Object.op_Equality((Object) uiPanel, (Object) null) && Object.op_Inequality((Object) trans, (Object) null); trans = trans.parent)
    {
      uiPanel = ((Component) trans).GetComponent<UIPanel>();
      if (Object.op_Inequality((Object) uiPanel, (Object) null) || Object.op_Equality((Object) trans.parent, (Object) null))
        break;
    }
    if (createIfMissing && Object.op_Equality((Object) uiPanel, (Object) null) && Object.op_Inequality((Object) trans, (Object) transform))
    {
      uiPanel = ((Component) trans).gameObject.AddComponent<UIPanel>();
      UIPanel.SetChildLayer(uiPanel.cachedTransform, uiPanel.cachedGameObject.layer);
    }
    return uiPanel;
  }

  private UIDrawCall GetDrawCall(Material mat, bool createIfMissing)
  {
    int index = 0;
    for (int size = this.drawCalls.size; index < size; ++index)
    {
      UIDrawCall drawCall = this.drawCalls.buffer[index];
      if (Object.op_Equality((Object) drawCall.material, (Object) mat))
        return drawCall;
    }
    UIDrawCall drawCall1 = (UIDrawCall) null;
    if (createIfMissing)
    {
      GameObject gameObject = new GameObject("_UIDrawCall [" + ((Object) mat).name + "]");
      Object.DontDestroyOnLoad((Object) gameObject);
      gameObject.layer = this.cachedGameObject.layer;
      drawCall1 = gameObject.AddComponent<UIDrawCall>();
      drawCall1.material = mat;
      this.mDrawCalls.Add(drawCall1);
    }
    return drawCall1;
  }

  public bool IsVisible(UIWidget w)
  {
    if ((double) this.mAlpha < 1.0 / 1000.0 || !((Behaviour) w).enabled || !NGUITools.GetActive(w.cachedGameObject) || (double) w.alpha < 1.0 / 1000.0)
      return false;
    if (this.mClipping == UIDrawCall.Clipping.None)
      return true;
    Vector2 relativeSize = w.relativeSize;
    Vector2 vector2_1 = Vector2.Scale(w.pivotOffset, relativeSize);
    Vector2 vector2_2 = vector2_1;
    vector2_1.x += relativeSize.x;
    vector2_1.y -= relativeSize.y;
    Transform cachedTransform = w.cachedTransform;
    return this.IsVisible(cachedTransform.TransformPoint(Vector2.op_Implicit(vector2_1)), cachedTransform.TransformPoint(Vector2.op_Implicit(new Vector2(vector2_1.x, vector2_2.y))), cachedTransform.TransformPoint(Vector2.op_Implicit(new Vector2(vector2_2.x, vector2_1.y))), cachedTransform.TransformPoint(Vector2.op_Implicit(vector2_2)));
  }

  public bool IsVisible(Vector3 worldPos)
  {
    if ((double) this.mAlpha < 1.0 / 1000.0)
      return false;
    if (this.mClipping != UIDrawCall.Clipping.None)
    {
      this.UpdateTransformMatrix();
      Vector3 vector3 = ((Matrix4x4) ref this.worldToLocal).MultiplyPoint3x4(worldPos);
      if ((double) vector3.x < (double) this.mMin.x || (double) vector3.y < (double) this.mMin.y || (double) vector3.x > (double) this.mMax.x || (double) vector3.y > (double) this.mMax.y)
        return false;
    }
    return true;
  }

  private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
  {
    this.UpdateTransformMatrix();
    a = ((Matrix4x4) ref this.worldToLocal).MultiplyPoint3x4(a);
    b = ((Matrix4x4) ref this.worldToLocal).MultiplyPoint3x4(b);
    c = ((Matrix4x4) ref this.worldToLocal).MultiplyPoint3x4(c);
    d = ((Matrix4x4) ref this.worldToLocal).MultiplyPoint3x4(d);
    UIPanel.mTemp[0] = a.x;
    UIPanel.mTemp[1] = b.x;
    UIPanel.mTemp[2] = c.x;
    UIPanel.mTemp[3] = d.x;
    float num1 = Mathf.Min(UIPanel.mTemp);
    float num2 = Mathf.Max(UIPanel.mTemp);
    UIPanel.mTemp[0] = a.y;
    UIPanel.mTemp[1] = b.y;
    UIPanel.mTemp[2] = c.y;
    UIPanel.mTemp[3] = d.y;
    float num3 = Mathf.Min(UIPanel.mTemp);
    float num4 = Mathf.Max(UIPanel.mTemp);
    return (double) num2 >= (double) this.mMin.x && (double) num4 >= (double) this.mMin.y && (double) num1 <= (double) this.mMax.x && (double) num3 <= (double) this.mMax.y;
  }

  private void LateUpdate()
  {
    this.mUpdateTime = Time.realtimeSinceStartup;
    this.UpdateTransformMatrix();
    if (this.mLayer != this.cachedGameObject.layer)
    {
      this.mLayer = this.mGo.layer;
      UICamera cameraForLayer = UICamera.FindCameraForLayer(this.mLayer);
      this.mCam = Object.op_Equality((Object) cameraForLayer, (Object) null) ? NGUITools.FindCameraForLayer(this.mLayer) : cameraForLayer.cachedCamera;
      UIPanel.SetChildLayer(this.cachedTransform, this.mLayer);
      int index = 0;
      for (int size = this.drawCalls.size; index < size; ++index)
        ((Component) this.mDrawCalls.buffer[index]).gameObject.layer = this.mLayer;
    }
    bool forceVisible = !this.cullWhileDragging && (this.clipping == UIDrawCall.Clipping.None || (double) this.mCullTime > (double) this.mUpdateTime);
    int i = 0;
    for (int size = this.mWidgets.size; i < size; ++i)
    {
      UIWidget mWidget = this.mWidgets[i];
      if (mWidget.UpdateGeometry(this, forceVisible) && !this.mChanged.Contains(mWidget.material))
        this.mChanged.Add(mWidget.material);
    }
    if (this.mChanged.size != 0 && this.onChange != null)
      this.onChange();
    if (this.mDepthChanged)
    {
      this.mDepthChanged = false;
      this.mWidgets.Sort(new Comparison<UIWidget>(UIWidget.CompareFunc));
    }
    int index1 = 0;
    for (int size = this.mChanged.size; index1 < size; ++index1)
      this.Fill(this.mChanged.buffer[index1]);
    this.UpdateDrawcalls();
    this.mChanged.Clear();
  }

  public void MarkMaterialAsChanged(Material mat, bool sort)
  {
    if (!Object.op_Inequality((Object) mat, (Object) null))
      return;
    if (sort)
      this.mDepthChanged = true;
    if (this.mChanged.Contains(mat))
      return;
    this.mChanged.Add(mat);
  }

  private void OnDisable()
  {
    int size = this.mDrawCalls.size;
    while (size > 0)
    {
      UIDrawCall uiDrawCall = this.mDrawCalls.buffer[--size];
      if (Object.op_Inequality((Object) uiDrawCall, (Object) null))
        NGUITools.DestroyImmediate((Object) ((Component) uiDrawCall).gameObject);
    }
    this.mDrawCalls.Clear();
    this.mChanged.Clear();
  }

  private void OnEnable()
  {
    int index = 0;
    while (index < this.mWidgets.size)
    {
      UIWidget uiWidget = this.mWidgets.buffer[index];
      if (Object.op_Inequality((Object) uiWidget, (Object) null))
      {
        this.MarkMaterialAsChanged(uiWidget.material, true);
        ++index;
      }
      else
        this.mWidgets.RemoveAt(index);
    }
  }

  public void Refresh()
  {
    UIWidget[] componentsInChildren = ((Component) this).GetComponentsInChildren<UIWidget>();
    int index = 0;
    for (int length = componentsInChildren.Length; index < length; ++index)
      componentsInChildren[index].Update();
    this.LateUpdate();
  }

  public void RemoveWidget(UIWidget w)
  {
    if (!Object.op_Inequality((Object) w, (Object) null) || !Object.op_Inequality((Object) w, (Object) null) || !this.mWidgets.Remove(w) || !Object.op_Inequality((Object) w.material, (Object) null))
      return;
    this.mChanged.Add(w.material);
  }

  public void SetAlphaRecursive(float val, bool rebuildList)
  {
    if (rebuildList || this.mChildPanels == null)
      this.mChildPanels = ((Component) this).GetComponentsInChildren<UIPanel>(true);
    int index = 0;
    for (int length = this.mChildPanels.Length; index < length; ++index)
      this.mChildPanels[index].alpha = val;
  }

  private static void SetChildLayer(Transform t, int layer)
  {
    for (int index = 0; index < t.childCount; ++index)
    {
      Transform child = t.GetChild(index);
      if (Object.op_Equality((Object) ((Component) child).GetComponent<UIPanel>(), (Object) null))
      {
        if (Object.op_Inequality((Object) ((Component) child).GetComponent<UIWidget>(), (Object) null))
          ((Component) child).gameObject.layer = layer;
        UIPanel.SetChildLayer(child, layer);
      }
    }
  }

  private void Start()
  {
    this.mLayer = this.mGo.layer;
    UICamera cameraForLayer = UICamera.FindCameraForLayer(this.mLayer);
    this.mCam = Object.op_Equality((Object) cameraForLayer, (Object) null) ? NGUITools.FindCameraForLayer(this.mLayer) : cameraForLayer.cachedCamera;
  }

  public void UpdateDrawcalls()
  {
    Vector4 zero = Vector4.zero;
    if (this.mClipping != UIDrawCall.Clipping.None)
    {
      // ISSUE: explicit constructor call
      ((Vector4) ref zero).\u002Ector(this.mClipRange.x, this.mClipRange.y, this.mClipRange.z * 0.5f, this.mClipRange.w * 0.5f);
    }
    if ((double) zero.z == 0.0)
      zero.z = (float) Screen.width * 0.5f;
    if ((double) zero.w == 0.0)
      zero.w = (float) Screen.height * 0.5f;
    RuntimePlatform platform = Application.platform;
    if (platform == 2 || platform == 5 || platform == 7)
    {
      zero.x -= 0.5f;
      zero.y += 0.5f;
    }
    Transform cachedTransform = this.cachedTransform;
    int index = 0;
    for (int size = this.mDrawCalls.size; index < size; ++index)
    {
      UIDrawCall uiDrawCall = this.mDrawCalls.buffer[index];
      uiDrawCall.clipping = this.mClipping;
      uiDrawCall.clipRange = zero;
      uiDrawCall.clipSoftness = this.mClipSoftness;
      uiDrawCall.depthPass = this.depthPass && this.mClipping == UIDrawCall.Clipping.None;
      Transform transform = ((Component) uiDrawCall).transform;
      transform.position = cachedTransform.position;
      transform.rotation = cachedTransform.rotation;
      transform.localScale = cachedTransform.lossyScale;
    }
  }

  private void UpdateTransformMatrix()
  {
    if ((double) this.mUpdateTime != 0.0 && (double) this.mMatrixTime == (double) this.mUpdateTime)
      return;
    this.mMatrixTime = this.mUpdateTime;
    this.worldToLocal = this.cachedTransform.worldToLocalMatrix;
    if (this.mClipping == UIDrawCall.Clipping.None)
      return;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(this.mClipRange.z, this.mClipRange.w);
    if ((double) vector2_1.x == 0.0)
      vector2_1.x = Object.op_Inequality((Object) this.mCam, (Object) null) ? this.mCam.pixelWidth : (float) Screen.width;
    if ((double) vector2_1.y == 0.0)
      vector2_1.y = Object.op_Inequality((Object) this.mCam, (Object) null) ? this.mCam.pixelHeight : (float) Screen.height;
    Vector2 vector2_2 = Vector2.op_Multiply(vector2_1, 0.5f);
    this.mMin.x = this.mClipRange.x - vector2_2.x;
    this.mMin.y = this.mClipRange.y - vector2_2.y;
    this.mMax.x = this.mClipRange.x + vector2_2.x;
    this.mMax.y = this.mClipRange.y + vector2_2.y;
  }

  public float alpha
  {
    get => this.mAlpha;
    set
    {
      float num = Mathf.Clamp01(value);
      if ((double) this.mAlpha == (double) num)
        return;
      this.mAlpha = num;
      for (int i = 0; i < this.mDrawCalls.size; ++i)
        this.MarkMaterialAsChanged(this.mDrawCalls[i].material, false);
      for (int i = 0; i < this.mWidgets.size; ++i)
        this.mWidgets[i].MarkAsChangedLite();
    }
  }

  public GameObject cachedGameObject
  {
    get
    {
      if (Object.op_Equality((Object) this.mGo, (Object) null))
        this.mGo = ((Component) this).gameObject;
      return this.mGo;
    }
  }

  public Transform cachedTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.mTrans, (Object) null))
        this.mTrans = ((Component) this).transform;
      return this.mTrans;
    }
  }

  public UIDrawCall.Clipping clipping
  {
    get => this.mClipping;
    set
    {
      if (this.mClipping == value)
        return;
      this.mClipping = value;
      this.mMatrixTime = 0.0f;
      this.UpdateDrawcalls();
    }
  }

  public Vector4 clipRange
  {
    get => this.mClipRange;
    set
    {
      if (!Vector4.op_Inequality(this.mClipRange, value))
        return;
      this.mCullTime = (double) this.mCullTime != 0.0 ? Time.realtimeSinceStartup + 0.15f : 1f / 1000f;
      this.mClipRange = value;
      this.mMatrixTime = 0.0f;
      this.UpdateDrawcalls();
    }
  }

  public Vector2 clipSoftness
  {
    get => this.mClipSoftness;
    set
    {
      if (!Vector2.op_Inequality(this.mClipSoftness, value))
        return;
      this.mClipSoftness = value;
      this.UpdateDrawcalls();
    }
  }

  public UIPanel.DebugInfo debugInfo
  {
    get => this.mDebugInfo;
    set
    {
      if (this.mDebugInfo == value)
        return;
      this.mDebugInfo = value;
      BetterList<UIDrawCall> drawCalls = this.drawCalls;
      HideFlags hideFlags = this.mDebugInfo != UIPanel.DebugInfo.Geometry ? (HideFlags) 13 : (HideFlags) 12;
      int i = 0;
      for (int size = drawCalls.size; i < size; ++i)
      {
        GameObject gameObject = ((Component) drawCalls[i]).gameObject;
        NGUITools.SetActiveSelf(gameObject, false);
        ((Object) gameObject).hideFlags = hideFlags;
        NGUITools.SetActiveSelf(gameObject, true);
      }
    }
  }

  public BetterList<UIDrawCall> drawCalls
  {
    get
    {
      int size = this.mDrawCalls.size;
      while (size > 0)
      {
        if (Object.op_Equality((Object) this.mDrawCalls[--size], (Object) null))
          this.mDrawCalls.RemoveAt(size);
      }
      return this.mDrawCalls;
    }
  }

  public BetterList<UIWidget> widgets => this.mWidgets;

  public enum DebugInfo
  {
    None,
    Gizmos,
    Geometry,
  }

  public delegate void OnChangeDelegate();
}
