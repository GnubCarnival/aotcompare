// Decompiled with JetBrains decompiler
// Type: UIScrollBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : MonoBehaviour
{
  [HideInInspector]
  [SerializeField]
  private UISprite mBG;
  private Camera mCam;
  [HideInInspector]
  [SerializeField]
  private UIScrollBar.Direction mDir;
  [HideInInspector]
  [SerializeField]
  private UISprite mFG;
  [SerializeField]
  [HideInInspector]
  private bool mInverted;
  private bool mIsDirty;
  private Vector2 mScreenPos = Vector2.zero;
  [SerializeField]
  [HideInInspector]
  private float mScroll;
  [HideInInspector]
  [SerializeField]
  private float mSize = 1f;
  private Transform mTrans;
  public UIScrollBar.OnScrollBarChange onChange;
  public UIScrollBar.OnDragFinished onDragFinished;

  private void CenterOnPos(Vector2 localPos)
  {
    if (!Object.op_Inequality((Object) this.mBG, (Object) null) || !Object.op_Inequality((Object) this.mFG, (Object) null))
      return;
    Bounds relativeInnerBounds1 = NGUIMath.CalculateRelativeInnerBounds(this.cachedTransform, this.mBG);
    Bounds relativeInnerBounds2 = NGUIMath.CalculateRelativeInnerBounds(this.cachedTransform, this.mFG);
    if (this.mDir == UIScrollBar.Direction.Horizontal)
    {
      float num1 = ((Bounds) ref relativeInnerBounds1).size.x - ((Bounds) ref relativeInnerBounds2).size.x;
      float num2 = num1 * 0.5f;
      float num3 = ((Bounds) ref relativeInnerBounds1).center.x - num2;
      float num4 = (double) num1 <= 0.0 ? 0.0f : (localPos.x - num3) / num1;
      this.scrollValue = !this.mInverted ? num4 : 1f - num4;
    }
    else
    {
      float num5 = ((Bounds) ref relativeInnerBounds1).size.y - ((Bounds) ref relativeInnerBounds2).size.y;
      float num6 = num5 * 0.5f;
      float num7 = ((Bounds) ref relativeInnerBounds1).center.y - num6;
      float num8 = (double) num5 <= 0.0 ? 0.0f : (float) (1.0 - ((double) localPos.y - (double) num7) / (double) num5);
      this.scrollValue = !this.mInverted ? num8 : 1f - num8;
    }
  }

  public void ForceUpdate()
  {
    this.mIsDirty = false;
    if (!Object.op_Inequality((Object) this.mBG, (Object) null) || !Object.op_Inequality((Object) this.mFG, (Object) null))
      return;
    this.mSize = Mathf.Clamp01(this.mSize);
    this.mScroll = Mathf.Clamp01(this.mScroll);
    Vector4 border1 = this.mBG.border;
    Vector4 border2 = this.mFG.border;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(Mathf.Max(0.0f, this.mBG.cachedTransform.localScale.x - border1.x - border1.z), Mathf.Max(0.0f, this.mBG.cachedTransform.localScale.y - border1.y - border1.w));
    float num = !this.mInverted ? this.mScroll : 1f - this.mScroll;
    if (this.mDir == UIScrollBar.Direction.Horizontal)
    {
      Vector2 vector2_2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_2).\u002Ector(vector2_1.x * this.mSize, vector2_1.y);
      this.mFG.pivot = UIWidget.Pivot.Left;
      this.mBG.pivot = UIWidget.Pivot.Left;
      this.mBG.cachedTransform.localPosition = Vector3.zero;
      this.mFG.cachedTransform.localPosition = new Vector3((float) ((double) border1.x - (double) border2.x + ((double) vector2_1.x - (double) vector2_2.x) * (double) num), 0.0f, 0.0f);
      this.mFG.cachedTransform.localScale = new Vector3(vector2_2.x + border2.x + border2.z, vector2_2.y + border2.y + border2.w, 1f);
      if ((double) num >= 0.99900001287460327 || (double) num <= 1.0 / 1000.0)
        return;
      this.mFG.MakePixelPerfect();
    }
    else
    {
      Vector2 vector2_3;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_3).\u002Ector(vector2_1.x, vector2_1.y * this.mSize);
      this.mFG.pivot = UIWidget.Pivot.Top;
      this.mBG.pivot = UIWidget.Pivot.Top;
      this.mBG.cachedTransform.localPosition = Vector3.zero;
      this.mFG.cachedTransform.localPosition = new Vector3(0.0f, (float) (-(double) border1.y + (double) border2.y - ((double) vector2_1.y - (double) vector2_3.y) * (double) num), 0.0f);
      this.mFG.cachedTransform.localScale = new Vector3(vector2_3.x + border2.x + border2.z, vector2_3.y + border2.y + border2.w, 1f);
      if ((double) num >= 0.99900001287460327 || (double) num <= 1.0 / 1000.0)
        return;
      this.mFG.MakePixelPerfect();
    }
  }

  private void OnDragBackground(GameObject go, Vector2 delta)
  {
    this.mCam = UICamera.currentCamera;
    this.Reposition(UICamera.lastTouchPosition);
  }

  private void OnDragForeground(GameObject go, Vector2 delta)
  {
    this.mCam = UICamera.currentCamera;
    this.Reposition(Vector2.op_Addition(this.mScreenPos, UICamera.currentTouch.totalDelta));
  }

  private void OnPressBackground(GameObject go, bool isPressed)
  {
    this.mCam = UICamera.currentCamera;
    this.Reposition(UICamera.lastTouchPosition);
    if (isPressed || this.onDragFinished == null)
      return;
    this.onDragFinished();
  }

  private void OnPressForeground(GameObject go, bool isPressed)
  {
    if (isPressed)
    {
      this.mCam = UICamera.currentCamera;
      Bounds absoluteWidgetBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.mFG.cachedTransform);
      this.mScreenPos = Vector2.op_Implicit(this.mCam.WorldToScreenPoint(((Bounds) ref absoluteWidgetBounds).center));
    }
    else
    {
      if (this.onDragFinished == null)
        return;
      this.onDragFinished();
    }
  }

  private void Reposition(Vector2 screenPos)
  {
    Transform cachedTransform = this.cachedTransform;
    Plane plane;
    // ISSUE: explicit constructor call
    ((Plane) ref plane).\u002Ector(Quaternion.op_Multiply(cachedTransform.rotation, Vector3.back), cachedTransform.position);
    Ray ray = this.cachedCamera.ScreenPointToRay(Vector2.op_Implicit(screenPos));
    float num;
    if (!((Plane) ref plane).Raycast(ray, ref num))
      return;
    this.CenterOnPos(Vector2.op_Implicit(cachedTransform.InverseTransformPoint(((Ray) ref ray).GetPoint(num))));
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.background, (Object) null) && Object.op_Inequality((Object) ((Component) this.background).collider, (Object) null))
    {
      UIEventListener uiEventListener = UIEventListener.Get(((Component) this.background).gameObject);
      uiEventListener.onPress += new UIEventListener.BoolDelegate(this.OnPressBackground);
      uiEventListener.onDrag += new UIEventListener.VectorDelegate(this.OnDragBackground);
    }
    if (Object.op_Inequality((Object) this.foreground, (Object) null) && Object.op_Inequality((Object) ((Component) this.foreground).collider, (Object) null))
    {
      UIEventListener uiEventListener = UIEventListener.Get(((Component) this.foreground).gameObject);
      uiEventListener.onPress += new UIEventListener.BoolDelegate(this.OnPressForeground);
      uiEventListener.onDrag += new UIEventListener.VectorDelegate(this.OnDragForeground);
    }
    this.ForceUpdate();
  }

  private void Update()
  {
    if (!this.mIsDirty)
      return;
    this.ForceUpdate();
  }

  public float alpha
  {
    get
    {
      if (Object.op_Inequality((Object) this.mFG, (Object) null))
        return this.mFG.alpha;
      return Object.op_Inequality((Object) this.mBG, (Object) null) ? this.mBG.alpha : 0.0f;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mFG, (Object) null))
      {
        this.mFG.alpha = value;
        NGUITools.SetActiveSelf(((Component) this.mFG).gameObject, (double) this.mFG.alpha > 1.0 / 1000.0);
      }
      if (!Object.op_Inequality((Object) this.mBG, (Object) null))
        return;
      this.mBG.alpha = value;
      NGUITools.SetActiveSelf(((Component) this.mBG).gameObject, (double) this.mBG.alpha > 1.0 / 1000.0);
    }
  }

  public UISprite background
  {
    get => this.mBG;
    set
    {
      if (!Object.op_Inequality((Object) this.mBG, (Object) value))
        return;
      this.mBG = value;
      this.mIsDirty = true;
    }
  }

  public float barSize
  {
    get => this.mSize;
    set
    {
      float num = Mathf.Clamp01(value);
      if ((double) this.mSize == (double) num)
        return;
      this.mSize = num;
      this.mIsDirty = true;
      if (this.onChange == null)
        return;
      this.onChange(this);
    }
  }

  public Camera cachedCamera
  {
    get
    {
      if (Object.op_Equality((Object) this.mCam, (Object) null))
        this.mCam = NGUITools.FindCameraForLayer(((Component) this).gameObject.layer);
      return this.mCam;
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

  public UIScrollBar.Direction direction
  {
    get => this.mDir;
    set
    {
      if (this.mDir == value)
        return;
      this.mDir = value;
      this.mIsDirty = true;
      if (!Object.op_Inequality((Object) this.mBG, (Object) null))
        return;
      Transform cachedTransform = this.mBG.cachedTransform;
      Vector3 localScale = cachedTransform.localScale;
      if ((this.mDir != UIScrollBar.Direction.Vertical || (double) localScale.x <= (double) localScale.y) && (this.mDir != UIScrollBar.Direction.Horizontal || (double) localScale.x >= (double) localScale.y))
        return;
      float x = localScale.x;
      localScale.x = localScale.y;
      localScale.y = x;
      cachedTransform.localScale = localScale;
      this.ForceUpdate();
      if (Object.op_Inequality((Object) ((Component) this.mBG).collider, (Object) null))
        NGUITools.AddWidgetCollider(((Component) this.mBG).gameObject);
      if (!Object.op_Inequality((Object) ((Component) this.mFG).collider, (Object) null))
        return;
      NGUITools.AddWidgetCollider(((Component) this.mFG).gameObject);
    }
  }

  public UISprite foreground
  {
    get => this.mFG;
    set
    {
      if (!Object.op_Inequality((Object) this.mFG, (Object) value))
        return;
      this.mFG = value;
      this.mIsDirty = true;
    }
  }

  public bool inverted
  {
    get => this.mInverted;
    set
    {
      if (this.mInverted == value)
        return;
      this.mInverted = value;
      this.mIsDirty = true;
    }
  }

  public float scrollValue
  {
    get => this.mScroll;
    set
    {
      float num = Mathf.Clamp01(value);
      if ((double) this.mScroll == (double) num)
        return;
      this.mScroll = num;
      this.mIsDirty = true;
      if (this.onChange == null)
        return;
      this.onChange(this);
    }
  }

  public enum Direction
  {
    Horizontal,
    Vertical,
  }

  public delegate void OnDragFinished();

  public delegate void OnScrollBarChange(UIScrollBar sb);
}
