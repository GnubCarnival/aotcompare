// Decompiled with JetBrains decompiler
// Type: UISlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Slider")]
public class UISlider : IgnoreTimeScale
{
  public static UISlider current;
  public UISlider.Direction direction;
  public GameObject eventReceiver;
  public Transform foreground;
  public string functionName = "OnSliderChange";
  private Vector2 mCenter = Vector2.op_Implicit(Vector3.zero);
  private BoxCollider mCol;
  private UISprite mFGFilled;
  private Transform mFGTrans;
  private UIWidget mFGWidget;
  private bool mInitDone;
  private Vector2 mSize = Vector2.zero;
  private Transform mTrans;
  public int numberOfSteps;
  public UISlider.OnValueChange onValueChange;
  [HideInInspector]
  [SerializeField]
  private float rawValue = 1f;
  public Transform thumb;

  private void Awake()
  {
    this.mTrans = ((Component) this).transform;
    this.mCol = ((Component) this).collider as BoxCollider;
  }

  public void ForceUpdate() => this.Set(this.rawValue, true);

  private void Init()
  {
    this.mInitDone = true;
    if (Object.op_Inequality((Object) this.foreground, (Object) null))
    {
      this.mFGWidget = ((Component) this.foreground).GetComponent<UIWidget>();
      this.mFGFilled = Object.op_Equality((Object) this.mFGWidget, (Object) null) ? (UISprite) null : this.mFGWidget as UISprite;
      this.mFGTrans = ((Component) this.foreground).transform;
      if (Vector2.op_Equality(this.mSize, Vector2.zero))
        this.mSize = Vector2.op_Implicit(this.foreground.localScale);
      if (!Vector2.op_Equality(this.mCenter, Vector2.zero))
        return;
      this.mCenter = Vector2.op_Implicit(Vector3.op_Addition(this.foreground.localPosition, Vector3.op_Multiply(this.foreground.localScale, 0.5f)));
    }
    else if (Object.op_Inequality((Object) this.mCol, (Object) null))
    {
      if (Vector2.op_Equality(this.mSize, Vector2.zero))
        this.mSize = Vector2.op_Implicit(this.mCol.size);
      if (!Vector2.op_Equality(this.mCenter, Vector2.zero))
        return;
      this.mCenter = Vector2.op_Implicit(this.mCol.center);
    }
    else
      Debug.LogWarning((object) "UISlider expected to find a foreground object or a box collider to work with", (Object) this);
  }

  private void OnDrag(Vector2 delta) => this.UpdateDrag();

  private void OnDragThumb(GameObject go, Vector2 delta) => this.UpdateDrag();

  private void OnKey(KeyCode key)
  {
    float num = (double) this.numberOfSteps <= 1.0 ? 0.125f : 1f / (float) (this.numberOfSteps - 1);
    if (this.direction == UISlider.Direction.Horizontal)
    {
      if (key == 276)
      {
        this.Set(this.rawValue - num, false);
      }
      else
      {
        if (key != 275)
          return;
        this.Set(this.rawValue + num, false);
      }
    }
    else if (key == 274)
    {
      this.Set(this.rawValue - num, false);
    }
    else
    {
      if (key != 273)
        return;
      this.Set(this.rawValue + num, false);
    }
  }

  private void OnPress(bool pressed)
  {
    if (!pressed || UICamera.currentTouchID == -100)
      return;
    this.UpdateDrag();
  }

  private void OnPressThumb(GameObject go, bool pressed)
  {
    if (!pressed)
      return;
    this.UpdateDrag();
  }

  private void Set(float input, bool force)
  {
    if (!this.mInitDone)
      this.Init();
    float num = Mathf.Clamp01(input);
    if ((double) num < 1.0 / 1000.0)
      num = 0.0f;
    float sliderValue1 = this.sliderValue;
    this.rawValue = num;
    float sliderValue2 = this.sliderValue;
    if (!force && (double) sliderValue1 == (double) sliderValue2)
      return;
    Vector3 vector3 = Vector2.op_Implicit(this.mSize);
    if (this.direction == UISlider.Direction.Horizontal)
      vector3.x *= sliderValue2;
    else
      vector3.y *= sliderValue2;
    if (Object.op_Inequality((Object) this.mFGFilled, (Object) null) && this.mFGFilled.type == UISprite.Type.Filled)
      this.mFGFilled.fillAmount = sliderValue2;
    else if (Object.op_Inequality((Object) this.foreground, (Object) null))
    {
      this.mFGTrans.localScale = vector3;
      if (Object.op_Inequality((Object) this.mFGWidget, (Object) null))
      {
        if ((double) sliderValue2 > 1.0 / 1000.0)
        {
          ((Behaviour) this.mFGWidget).enabled = true;
          this.mFGWidget.MarkAsChanged();
        }
        else
          ((Behaviour) this.mFGWidget).enabled = false;
      }
    }
    if (Object.op_Inequality((Object) this.thumb, (Object) null))
    {
      Vector3 localPosition = this.thumb.localPosition;
      if (Object.op_Inequality((Object) this.mFGFilled, (Object) null) && this.mFGFilled.type == UISprite.Type.Filled)
      {
        if (this.mFGFilled.fillDirection == UISprite.FillDirection.Horizontal)
          localPosition.x = !this.mFGFilled.invert ? vector3.x : this.mSize.x - vector3.x;
        else if (this.mFGFilled.fillDirection == UISprite.FillDirection.Vertical)
          localPosition.y = !this.mFGFilled.invert ? vector3.y : this.mSize.y - vector3.y;
        else
          Debug.LogWarning((object) "Slider thumb is only supported with Horizontal or Vertical fill direction", (Object) this);
      }
      else if (this.direction == UISlider.Direction.Horizontal)
        localPosition.x = vector3.x;
      else
        localPosition.y = vector3.y;
      this.thumb.localPosition = localPosition;
    }
    UISlider.current = this;
    if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.functionName) && Application.isPlaying)
      this.eventReceiver.SendMessage(this.functionName, (object) sliderValue2, (SendMessageOptions) 1);
    if (this.onValueChange != null)
      this.onValueChange(sliderValue2);
    UISlider.current = (UISlider) null;
  }

  private void Start()
  {
    this.Init();
    if (Application.isPlaying && Object.op_Inequality((Object) this.thumb, (Object) null) && Object.op_Inequality((Object) ((Component) this.thumb).collider, (Object) null))
    {
      UIEventListener uiEventListener = UIEventListener.Get(((Component) this.thumb).gameObject);
      uiEventListener.onPress += new UIEventListener.BoolDelegate(this.OnPressThumb);
      uiEventListener.onDrag += new UIEventListener.VectorDelegate(this.OnDragThumb);
    }
    this.Set(this.rawValue, true);
  }

  private void UpdateDrag()
  {
    if (!Object.op_Inequality((Object) this.mCol, (Object) null) || !Object.op_Inequality((Object) UICamera.currentCamera, (Object) null) || UICamera.currentTouch == null)
      return;
    UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
    Ray ray = UICamera.currentCamera.ScreenPointToRay(Vector2.op_Implicit(UICamera.currentTouch.pos));
    Plane plane;
    // ISSUE: explicit constructor call
    ((Plane) ref plane).\u002Ector(Quaternion.op_Multiply(this.mTrans.rotation, Vector3.back), this.mTrans.position);
    float num;
    if (!((Plane) ref plane).Raycast(ray, ref num))
      return;
    Vector3 vector3_1 = Vector3.op_Subtraction(this.mTrans.localPosition, Vector3.op_Addition(this.mTrans.localPosition, Vector2.op_Implicit(Vector2.op_Subtraction(this.mCenter, Vector2.op_Multiply(this.mSize, 0.5f)))));
    Vector3 vector3_2 = Vector3.op_Addition(this.mTrans.InverseTransformPoint(((Ray) ref ray).GetPoint(num)), vector3_1);
    this.Set(this.direction != UISlider.Direction.Horizontal ? vector3_2.y / this.mSize.y : vector3_2.x / this.mSize.x, false);
  }

  public Vector2 fullSize
  {
    get => this.mSize;
    set
    {
      if (!Vector2.op_Inequality(this.mSize, value))
        return;
      this.mSize = value;
      this.ForceUpdate();
    }
  }

  public float sliderValue
  {
    get
    {
      float sliderValue = this.rawValue;
      if (this.numberOfSteps > 1)
        sliderValue = Mathf.Round(sliderValue * (float) (this.numberOfSteps - 1)) / (float) (this.numberOfSteps - 1);
      return sliderValue;
    }
    set => this.Set(value, false);
  }

  public enum Direction
  {
    Horizontal,
    Vertical,
  }

  public delegate void OnValueChange(float val);
}
