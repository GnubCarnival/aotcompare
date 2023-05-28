// Decompiled with JetBrains decompiler
// Type: UIDragObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : IgnoreTimeScale
{
  public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;
  private Bounds mBounds;
  private Vector3 mLastPos;
  private Vector3 mMomentum = Vector3.zero;
  public float momentumAmount = 35f;
  private UIPanel mPanel;
  private Plane mPlane;
  private bool mPressed;
  private float mScroll;
  public bool restrictWithinPanel;
  public Vector3 scale = Vector3.one;
  public float scrollWheelFactor;
  public Transform target;

  private void FindPanel()
  {
    this.mPanel = Object.op_Equality((Object) this.target, (Object) null) ? (UIPanel) null : UIPanel.Find(((Component) this.target).transform, false);
    if (!Object.op_Equality((Object) this.mPanel, (Object) null))
      return;
    this.restrictWithinPanel = false;
  }

  private void LateUpdate()
  {
    float deltaTime = this.UpdateRealTimeDelta();
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    if (this.mPressed)
    {
      SpringPosition component = ((Component) this.target).GetComponent<SpringPosition>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = false;
      this.mScroll = 0.0f;
    }
    else
    {
      this.mMomentum = Vector3.op_Addition(this.mMomentum, Vector3.op_Multiply(this.scale, (float) (-(double) this.mScroll * 0.05000000074505806)));
      this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0.0f, 20f, deltaTime);
      if ((double) ((Vector3) ref this.mMomentum).magnitude > 9.9999997473787516E-05)
      {
        if (Object.op_Equality((Object) this.mPanel, (Object) null))
          this.FindPanel();
        if (Object.op_Inequality((Object) this.mPanel, (Object) null))
        {
          Transform target = this.target;
          target.position = Vector3.op_Addition(target.position, NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
          if (!this.restrictWithinPanel || this.mPanel.clipping == UIDrawCall.Clipping.None)
            return;
          this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
          if (this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
            return;
          SpringPosition component = ((Component) this.target).GetComponent<SpringPosition>();
          if (!Object.op_Inequality((Object) component, (Object) null))
            return;
          ((Behaviour) component).enabled = false;
          return;
        }
      }
      else
        this.mScroll = 0.0f;
    }
    NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
  }

  private void OnDrag(Vector2 delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.target, (Object) null))
      return;
    UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
    Ray ray = UICamera.currentCamera.ScreenPointToRay(Vector2.op_Implicit(UICamera.currentTouch.pos));
    float num = 0.0f;
    if (!((Plane) ref this.mPlane).Raycast(ray, ref num))
      return;
    Vector3 point = ((Ray) ref ray).GetPoint(num);
    Vector3 vector3_1 = Vector3.op_Subtraction(point, this.mLastPos);
    this.mLastPos = point;
    if ((double) vector3_1.x != 0.0 || (double) vector3_1.y != 0.0)
    {
      Vector3 vector3_2 = this.target.InverseTransformDirection(vector3_1);
      ((Vector3) ref vector3_2).Scale(this.scale);
      vector3_1 = this.target.TransformDirection(vector3_2);
    }
    if (this.dragEffect != UIDragObject.DragEffect.None)
      this.mMomentum = Vector3.Lerp(this.mMomentum, Vector3.op_Addition(this.mMomentum, Vector3.op_Multiply(vector3_1, 0.01f * this.momentumAmount)), 0.67f);
    if (this.restrictWithinPanel)
    {
      Vector3 localPosition = this.target.localPosition;
      Transform target = this.target;
      target.position = Vector3.op_Addition(target.position, vector3_1);
      ref Bounds local = ref this.mBounds;
      ((Bounds) ref local).center = Vector3.op_Addition(((Bounds) ref local).center, Vector3.op_Subtraction(this.target.localPosition, localPosition));
      if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring || this.mPanel.clipping == UIDrawCall.Clipping.None || !this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
        return;
      this.mMomentum = Vector3.zero;
      this.mScroll = 0.0f;
    }
    else
    {
      Transform target = this.target;
      target.position = Vector3.op_Addition(target.position, vector3_1);
    }
  }

  private void OnPress(bool pressed)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.target, (Object) null))
      return;
    this.mPressed = pressed;
    if (pressed)
    {
      if (this.restrictWithinPanel && Object.op_Equality((Object) this.mPanel, (Object) null))
        this.FindPanel();
      if (this.restrictWithinPanel)
        this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
      this.mMomentum = Vector3.zero;
      this.mScroll = 0.0f;
      SpringPosition component = ((Component) this.target).GetComponent<SpringPosition>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = false;
      this.mLastPos = ((RaycastHit) ref UICamera.lastHit).point;
      Transform transform = ((Component) UICamera.currentCamera).transform;
      this.mPlane = new Plane(Quaternion.op_Multiply(Object.op_Equality((Object) this.mPanel, (Object) null) ? transform.rotation : this.mPanel.cachedTransform.rotation, Vector3.back), this.mLastPos);
    }
    else
    {
      if (!this.restrictWithinPanel || this.mPanel.clipping == UIDrawCall.Clipping.None || this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring)
        return;
      this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, false);
    }
  }

  private void OnScroll(float delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject))
      return;
    if ((double) Mathf.Sign(this.mScroll) != (double) Mathf.Sign(delta))
      this.mScroll = 0.0f;
    this.mScroll += delta * this.scrollWheelFactor;
  }

  public enum DragEffect
  {
    None,
    Momentum,
    MomentumAndSpring,
  }
}
