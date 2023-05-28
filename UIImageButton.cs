// Decompiled with JetBrains decompiler
// Type: UIImageButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
  public string disabledSprite;
  public string hoverSprite;
  public string normalSprite;
  public string pressedSprite;
  public UISprite target;

  private void Awake()
  {
    if (!Object.op_Equality((Object) this.target, (Object) null))
      return;
    this.target = ((Component) this).GetComponentInChildren<UISprite>();
  }

  private void OnEnable() => this.UpdateImage();

  private void OnHover(bool isOver)
  {
    if (!this.isEnabled || !Object.op_Inequality((Object) this.target, (Object) null))
      return;
    this.target.spriteName = !isOver ? this.normalSprite : this.hoverSprite;
    this.target.MakePixelPerfect();
  }

  private void OnPress(bool pressed)
  {
    if (pressed)
    {
      this.target.spriteName = this.pressedSprite;
      this.target.MakePixelPerfect();
    }
    else
      this.UpdateImage();
  }

  private void UpdateImage()
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    this.target.spriteName = !this.isEnabled ? this.disabledSprite : (!UICamera.IsHighlighted(((Component) this).gameObject) ? this.normalSprite : this.hoverSprite);
    this.target.MakePixelPerfect();
  }

  public bool isEnabled
  {
    get
    {
      Collider collider = ((Component) this).collider;
      return Object.op_Inequality((Object) collider, (Object) null) && collider.enabled;
    }
    set
    {
      Collider collider = ((Component) this).collider;
      if (!Object.op_Inequality((Object) collider, (Object) null) || collider.enabled == value)
        return;
      collider.enabled = value;
      this.UpdateImage();
    }
  }
}
