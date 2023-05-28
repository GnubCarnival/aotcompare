// Decompiled with JetBrains decompiler
// Type: UITooltip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
  public float appearSpeed = 10f;
  public UISprite background;
  private float mCurrent;
  private static UITooltip mInstance;
  private Vector3 mPos;
  private Vector3 mSize;
  private float mTarget;
  private Transform mTrans;
  private UIWidget[] mWidgets;
  public bool scalingTransitions = true;
  public UILabel text;
  public Camera uiCamera;

  private void Awake() => UITooltip.mInstance = this;

  private void OnDestroy() => UITooltip.mInstance = (UITooltip) null;

  private void SetAlpha(float val)
  {
    int index = 0;
    for (int length = this.mWidgets.Length; index < length; ++index)
    {
      UIWidget mWidget = this.mWidgets[index];
      Color color = mWidget.color;
      color.a = val;
      mWidget.color = color;
    }
  }

  private void SetText(string tooltipText)
  {
    if (Object.op_Inequality((Object) this.text, (Object) null) && !string.IsNullOrEmpty(tooltipText))
    {
      this.mTarget = 1f;
      if (Object.op_Inequality((Object) this.text, (Object) null))
        this.text.text = tooltipText;
      this.mPos = Input.mousePosition;
      if (Object.op_Inequality((Object) this.background, (Object) null))
      {
        Transform transform1 = ((Component) this.background).transform;
        Transform transform2 = ((Component) this.text).transform;
        Vector3 localPosition = transform2.localPosition;
        Vector3 localScale = transform2.localScale;
        this.mSize = Vector2.op_Implicit(this.text.relativeSize);
        this.mSize.x *= localScale.x;
        this.mSize.y *= localScale.y;
        this.mSize.x += (float) ((double) this.background.border.x + (double) this.background.border.z + ((double) localPosition.x - (double) this.background.border.x) * 2.0);
        this.mSize.y += (float) ((double) this.background.border.y + (double) this.background.border.w + (-(double) localPosition.y - (double) this.background.border.y) * 2.0);
        this.mSize.z = 1f;
        Vector3 mSize = this.mSize;
        transform1.localScale = mSize;
      }
      if (Object.op_Inequality((Object) this.uiCamera, (Object) null))
      {
        this.mPos.x = Mathf.Clamp01(this.mPos.x / (float) Screen.width);
        this.mPos.y = Mathf.Clamp01(this.mPos.y / (float) Screen.height);
        float num = (float) Screen.height * 0.5f / (this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y);
        Vector2 vector2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(num * this.mSize.x / (float) Screen.width, num * this.mSize.y / (float) Screen.height);
        this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector2.x);
        this.mPos.y = Mathf.Max(this.mPos.y, vector2.y);
        this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
        this.mPos = this.mTrans.localPosition;
        this.mPos.x = Mathf.Round(this.mPos.x);
        this.mPos.y = Mathf.Round(this.mPos.y);
        this.mTrans.localPosition = this.mPos;
      }
      else
      {
        if ((double) this.mPos.x + (double) this.mSize.x > (double) Screen.width)
          this.mPos.x = (float) Screen.width - this.mSize.x;
        if ((double) this.mPos.y - (double) this.mSize.y < 0.0)
          this.mPos.y = this.mSize.y;
        this.mPos.x -= (float) Screen.width * 0.5f;
        this.mPos.y -= (float) Screen.height * 0.5f;
      }
    }
    else
      this.mTarget = 0.0f;
  }

  public static void ShowText(string tooltipText)
  {
    if (!Object.op_Inequality((Object) UITooltip.mInstance, (Object) null))
      return;
    UITooltip.mInstance.SetText(tooltipText);
  }

  private void Start()
  {
    this.mTrans = ((Component) this).transform;
    this.mWidgets = ((Component) this).GetComponentsInChildren<UIWidget>();
    this.mPos = this.mTrans.localPosition;
    this.mSize = this.mTrans.localScale;
    if (Object.op_Equality((Object) this.uiCamera, (Object) null))
      this.uiCamera = NGUITools.FindCameraForLayer(((Component) this).gameObject.layer);
    this.SetAlpha(0.0f);
  }

  private void Update()
  {
    if ((double) this.mCurrent == (double) this.mTarget)
      return;
    this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, Time.deltaTime * this.appearSpeed);
    if ((double) Mathf.Abs(this.mCurrent - this.mTarget) < 1.0 / 1000.0)
      this.mCurrent = this.mTarget;
    this.SetAlpha(this.mCurrent * this.mCurrent);
    if (!this.scalingTransitions)
      return;
    Vector3 vector3_1 = Vector3.op_Multiply(this.mSize, 0.25f);
    vector3_1.y = -vector3_1.y;
    Vector3 vector3_2 = Vector3.op_Multiply(Vector3.one, (float) (1.5 - (double) this.mCurrent * 0.5));
    this.mTrans.localPosition = Vector3.Lerp(Vector3.op_Subtraction(this.mPos, vector3_1), this.mPos, this.mCurrent);
    this.mTrans.localScale = vector3_2;
  }
}
