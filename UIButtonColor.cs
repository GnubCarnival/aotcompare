// Decompiled with JetBrains decompiler
// Type: UIButtonColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : MonoBehaviour
{
  public float duration = 0.2f;
  public Color hover = new Color(0.6f, 1f, 0.2f, 1f);
  protected Color mColor;
  protected bool mHighlighted;
  protected bool mStarted;
  public Color pressed = Color.grey;
  public GameObject tweenTarget;

  protected void Init()
  {
    if (Object.op_Equality((Object) this.tweenTarget, (Object) null))
      this.tweenTarget = ((Component) this).gameObject;
    UIWidget component = this.tweenTarget.GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      this.mColor = component.color;
    }
    else
    {
      Renderer renderer = this.tweenTarget.renderer;
      if (Object.op_Inequality((Object) renderer, (Object) null))
      {
        this.mColor = renderer.material.color;
      }
      else
      {
        Light light = this.tweenTarget.light;
        if (Object.op_Inequality((Object) light, (Object) null))
        {
          this.mColor = light.color;
        }
        else
        {
          Debug.LogWarning((object) (NGUITools.GetHierarchy(((Component) this).gameObject) + " has nothing for UIButtonColor to color"), (Object) this);
          ((Behaviour) this).enabled = false;
        }
      }
    }
    this.OnEnable();
  }

  private void OnDisable()
  {
    if (!this.mStarted || !Object.op_Inequality((Object) this.tweenTarget, (Object) null))
      return;
    TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.color = this.mColor;
    ((Behaviour) component).enabled = false;
  }

  protected virtual void OnEnable()
  {
    if (!this.mStarted || !this.mHighlighted)
      return;
    this.OnHover(UICamera.IsHighlighted(((Component) this).gameObject));
  }

  public virtual void OnHover(bool isOver)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenColor.Begin(this.tweenTarget, this.duration, !isOver ? this.mColor : this.hover);
    this.mHighlighted = isOver;
  }

  public virtual void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenColor.Begin(this.tweenTarget, this.duration, !isPressed ? (!UICamera.IsHighlighted(((Component) this).gameObject) ? this.mColor : this.hover) : this.pressed);
  }

  private void Start()
  {
    if (this.mStarted)
      return;
    this.Init();
    this.mStarted = true;
  }

  public Color defaultColor
  {
    get
    {
      if (!this.mStarted)
        this.Init();
      return this.mColor;
    }
    set => this.mColor = value;
  }
}
