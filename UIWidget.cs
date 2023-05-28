// Decompiled with JetBrains decompiler
// Type: UIWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
  protected bool mChanged = true;
  [SerializeField]
  [HideInInspector]
  private Color mColor = Color.white;
  [SerializeField]
  [HideInInspector]
  private int mDepth;
  private Vector3 mDiffPos;
  private Quaternion mDiffRot;
  private Vector3 mDiffScale;
  private bool mForceVisible;
  private UIGeometry mGeom = new UIGeometry();
  protected GameObject mGo;
  private float mLastAlpha;
  private Matrix4x4 mLocalToPanel;
  [HideInInspector]
  [SerializeField]
  protected Material mMat;
  private Vector3 mOldV0;
  private Vector3 mOldV1;
  protected UIPanel mPanel;
  [HideInInspector]
  [SerializeField]
  private UIWidget.Pivot mPivot = UIWidget.Pivot.Center;
  protected bool mPlayMode = true;
  [SerializeField]
  [HideInInspector]
  protected Texture mTex;
  protected Transform mTrans;
  private bool mVisibleByPanel = true;

  protected virtual void Awake()
  {
    this.mGo = ((Component) this).gameObject;
    this.mPlayMode = Application.isPlaying;
  }

  public void CheckLayer()
  {
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null) || ((Component) this.mPanel).gameObject.layer == ((Component) this).gameObject.layer)
      return;
    Debug.LogWarning((object) "You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", (Object) this);
    ((Component) this).gameObject.layer = ((Component) this.mPanel).gameObject.layer;
  }

  [Obsolete("Use ParentHasChanged() instead")]
  public void CheckParent() => this.ParentHasChanged();

  public static int CompareFunc(UIWidget left, UIWidget right)
  {
    if (left.mDepth > right.mDepth)
      return 1;
    return left.mDepth < right.mDepth ? -1 : 0;
  }

  public void CreatePanel()
  {
    if (!Object.op_Equality((Object) this.mPanel, (Object) null) || !((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.material, (Object) null))
      return;
    this.mPanel = UIPanel.Find(this.cachedTransform);
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
      return;
    this.CheckLayer();
    this.mPanel.AddWidget(this);
    this.mChanged = true;
  }

  public virtual void MakePixelPerfect()
  {
    Vector3 localScale = this.cachedTransform.localScale;
    int num1 = Mathf.RoundToInt(localScale.x);
    int num2 = Mathf.RoundToInt(localScale.y);
    localScale.x = (float) num1;
    localScale.y = (float) num2;
    localScale.z = 1f;
    Vector3 localPosition = this.cachedTransform.localPosition;
    localPosition.z = (float) Mathf.RoundToInt(localPosition.z);
    localPosition.x = num1 % 2 != 1 || this.pivot != UIWidget.Pivot.Top && this.pivot != UIWidget.Pivot.Center && this.pivot != UIWidget.Pivot.Bottom ? Mathf.Round(localPosition.x) : Mathf.Floor(localPosition.x) + 0.5f;
    localPosition.y = num2 % 2 != 1 || this.pivot != UIWidget.Pivot.Left && this.pivot != UIWidget.Pivot.Center && this.pivot != UIWidget.Pivot.Right ? Mathf.Round(localPosition.y) : Mathf.Ceil(localPosition.y) - 0.5f;
    this.cachedTransform.localPosition = localPosition;
    this.cachedTransform.localScale = localScale;
  }

  public virtual void MarkAsChanged()
  {
    this.mChanged = true;
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null) || !((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || Application.isPlaying || !Object.op_Inequality((Object) this.material, (Object) null))
      return;
    this.mPanel.AddWidget(this);
    this.CheckLayer();
  }

  public void MarkAsChangedLite() => this.mChanged = true;

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
      return;
    this.mPanel.RemoveWidget(this);
    this.mPanel = (UIPanel) null;
  }

  private void OnDisable()
  {
    if (!this.keepMaterial)
      this.material = (Material) null;
    else if (Object.op_Inequality((Object) this.mPanel, (Object) null))
      this.mPanel.RemoveWidget(this);
    this.mPanel = (UIPanel) null;
  }

  protected virtual void OnEnable()
  {
    this.mChanged = true;
    if (!this.keepMaterial)
    {
      this.mMat = (Material) null;
      this.mTex = (Texture) null;
    }
    this.mPanel = (UIPanel) null;
  }

  public virtual void OnFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
  }

  protected virtual void OnStart()
  {
  }

  public void ParentHasChanged()
  {
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null) || !Object.op_Inequality((Object) this.mPanel, (Object) UIPanel.Find(this.cachedTransform)))
      return;
    this.mPanel.RemoveWidget(this);
    if (!this.keepMaterial || Application.isPlaying)
      this.material = (Material) null;
    this.mPanel = (UIPanel) null;
    this.CreatePanel();
  }

  public static BetterList<UIWidget> Raycast(GameObject root, Vector2 mousePos)
  {
    BetterList<UIWidget> betterList = new BetterList<UIWidget>();
    UICamera cameraForLayer = UICamera.FindCameraForLayer(root.layer);
    if (Object.op_Inequality((Object) cameraForLayer, (Object) null))
    {
      Camera cachedCamera = cameraForLayer.cachedCamera;
      foreach (UIWidget componentsInChild in root.GetComponentsInChildren<UIWidget>())
      {
        if ((double) NGUIMath.DistanceToRectangle(NGUIMath.CalculateWidgetCorners(componentsInChild), mousePos, cachedCamera) == 0.0)
          betterList.Add(componentsInChild);
      }
      // ISSUE: reference to a compiler-generated field
      if (UIWidget.famScache14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UIWidget.famScache14 = (Comparison<UIWidget>) ((w1, w2) => w2.mDepth.CompareTo(w1.mDepth));
      }
      // ISSUE: reference to a compiler-generated field
      betterList.Sort(UIWidget.famScache14);
    }
    return betterList;
  }

  private void Start()
  {
    this.OnStart();
    this.CreatePanel();
  }

  public virtual void Update()
  {
    if (!Object.op_Equality((Object) this.mPanel, (Object) null))
      return;
    this.CreatePanel();
  }

  public bool UpdateGeometry(UIPanel p, bool forceVisible)
  {
    if (Object.op_Inequality((Object) this.material, (Object) null) && Object.op_Inequality((Object) p, (Object) null))
    {
      this.mPanel = p;
      bool flag1 = false;
      float finalAlpha = this.finalAlpha;
      bool flag2 = (double) finalAlpha > 1.0 / 1000.0;
      bool flag3 = forceVisible || this.mVisibleByPanel;
      if (this.cachedTransform.hasChanged)
      {
        this.mTrans.hasChanged = false;
        if (!this.mPanel.widgetsAreStatic)
        {
          Vector2 relativeSize = this.relativeSize;
          Vector2 pivotOffset = this.pivotOffset;
          Vector4 relativePadding = this.relativePadding;
          float num1 = pivotOffset.x * relativeSize.x - relativePadding.x;
          float num2 = pivotOffset.y * relativeSize.y + relativePadding.y;
          float num3 = num1 + relativeSize.x + relativePadding.x + relativePadding.z;
          float num4 = num2 - relativeSize.y - relativePadding.y - relativePadding.w;
          this.mLocalToPanel = Matrix4x4.op_Multiply(p.worldToLocal, this.cachedTransform.localToWorldMatrix);
          flag1 = true;
          Vector3 vector3_1;
          // ISSUE: explicit constructor call
          ((Vector3) ref vector3_1).\u002Ector(num1, num2, 0.0f);
          Vector3 vector3_2;
          // ISSUE: explicit constructor call
          ((Vector3) ref vector3_2).\u002Ector(num3, num4, 0.0f);
          Vector3 vector3_3 = ((Matrix4x4) ref this.mLocalToPanel).MultiplyPoint3x4(vector3_1);
          Vector3 vector3_4 = ((Matrix4x4) ref this.mLocalToPanel).MultiplyPoint3x4(vector3_2);
          if ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(this.mOldV0, vector3_3)) > 9.9999999747524271E-07 || (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(this.mOldV1, vector3_4)) > 9.9999999747524271E-07)
          {
            this.mChanged = true;
            this.mOldV0 = vector3_3;
            this.mOldV1 = vector3_4;
          }
        }
        if (flag2 || this.mForceVisible != forceVisible)
        {
          this.mForceVisible = forceVisible;
          flag3 = forceVisible || this.mPanel.IsVisible(this);
        }
      }
      else if (flag2 && this.mForceVisible != forceVisible)
      {
        this.mForceVisible = forceVisible;
        flag3 = this.mPanel.IsVisible(this);
      }
      if (this.mVisibleByPanel != flag3)
      {
        this.mVisibleByPanel = flag3;
        this.mChanged = true;
      }
      if (this.mVisibleByPanel && (double) this.mLastAlpha != (double) finalAlpha)
        this.mChanged = true;
      this.mLastAlpha = finalAlpha;
      if (this.mChanged)
      {
        this.mChanged = false;
        if (this.isVisible)
        {
          this.mGeom.Clear();
          this.OnFill(this.mGeom.verts, this.mGeom.uvs, this.mGeom.cols);
          if (this.mGeom.hasVertices)
          {
            Vector3 pivotOffset = Vector2.op_Implicit(this.pivotOffset);
            Vector2 relativeSize = this.relativeSize;
            pivotOffset.x *= relativeSize.x;
            pivotOffset.y *= relativeSize.y;
            if (!flag1)
              this.mLocalToPanel = Matrix4x4.op_Multiply(p.worldToLocal, this.cachedTransform.localToWorldMatrix);
            this.mGeom.ApplyOffset(pivotOffset);
            this.mGeom.ApplyTransform(this.mLocalToPanel, p.generateNormals);
          }
          return true;
        }
        if (this.mGeom.hasVertices)
        {
          this.mGeom.Clear();
          return true;
        }
      }
    }
    return false;
  }

  public void WriteToBuffers(
    BetterList<Vector3> v,
    BetterList<Vector2> u,
    BetterList<Color32> c,
    BetterList<Vector3> n,
    BetterList<Vector4> t)
  {
    this.mGeom.WriteToBuffers(v, u, c, n, t);
  }

  public float alpha
  {
    get => this.mColor.a;
    set
    {
      Color mColor = this.mColor;
      mColor.a = value;
      this.color = mColor;
    }
  }

  public virtual Vector4 border => Vector4.zero;

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

  public Color color
  {
    get => this.mColor;
    set
    {
      if (this.mColor.Equals((object) value))
        return;
      this.mColor = value;
      this.mChanged = true;
    }
  }

  public int depth
  {
    get => this.mDepth;
    set
    {
      if (this.mDepth == value)
        return;
      this.mDepth = value;
      if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
        return;
      this.mPanel.MarkMaterialAsChanged(this.material, true);
    }
  }

  public float finalAlpha
  {
    get
    {
      if (Object.op_Equality((Object) this.mPanel, (Object) null))
        this.CreatePanel();
      return !Object.op_Equality((Object) this.mPanel, (Object) null) ? this.mColor.a * this.mPanel.alpha : this.mColor.a;
    }
  }

  public bool isVisible => this.mVisibleByPanel && (double) this.finalAlpha > 1.0 / 1000.0;

  public virtual bool keepMaterial => false;

  public virtual Texture mainTexture
  {
    get
    {
      Material material = this.material;
      if (Object.op_Inequality((Object) material, (Object) null))
      {
        if (Object.op_Inequality((Object) material.mainTexture, (Object) null))
          this.mTex = material.mainTexture;
        else if (Object.op_Inequality((Object) this.mTex, (Object) null))
        {
          if (Object.op_Inequality((Object) this.mPanel, (Object) null))
            this.mPanel.RemoveWidget(this);
          this.mPanel = (UIPanel) null;
          this.mMat.mainTexture = this.mTex;
          if (((Behaviour) this).enabled)
            this.CreatePanel();
        }
      }
      return this.mTex;
    }
    set
    {
      Material material1 = this.material;
      if (!Object.op_Equality((Object) material1, (Object) null) && !Object.op_Inequality((Object) material1.mainTexture, (Object) value))
        return;
      if (Object.op_Inequality((Object) this.mPanel, (Object) null))
        this.mPanel.RemoveWidget(this);
      this.mPanel = (UIPanel) null;
      this.mTex = value;
      Material material2 = this.material;
      if (!Object.op_Inequality((Object) material2, (Object) null))
        return;
      material2.mainTexture = value;
      if (!((Behaviour) this).enabled)
        return;
      this.CreatePanel();
    }
  }

  public virtual Material material
  {
    get => this.mMat;
    set
    {
      if (!Object.op_Inequality((Object) this.mMat, (Object) value))
        return;
      if (Object.op_Inequality((Object) this.mMat, (Object) null) && Object.op_Inequality((Object) this.mPanel, (Object) null))
        this.mPanel.RemoveWidget(this);
      this.mPanel = (UIPanel) null;
      this.mMat = value;
      this.mTex = (Texture) null;
      if (!Object.op_Inequality((Object) this.mMat, (Object) null))
        return;
      this.CreatePanel();
    }
  }

  public UIPanel panel
  {
    get
    {
      if (Object.op_Equality((Object) this.mPanel, (Object) null))
        this.CreatePanel();
      return this.mPanel;
    }
    set => this.mPanel = value;
  }

  public UIWidget.Pivot pivot
  {
    get => this.mPivot;
    set
    {
      if (this.mPivot == value)
        return;
      Vector3 widgetCorner1 = NGUIMath.CalculateWidgetCorners(this)[0];
      this.mPivot = value;
      this.mChanged = true;
      Vector3 widgetCorner2 = NGUIMath.CalculateWidgetCorners(this)[0];
      Transform cachedTransform = this.cachedTransform;
      Vector3 position = cachedTransform.position;
      float z = cachedTransform.localPosition.z;
      position.x += widgetCorner1.x - widgetCorner2.x;
      position.y += widgetCorner1.y - widgetCorner2.y;
      this.cachedTransform.position = position;
      Vector3 localPosition = this.cachedTransform.localPosition;
      localPosition.x = Mathf.Round(localPosition.x);
      localPosition.y = Mathf.Round(localPosition.y);
      localPosition.z = z;
      this.cachedTransform.localPosition = localPosition;
    }
  }

  public Vector2 pivotOffset
  {
    get
    {
      Vector2 zero = Vector2.zero;
      Vector4 relativePadding = this.relativePadding;
      UIWidget.Pivot pivot = this.pivot;
      switch (pivot)
      {
        case UIWidget.Pivot.Top:
        case UIWidget.Pivot.Center:
        case UIWidget.Pivot.Bottom:
          zero.x = (float) (((double) relativePadding.x - (double) relativePadding.z - 1.0) * 0.5);
          break;
        case UIWidget.Pivot.TopRight:
        case UIWidget.Pivot.Right:
        case UIWidget.Pivot.BottomRight:
          zero.x = -1f - relativePadding.z;
          break;
        default:
          zero.x = relativePadding.x;
          break;
      }
      switch (pivot)
      {
        case UIWidget.Pivot.Left:
        case UIWidget.Pivot.Center:
        case UIWidget.Pivot.Right:
          zero.y = (float) (((double) relativePadding.w - (double) relativePadding.y + 1.0) * 0.5);
          return zero;
        case UIWidget.Pivot.BottomLeft:
        case UIWidget.Pivot.Bottom:
        case UIWidget.Pivot.BottomRight:
          zero.y = 1f + relativePadding.w;
          return zero;
        default:
          zero.y = -relativePadding.y;
          return zero;
      }
    }
  }

  public virtual bool pixelPerfectAfterResize => false;

  public virtual Vector4 relativePadding => Vector4.zero;

  public virtual Vector2 relativeSize => Vector2.one;

  public enum Pivot
  {
    TopLeft,
    Top,
    TopRight,
    Left,
    Center,
    Right,
    BottomLeft,
    Bottom,
    BottomRight,
  }
}
