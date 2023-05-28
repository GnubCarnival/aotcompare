// Decompiled with JetBrains decompiler
// Type: UIDraggablePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (UIPanel))]
[AddComponentMenu("NGUI/Interaction/Draggable Panel")]
public class UIDraggablePanel : IgnoreTimeScale
{
  public bool disableDragIfFits;
  public UIDraggablePanel.DragEffect dragEffect = UIDraggablePanel.DragEffect.MomentumAndSpring;
  public UIScrollBar horizontalScrollBar;
  public bool iOSDragEmulation = true;
  private Bounds mBounds;
  private bool mCalculatedBounds;
  private int mDragID = -10;
  private bool mDragStarted;
  private Vector2 mDragStartOffset = Vector2.zero;
  private bool mIgnoreCallbacks;
  private Vector3 mLastPos;
  private Vector3 mMomentum = Vector3.zero;
  public float momentumAmount = 35f;
  private UIPanel mPanel;
  private Plane mPlane;
  private bool mPressed;
  private float mScroll;
  private bool mShouldMove;
  private Transform mTrans;
  public UIDraggablePanel.OnDragFinished onDragFinished;
  public Vector2 relativePositionOnReset = Vector2.zero;
  public bool repositionClipping;
  public bool restrictWithinPanel = true;
  public Vector3 scale = Vector3.one;
  public float scrollWheelFactor;
  public UIDraggablePanel.ShowCondition showScrollBars = UIDraggablePanel.ShowCondition.OnlyIfNeeded;
  public bool smoothDragStart = true;
  public UIScrollBar verticalScrollBar;

  private void Awake()
  {
    this.mTrans = ((Component) this).transform;
    this.mPanel = ((Component) this).GetComponent<UIPanel>();
    this.mPanel.onChange += new UIPanel.OnChangeDelegate(this.OnPanelChange);
  }

  public void DisableSpring()
  {
    SpringPanel component = ((Component) this).GetComponent<SpringPanel>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((Behaviour) component).enabled = false;
  }

  public void Drag()
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !this.mShouldMove)
      return;
    if (this.mDragID == -10)
      this.mDragID = UICamera.currentTouchID;
    UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
    if (this.smoothDragStart && !this.mDragStarted)
    {
      this.mDragStarted = true;
      this.mDragStartOffset = UICamera.currentTouch.totalDelta;
    }
    Ray ray = !this.smoothDragStart ? UICamera.currentCamera.ScreenPointToRay(Vector2.op_Implicit(UICamera.currentTouch.pos)) : UICamera.currentCamera.ScreenPointToRay(Vector2.op_Implicit(Vector2.op_Subtraction(UICamera.currentTouch.pos, this.mDragStartOffset)));
    float num = 0.0f;
    if (!((Plane) ref this.mPlane).Raycast(ray, ref num))
      return;
    Vector3 point = ((Ray) ref ray).GetPoint(num);
    Vector3 absolute = Vector3.op_Subtraction(point, this.mLastPos);
    this.mLastPos = point;
    if ((double) absolute.x != 0.0 || (double) absolute.y != 0.0)
    {
      Vector3 vector3 = this.mTrans.InverseTransformDirection(absolute);
      ((Vector3) ref vector3).Scale(this.scale);
      absolute = this.mTrans.TransformDirection(vector3);
    }
    this.mMomentum = Vector3.Lerp(this.mMomentum, Vector3.op_Addition(this.mMomentum, Vector3.op_Multiply(absolute, 0.01f * this.momentumAmount)), 0.67f);
    if (!this.iOSDragEmulation)
    {
      this.MoveAbsolute(absolute);
    }
    else
    {
      UIPanel mPanel = this.mPanel;
      Bounds bounds1 = this.bounds;
      Vector2 min = Vector2.op_Implicit(((Bounds) ref bounds1).min);
      Bounds bounds2 = this.bounds;
      Vector2 max = Vector2.op_Implicit(((Bounds) ref bounds2).max);
      Vector3 constrainOffset = mPanel.CalculateConstrainOffset(min, max);
      if ((double) ((Vector3) ref constrainOffset).magnitude > 1.0 / 1000.0)
      {
        this.MoveAbsolute(Vector3.op_Multiply(absolute, 0.5f));
        this.mMomentum = Vector3.op_Multiply(this.mMomentum, 0.5f);
      }
      else
        this.MoveAbsolute(absolute);
    }
    if (!this.restrictWithinPanel || this.mPanel.clipping == UIDrawCall.Clipping.None || this.dragEffect == UIDraggablePanel.DragEffect.MomentumAndSpring)
      return;
    this.RestrictWithinBounds(true);
  }

  private void LateUpdate()
  {
    if (this.repositionClipping)
    {
      this.repositionClipping = false;
      this.mCalculatedBounds = false;
      this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
    }
    if (!Application.isPlaying)
      return;
    float deltaTime = this.UpdateRealTimeDelta();
    if (this.showScrollBars != UIDraggablePanel.ShowCondition.Always)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.showScrollBars != UIDraggablePanel.ShowCondition.WhenDragging || this.mDragID != -10 || (double) ((Vector3) ref this.mMomentum).magnitude > 0.0099999997764825821)
      {
        flag1 = this.shouldMoveVertically;
        flag2 = this.shouldMoveHorizontally;
      }
      if (Object.op_Inequality((Object) this.verticalScrollBar, (Object) null))
      {
        float num = Mathf.Clamp01(this.verticalScrollBar.alpha + (!flag1 ? (float) (-(double) deltaTime * 3.0) : deltaTime * 6f));
        if ((double) this.verticalScrollBar.alpha != (double) num)
          this.verticalScrollBar.alpha = num;
      }
      if (Object.op_Inequality((Object) this.horizontalScrollBar, (Object) null))
      {
        float num = Mathf.Clamp01(this.horizontalScrollBar.alpha + (!flag2 ? (float) (-(double) deltaTime * 3.0) : deltaTime * 6f));
        if ((double) this.horizontalScrollBar.alpha != (double) num)
          this.horizontalScrollBar.alpha = num;
      }
    }
    if (this.mShouldMove && !this.mPressed)
    {
      this.mMomentum = Vector3.op_Subtraction(this.mMomentum, Vector3.op_Multiply(this.scale, this.mScroll * 0.05f));
      if ((double) ((Vector3) ref this.mMomentum).magnitude > 9.9999997473787516E-05)
      {
        this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0.0f, 20f, deltaTime);
        this.MoveAbsolute(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
        if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
          this.RestrictWithinBounds(false);
        if ((double) ((Vector3) ref this.mMomentum).magnitude >= 9.9999997473787516E-05 || this.onDragFinished == null)
          return;
        this.onDragFinished();
        return;
      }
      this.mScroll = 0.0f;
      this.mMomentum = Vector3.zero;
    }
    else
      this.mScroll = 0.0f;
    NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
  }

  public void MoveAbsolute(Vector3 absolute) => this.MoveRelative(Vector3.op_Subtraction(this.mTrans.InverseTransformPoint(absolute), this.mTrans.InverseTransformPoint(Vector3.zero)));

  public void MoveRelative(Vector3 relative)
  {
    Transform mTrans = this.mTrans;
    mTrans.localPosition = Vector3.op_Addition(mTrans.localPosition, relative);
    Vector4 clipRange = this.mPanel.clipRange;
    clipRange.x -= relative.x;
    clipRange.y -= relative.y;
    this.mPanel.clipRange = clipRange;
    this.UpdateScrollbars(false);
  }

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
      return;
    this.mPanel.onChange -= new UIPanel.OnChangeDelegate(this.OnPanelChange);
  }

  private void OnHorizontalBar(UIScrollBar sb)
  {
    if (this.mIgnoreCallbacks)
      return;
    this.SetDragAmount(Object.op_Equality((Object) this.horizontalScrollBar, (Object) null) ? 0.0f : this.horizontalScrollBar.scrollValue, Object.op_Equality((Object) this.verticalScrollBar, (Object) null) ? 0.0f : this.verticalScrollBar.scrollValue, false);
  }

  private void OnPanelChange() => this.UpdateScrollbars(true);

  private void OnVerticalBar(UIScrollBar sb)
  {
    if (this.mIgnoreCallbacks)
      return;
    this.SetDragAmount(Object.op_Equality((Object) this.horizontalScrollBar, (Object) null) ? 0.0f : this.horizontalScrollBar.scrollValue, Object.op_Equality((Object) this.verticalScrollBar, (Object) null) ? 0.0f : this.verticalScrollBar.scrollValue, false);
  }

  public void Press(bool pressed)
  {
    if (this.smoothDragStart & pressed)
    {
      this.mDragStarted = false;
      this.mDragStartOffset = Vector2.zero;
    }
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject))
      return;
    if (!pressed && this.mDragID == UICamera.currentTouchID)
      this.mDragID = -10;
    this.mCalculatedBounds = false;
    this.mShouldMove = this.shouldMove;
    if (!this.mShouldMove)
      return;
    this.mPressed = pressed;
    if (pressed)
    {
      this.mMomentum = Vector3.zero;
      this.mScroll = 0.0f;
      this.DisableSpring();
      this.mLastPos = ((RaycastHit) ref UICamera.lastHit).point;
      this.mPlane = new Plane(Quaternion.op_Multiply(this.mTrans.rotation, Vector3.back), this.mLastPos);
    }
    else
    {
      if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect == UIDraggablePanel.DragEffect.MomentumAndSpring)
        this.RestrictWithinBounds(false);
      if (this.onDragFinished == null)
        return;
      this.onDragFinished();
    }
  }

  public void ResetPosition()
  {
    this.mCalculatedBounds = false;
    this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, false);
    this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
  }

  public bool RestrictWithinBounds(bool instant)
  {
    UIPanel mPanel = this.mPanel;
    Bounds bounds = this.bounds;
    Vector2 min = Vector2.op_Implicit(((Bounds) ref bounds).min);
    bounds = this.bounds;
    Vector2 max = Vector2.op_Implicit(((Bounds) ref bounds).max);
    Vector3 constrainOffset = mPanel.CalculateConstrainOffset(min, max);
    if ((double) ((Vector3) ref constrainOffset).magnitude <= 1.0 / 1000.0)
      return false;
    if (!instant && this.dragEffect == UIDraggablePanel.DragEffect.MomentumAndSpring)
    {
      SpringPanel.Begin(((Component) this.mPanel).gameObject, Vector3.op_Addition(this.mTrans.localPosition, constrainOffset), 13f);
    }
    else
    {
      this.MoveRelative(constrainOffset);
      this.mMomentum = Vector3.zero;
      this.mScroll = 0.0f;
    }
    return true;
  }

  public void Scroll(float delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || (double) this.scrollWheelFactor == 0.0)
      return;
    this.DisableSpring();
    this.mShouldMove = this.shouldMove;
    if ((double) Mathf.Sign(this.mScroll) != (double) Mathf.Sign(delta))
      this.mScroll = 0.0f;
    this.mScroll += delta * this.scrollWheelFactor;
  }

  public void SetDragAmount(float x, float y, bool updateScrollbars)
  {
    this.DisableSpring();
    Bounds bounds = this.bounds;
    if ((double) ((Bounds) ref bounds).min.x == (double) ((Bounds) ref bounds).max.x || (double) ((Bounds) ref bounds).min.y == (double) ((Bounds) ref bounds).max.y)
      return;
    Vector4 clipRange = this.mPanel.clipRange;
    float num1 = clipRange.z * 0.5f;
    float num2 = clipRange.w * 0.5f;
    float num3 = ((Bounds) ref bounds).min.x + num1;
    float num4 = ((Bounds) ref bounds).max.x - num1;
    float num5 = ((Bounds) ref bounds).min.y + num2;
    float num6 = ((Bounds) ref bounds).max.y - num2;
    if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
    {
      num3 -= this.mPanel.clipSoftness.x;
      num4 += this.mPanel.clipSoftness.x;
      num5 -= this.mPanel.clipSoftness.y;
      num6 += this.mPanel.clipSoftness.y;
    }
    float num7 = Mathf.Lerp(num3, num4, x);
    float num8 = Mathf.Lerp(num6, num5, y);
    if (!updateScrollbars)
    {
      Vector3 localPosition = this.mTrans.localPosition;
      if ((double) this.scale.x != 0.0)
        localPosition.x += clipRange.x - num7;
      if ((double) this.scale.y != 0.0)
        localPosition.y += clipRange.y - num8;
      this.mTrans.localPosition = localPosition;
    }
    clipRange.x = num7;
    clipRange.y = num8;
    this.mPanel.clipRange = clipRange;
    if (!updateScrollbars)
      return;
    this.UpdateScrollbars(false);
  }

  private void Start()
  {
    this.UpdateScrollbars(true);
    if (Object.op_Inequality((Object) this.horizontalScrollBar, (Object) null))
    {
      this.horizontalScrollBar.onChange += new UIScrollBar.OnScrollBarChange(this.OnHorizontalBar);
      this.horizontalScrollBar.alpha = this.showScrollBars == UIDraggablePanel.ShowCondition.Always || this.shouldMoveHorizontally ? 1f : 0.0f;
    }
    if (!Object.op_Inequality((Object) this.verticalScrollBar, (Object) null))
      return;
    this.verticalScrollBar.onChange += new UIScrollBar.OnScrollBarChange(this.OnVerticalBar);
    this.verticalScrollBar.alpha = this.showScrollBars == UIDraggablePanel.ShowCondition.Always || this.shouldMoveVertically ? 1f : 0.0f;
  }

  public void UpdateScrollbars(bool recalculateBounds)
  {
    if (!Object.op_Inequality((Object) this.mPanel, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.horizontalScrollBar, (Object) null) || Object.op_Inequality((Object) this.verticalScrollBar, (Object) null))
    {
      if (recalculateBounds)
      {
        this.mCalculatedBounds = false;
        this.mShouldMove = this.shouldMove;
      }
      Bounds bounds = this.bounds;
      Vector2 vector2_1 = Vector2.op_Implicit(((Bounds) ref bounds).min);
      Vector2 vector2_2 = Vector2.op_Implicit(((Bounds) ref bounds).max);
      if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
      {
        Vector2 clipSoftness = this.mPanel.clipSoftness;
        vector2_1 = Vector2.op_Subtraction(vector2_1, clipSoftness);
        vector2_2 = Vector2.op_Addition(vector2_2, clipSoftness);
      }
      if (Object.op_Inequality((Object) this.horizontalScrollBar, (Object) null) && (double) vector2_2.x > (double) vector2_1.x)
      {
        Vector4 clipRange = this.mPanel.clipRange;
        float num1 = clipRange.z * 0.5f;
        float num2 = clipRange.x - num1 - ((Bounds) ref bounds).min.x;
        float num3 = ((Bounds) ref bounds).max.x - num1 - clipRange.x;
        float num4 = vector2_2.x - vector2_1.x;
        float num5 = Mathf.Clamp01(num2 / num4);
        float num6 = Mathf.Clamp01(num3 / num4);
        float num7 = num5 + num6;
        this.mIgnoreCallbacks = true;
        this.horizontalScrollBar.barSize = 1f - num7;
        this.horizontalScrollBar.scrollValue = (double) num7 <= 1.0 / 1000.0 ? 0.0f : num5 / num7;
        this.mIgnoreCallbacks = false;
      }
      if (!Object.op_Inequality((Object) this.verticalScrollBar, (Object) null) || (double) vector2_2.y <= (double) vector2_1.y)
        return;
      Vector4 clipRange1 = this.mPanel.clipRange;
      float num8 = clipRange1.w * 0.5f;
      float num9 = clipRange1.y - num8 - vector2_1.y;
      float num10 = vector2_2.y - num8 - clipRange1.y;
      float num11 = vector2_2.y - vector2_1.y;
      float num12 = Mathf.Clamp01(num9 / num11);
      float num13 = Mathf.Clamp01(num10 / num11);
      float num14 = num12 + num13;
      this.mIgnoreCallbacks = true;
      this.verticalScrollBar.barSize = 1f - num14;
      this.verticalScrollBar.scrollValue = (double) num14 <= 1.0 / 1000.0 ? 0.0f : (float) (1.0 - (double) num12 / (double) num14);
      this.mIgnoreCallbacks = false;
    }
    else
    {
      if (!recalculateBounds)
        return;
      this.mCalculatedBounds = false;
    }
  }

  public Bounds bounds
  {
    get
    {
      if (!this.mCalculatedBounds)
      {
        this.mCalculatedBounds = true;
        this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
      }
      return this.mBounds;
    }
  }

  public Vector3 currentMomentum
  {
    get => this.mMomentum;
    set
    {
      this.mMomentum = value;
      this.mShouldMove = true;
    }
  }

  public UIPanel panel => this.mPanel;

  private bool shouldMove
  {
    get
    {
      if (!this.disableDragIfFits)
        return true;
      if (Object.op_Equality((Object) this.mPanel, (Object) null))
        this.mPanel = ((Component) this).GetComponent<UIPanel>();
      Vector4 clipRange = this.mPanel.clipRange;
      Bounds bounds = this.bounds;
      float num1 = (double) clipRange.z != 0.0 ? clipRange.z * 0.5f : (float) Screen.width;
      float num2 = (double) clipRange.w != 0.0 ? clipRange.w * 0.5f : (float) Screen.height;
      return !Mathf.Approximately(this.scale.x, 0.0f) && ((double) ((Bounds) ref bounds).min.x < (double) clipRange.x - (double) num1 || (double) ((Bounds) ref bounds).max.x > (double) clipRange.x + (double) num1) || !Mathf.Approximately(this.scale.y, 0.0f) && ((double) ((Bounds) ref bounds).min.y < (double) clipRange.y - (double) num2 || (double) ((Bounds) ref bounds).max.y > (double) clipRange.y + (double) num2);
    }
  }

  public bool shouldMoveHorizontally
  {
    get
    {
      Bounds bounds = this.bounds;
      float x = ((Bounds) ref bounds).size.x;
      if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
        x += this.mPanel.clipSoftness.x * 2f;
      return (double) x > (double) this.mPanel.clipRange.z;
    }
  }

  public bool shouldMoveVertically
  {
    get
    {
      Bounds bounds = this.bounds;
      float y = ((Bounds) ref bounds).size.y;
      if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
        y += this.mPanel.clipSoftness.y * 2f;
      return (double) y > (double) this.mPanel.clipRange.w;
    }
  }

  public enum DragEffect
  {
    None,
    Momentum,
    MomentumAndSpring,
  }

  public delegate void OnDragFinished();

  public enum ShowCondition
  {
    Always,
    OnlyIfNeeded,
    WhenDragging,
  }
}
