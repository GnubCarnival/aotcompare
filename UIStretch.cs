// Decompiled with JetBrains decompiler
// Type: UIStretch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Stretch")]
[ExecuteInEditMode]
public class UIStretch : MonoBehaviour
{
  public Vector2 initialSize = Vector2.one;
  private Animation mAnim;
  private Rect mRect;
  private UIRoot mRoot;
  private Transform mTrans;
  public UIPanel panelContainer;
  public Vector2 relativeSize = Vector2.one;
  public UIStretch.Style style;
  public Camera uiCamera;
  public UIWidget widgetContainer;

  private void Awake()
  {
    this.mAnim = ((Component) this).animation;
    this.mRect = new Rect();
    this.mTrans = ((Component) this).transform;
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.uiCamera, (Object) null))
      this.uiCamera = NGUITools.FindCameraForLayer(((Component) this).gameObject.layer);
    this.mRoot = NGUITools.FindInParents<UIRoot>(((Component) this).gameObject);
  }

  private void Update()
  {
    if (!Object.op_Equality((Object) this.mAnim, (Object) null) && this.mAnim.isPlaying || this.style == UIStretch.Style.None)
      return;
    float num1 = 1f;
    if (Object.op_Inequality((Object) this.panelContainer, (Object) null))
    {
      if (this.panelContainer.clipping == UIDrawCall.Clipping.None)
      {
        ((Rect) ref this.mRect).xMin = (float) -Screen.width * 0.5f;
        ((Rect) ref this.mRect).yMin = (float) -Screen.height * 0.5f;
        ((Rect) ref this.mRect).xMax = -((Rect) ref this.mRect).xMin;
        ((Rect) ref this.mRect).yMax = -((Rect) ref this.mRect).yMin;
      }
      else
      {
        Vector4 clipRange = this.panelContainer.clipRange;
        ((Rect) ref this.mRect).x = clipRange.x - clipRange.z * 0.5f;
        ((Rect) ref this.mRect).y = clipRange.y - clipRange.w * 0.5f;
        ((Rect) ref this.mRect).width = clipRange.z;
        ((Rect) ref this.mRect).height = clipRange.w;
      }
    }
    else if (Object.op_Inequality((Object) this.widgetContainer, (Object) null))
    {
      Transform cachedTransform = this.widgetContainer.cachedTransform;
      Vector3 localScale = cachedTransform.localScale;
      Vector3 localPosition = cachedTransform.localPosition;
      Vector3 vector3_1 = Vector2.op_Implicit(this.widgetContainer.relativeSize);
      Vector3 vector3_2 = Vector2.op_Implicit(this.widgetContainer.pivotOffset);
      --vector3_2.y;
      vector3_2.x *= this.widgetContainer.relativeSize.x * localScale.x;
      vector3_2.y *= this.widgetContainer.relativeSize.y * localScale.y;
      ((Rect) ref this.mRect).x = localPosition.x + vector3_2.x;
      ((Rect) ref this.mRect).y = localPosition.y + vector3_2.y;
      ((Rect) ref this.mRect).width = vector3_1.x * localScale.x;
      ((Rect) ref this.mRect).height = vector3_1.y * localScale.y;
    }
    else
    {
      if (!Object.op_Inequality((Object) this.uiCamera, (Object) null))
        return;
      this.mRect = this.uiCamera.pixelRect;
      if (Object.op_Inequality((Object) this.mRoot, (Object) null))
        num1 = this.mRoot.pixelSizeAdjustment;
    }
    float width = ((Rect) ref this.mRect).width;
    float height = ((Rect) ref this.mRect).height;
    if ((double) num1 != 1.0 && (double) height > 1.0)
    {
      float num2 = (float) this.mRoot.activeHeight / height;
      width *= num2;
      height *= num2;
    }
    Vector3 localScale1 = this.mTrans.localScale;
    if (this.style == UIStretch.Style.BasedOnHeight)
    {
      localScale1.x = this.relativeSize.x * height;
      localScale1.y = this.relativeSize.y * height;
    }
    else if (this.style == UIStretch.Style.FillKeepingRatio)
    {
      if ((double) this.initialSize.x / (double) this.initialSize.y < (double) (width / height))
      {
        float num3 = width / this.initialSize.x;
        localScale1.x = width;
        localScale1.y = this.initialSize.y * num3;
      }
      else
      {
        float num4 = height / this.initialSize.y;
        localScale1.x = this.initialSize.x * num4;
        localScale1.y = height;
      }
    }
    else if (this.style == UIStretch.Style.FitInternalKeepingRatio)
    {
      if ((double) this.initialSize.x / (double) this.initialSize.y > (double) (width / height))
      {
        float num5 = width / this.initialSize.x;
        localScale1.x = width;
        localScale1.y = this.initialSize.y * num5;
      }
      else
      {
        float num6 = height / this.initialSize.y;
        localScale1.x = this.initialSize.x * num6;
        localScale1.y = height;
      }
    }
    else
    {
      if (this.style == UIStretch.Style.Both || this.style == UIStretch.Style.Horizontal)
        localScale1.x = this.relativeSize.x * width;
      if (this.style == UIStretch.Style.Both || this.style == UIStretch.Style.Vertical)
        localScale1.y = this.relativeSize.y * height;
    }
    if (!Vector3.op_Inequality(this.mTrans.localScale, localScale1))
      return;
    this.mTrans.localScale = localScale1;
  }

  public enum Style
  {
    None,
    Horizontal,
    Vertical,
    Both,
    BasedOnHeight,
    FillKeepingRatio,
    FitInternalKeepingRatio,
  }
}
