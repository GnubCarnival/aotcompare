// Decompiled with JetBrains decompiler
// Type: UIDraggableCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("NGUI/Interaction/Draggable Camera")]
public class UIDraggableCamera : IgnoreTimeScale
{
  public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;
  private Bounds mBounds;
  private Camera mCam;
  private bool mDragStarted;
  private Vector2 mMomentum = Vector2.zero;
  public float momentumAmount = 35f;
  private bool mPressed;
  private UIRoot mRoot;
  private float mScroll;
  private Transform mTrans;
  public Transform rootForBounds;
  public Vector2 scale = Vector2.one;
  public float scrollWheelFactor;
  public bool smoothDragStart = true;

  private void Awake()
  {
    this.mCam = ((Component) this).camera;
    this.mTrans = ((Component) this).transform;
    if (!Object.op_Equality((Object) this.rootForBounds, (Object) null))
      return;
    Debug.LogError((object) (NGUITools.GetHierarchy(((Component) this).gameObject) + " needs the 'Root For Bounds' parameter to be set"), (Object) this);
    ((Behaviour) this).enabled = false;
  }

  private Vector3 CalculateConstrainOffset()
  {
    if (Object.op_Equality((Object) this.rootForBounds, (Object) null) || this.rootForBounds.childCount == 0)
      return Vector3.zero;
    Vector3 vector3_1;
    ref Vector3 local1 = ref vector3_1;
    Rect rect1 = this.mCam.rect;
    double num1 = (double) ((Rect) ref rect1).xMin * (double) Screen.width;
    Rect rect2 = this.mCam.rect;
    double num2 = (double) ((Rect) ref rect2).yMin * (double) Screen.height;
    // ISSUE: explicit constructor call
    ((Vector3) ref local1).\u002Ector((float) num1, (float) num2, 0.0f);
    Vector3 vector3_2;
    ref Vector3 local2 = ref vector3_2;
    Rect rect3 = this.mCam.rect;
    double num3 = (double) ((Rect) ref rect3).xMax * (double) Screen.width;
    Rect rect4 = this.mCam.rect;
    double num4 = (double) ((Rect) ref rect4).yMax * (double) Screen.height;
    // ISSUE: explicit constructor call
    ((Vector3) ref local2).\u002Ector((float) num3, (float) num4, 0.0f);
    Vector3 worldPoint1 = this.mCam.ScreenToWorldPoint(vector3_1);
    Vector3 worldPoint2 = this.mCam.ScreenToWorldPoint(vector3_2);
    Vector2 minRect;
    // ISSUE: explicit constructor call
    ((Vector2) ref minRect).\u002Ector(((Bounds) ref this.mBounds).min.x, ((Bounds) ref this.mBounds).min.y);
    Vector2 maxRect;
    // ISSUE: explicit constructor call
    ((Vector2) ref maxRect).\u002Ector(((Bounds) ref this.mBounds).max.x, ((Bounds) ref this.mBounds).max.y);
    return Vector2.op_Implicit(NGUIMath.ConstrainRect(minRect, maxRect, Vector2.op_Implicit(worldPoint1), Vector2.op_Implicit(worldPoint2)));
  }

  public bool ConstrainToBounds(bool immediate)
  {
    if (Object.op_Inequality((Object) this.mTrans, (Object) null) && Object.op_Inequality((Object) this.rootForBounds, (Object) null))
    {
      Vector3 constrainOffset = this.CalculateConstrainOffset();
      if ((double) ((Vector3) ref constrainOffset).magnitude > 0.0)
      {
        if (immediate)
        {
          Transform mTrans = this.mTrans;
          mTrans.position = Vector3.op_Subtraction(mTrans.position, constrainOffset);
        }
        else
        {
          SpringPosition springPosition = SpringPosition.Begin(((Component) this).gameObject, Vector3.op_Subtraction(this.mTrans.position, constrainOffset), 13f);
          springPosition.ignoreTimeScale = true;
          springPosition.worldSpace = true;
        }
        return true;
      }
    }
    return false;
  }

  public void Drag(Vector2 delta)
  {
    if (this.smoothDragStart && !this.mDragStarted)
    {
      this.mDragStarted = true;
    }
    else
    {
      UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
      if (Object.op_Inequality((Object) this.mRoot, (Object) null))
        delta = Vector2.op_Multiply(delta, this.mRoot.pixelSizeAdjustment);
      Vector2 vector2 = Vector2.Scale(delta, Vector2.op_UnaryNegation(this.scale));
      Transform mTrans = this.mTrans;
      mTrans.localPosition = Vector3.op_Addition(mTrans.localPosition, Vector2.op_Implicit(vector2));
      this.mMomentum = Vector2.Lerp(this.mMomentum, Vector2.op_Addition(this.mMomentum, Vector2.op_Multiply(vector2, 0.01f * this.momentumAmount)), 0.67f);
      if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring || !this.ConstrainToBounds(true))
        return;
      this.mMomentum = Vector2.zero;
      this.mScroll = 0.0f;
    }
  }

  public void Press(bool isPressed)
  {
    if (isPressed)
      this.mDragStarted = false;
    if (!Object.op_Inequality((Object) this.rootForBounds, (Object) null))
      return;
    this.mPressed = isPressed;
    if (isPressed)
    {
      this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
      this.mMomentum = Vector2.zero;
      this.mScroll = 0.0f;
      SpringPosition component = ((Component) this).GetComponent<SpringPosition>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      ((Behaviour) component).enabled = false;
    }
    else
    {
      if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring)
        return;
      this.ConstrainToBounds(false);
    }
  }

  public void Scroll(float delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject))
      return;
    if ((double) Mathf.Sign(this.mScroll) != (double) Mathf.Sign(delta))
      this.mScroll = 0.0f;
    this.mScroll += delta * this.scrollWheelFactor;
  }

  private void Start() => this.mRoot = NGUITools.FindInParents<UIRoot>(((Component) this).gameObject);

  private void Update()
  {
    float deltaTime = this.UpdateRealTimeDelta();
    if (this.mPressed)
    {
      SpringPosition component = ((Component) this).GetComponent<SpringPosition>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = false;
      this.mScroll = 0.0f;
    }
    else
    {
      this.mMomentum = Vector2.op_Addition(this.mMomentum, Vector2.op_Multiply(this.scale, this.mScroll * 20f));
      this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0.0f, 20f, deltaTime);
      if ((double) ((Vector2) ref this.mMomentum).magnitude > 0.0099999997764825821)
      {
        Transform mTrans = this.mTrans;
        mTrans.localPosition = Vector3.op_Addition(mTrans.localPosition, Vector2.op_Implicit(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime)));
        this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
        if (this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
          return;
        SpringPosition component = ((Component) this).GetComponent<SpringPosition>();
        if (!Object.op_Inequality((Object) component, (Object) null))
          return;
        ((Behaviour) component).enabled = false;
        return;
      }
      this.mScroll = 0.0f;
    }
    NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
  }

  public Vector2 currentMomentum
  {
    get => this.mMomentum;
    set => this.mMomentum = value;
  }
}
