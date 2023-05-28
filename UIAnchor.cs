// Decompiled with JetBrains decompiler
// Type: UIAnchor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
  public bool halfPixelOffset = true;
  private Animation mAnim;
  private bool mNeedsHalfPixelOffset;
  private Rect mRect;
  private UIRoot mRoot;
  private Transform mTrans;
  public UIPanel panelContainer;
  public Vector2 relativeOffset = Vector2.zero;
  public bool runOnlyOnce;
  public UIAnchor.Side side = UIAnchor.Side.Center;
  public Camera uiCamera;
  public UIWidget widgetContainer;

  private void Awake()
  {
    this.mTrans = ((Component) this).transform;
    this.mAnim = ((Component) this).animation;
  }

  private void Start()
  {
    this.mRoot = NGUITools.FindInParents<UIRoot>(((Component) this).gameObject);
    this.mNeedsHalfPixelOffset = Application.platform == 2 || Application.platform == 10 || Application.platform == 5 || Application.platform == 7;
    if (this.mNeedsHalfPixelOffset)
      this.mNeedsHalfPixelOffset = SystemInfo.graphicsShaderLevel < 40;
    if (Object.op_Equality((Object) this.uiCamera, (Object) null))
      this.uiCamera = NGUITools.FindCameraForLayer(((Component) this).gameObject.layer);
    this.Update();
  }

  private void Update()
  {
    if (!Object.op_Equality((Object) this.mAnim, (Object) null) && ((Behaviour) this.mAnim).enabled && this.mAnim.isPlaying)
      return;
    bool flag = false;
    if (Object.op_Inequality((Object) this.panelContainer, (Object) null))
    {
      if (this.panelContainer.clipping == UIDrawCall.Clipping.None)
      {
        float num = Object.op_Equality((Object) this.mRoot, (Object) null) ? 0.5f : (float) ((double) this.mRoot.activeHeight / (double) Screen.height * 0.5);
        ((Rect) ref this.mRect).xMin = (float) -Screen.width * num;
        ((Rect) ref this.mRect).yMin = (float) -Screen.height * num;
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
      flag = true;
      this.mRect = this.uiCamera.pixelRect;
    }
    float num1 = (float) (((double) ((Rect) ref this.mRect).xMin + (double) ((Rect) ref this.mRect).xMax) * 0.5);
    float num2 = (float) (((double) ((Rect) ref this.mRect).yMin + (double) ((Rect) ref this.mRect).yMax) * 0.5);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(num1, num2, 0.0f);
    if (this.side != UIAnchor.Side.Center)
    {
      vector3.x = this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight ? ((Rect) ref this.mRect).xMax : (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom ? num1 : ((Rect) ref this.mRect).xMin);
      vector3.y = this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft ? ((Rect) ref this.mRect).yMax : (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right ? num2 : ((Rect) ref this.mRect).yMin);
    }
    float width = ((Rect) ref this.mRect).width;
    float height = ((Rect) ref this.mRect).height;
    vector3.x += this.relativeOffset.x * width;
    vector3.y += this.relativeOffset.y * height;
    if (flag)
    {
      if (this.uiCamera.orthographic)
      {
        vector3.x = Mathf.Round(vector3.x);
        vector3.y = Mathf.Round(vector3.y);
        if (this.halfPixelOffset && this.mNeedsHalfPixelOffset)
        {
          vector3.x -= 0.5f;
          vector3.y += 0.5f;
        }
      }
      vector3.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
      vector3 = this.uiCamera.ScreenToWorldPoint(vector3);
    }
    else
    {
      vector3.x = Mathf.Round(vector3.x);
      vector3.y = Mathf.Round(vector3.y);
      if (Object.op_Inequality((Object) this.panelContainer, (Object) null))
        vector3 = this.panelContainer.cachedTransform.TransformPoint(vector3);
      else if (Object.op_Inequality((Object) this.widgetContainer, (Object) null))
      {
        Transform parent = this.widgetContainer.cachedTransform.parent;
        if (Object.op_Inequality((Object) parent, (Object) null))
          vector3 = parent.TransformPoint(vector3);
      }
      vector3.z = this.mTrans.position.z;
    }
    if (Vector3.op_Inequality(this.mTrans.position, vector3))
      this.mTrans.position = vector3;
    if (!this.runOnlyOnce || !Application.isPlaying)
      return;
    Object.Destroy((Object) this);
  }

  public enum Side
  {
    BottomLeft,
    Left,
    TopLeft,
    Top,
    TopRight,
    Right,
    BottomRight,
    Bottom,
    Center,
  }
}
