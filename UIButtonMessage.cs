// Decompiled with JetBrains decompiler
// Type: UIButtonMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
  public string functionName;
  public bool includeChildren;
  private bool mHighlighted;
  private bool mStarted;
  public GameObject target;
  public UIButtonMessage.Trigger trigger;

  private void OnClick()
  {
    if (!((Behaviour) this).enabled || this.trigger != UIButtonMessage.Trigger.OnClick)
      return;
    this.Send();
  }

  private void OnDoubleClick()
  {
    if (!((Behaviour) this).enabled || this.trigger != UIButtonMessage.Trigger.OnDoubleClick)
      return;
    this.Send();
  }

  private void OnEnable()
  {
    if (!this.mStarted || !this.mHighlighted)
      return;
    this.OnHover(UICamera.IsHighlighted(((Component) this).gameObject));
  }

  private void OnHover(bool isOver)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver || !isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)
      this.Send();
    this.mHighlighted = isOver;
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled || (!isPressed || this.trigger != UIButtonMessage.Trigger.OnPress) && (isPressed || this.trigger != UIButtonMessage.Trigger.OnRelease))
      return;
    this.Send();
  }

  private void Send()
  {
    if (string.IsNullOrEmpty(this.functionName))
      return;
    if (Object.op_Equality((Object) this.target, (Object) null))
      this.target = ((Component) this).gameObject;
    if (this.includeChildren)
    {
      Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
      int index = 0;
      for (int length = componentsInChildren.Length; index < length; ++index)
        ((Component) componentsInChildren[index]).gameObject.SendMessage(this.functionName, (object) ((Component) this).gameObject, (SendMessageOptions) 1);
    }
    else
      this.target.SendMessage(this.functionName, (object) ((Component) this).gameObject, (SendMessageOptions) 1);
  }

  private void Start() => this.mStarted = true;

  public enum Trigger
  {
    OnClick,
    OnMouseOver,
    OnMouseOut,
    OnPress,
    OnRelease,
    OnDoubleClick,
  }
}
