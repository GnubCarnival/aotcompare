// Decompiled with JetBrains decompiler
// Type: UIButtonKeys
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Keys")]
[RequireComponent(typeof (Collider))]
public class UIButtonKeys : MonoBehaviour
{
  public UIButtonKeys selectOnClick;
  public UIButtonKeys selectOnDown;
  public UIButtonKeys selectOnLeft;
  public UIButtonKeys selectOnRight;
  public UIButtonKeys selectOnUp;
  public bool startsSelected;

  private void OnClick()
  {
    if (!((Behaviour) this).enabled || !Object.op_Inequality((Object) this.selectOnClick, (Object) null))
      return;
    UICamera.selectedObject = ((Component) this.selectOnClick).gameObject;
  }

  private void OnKey(KeyCode key)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject))
      return;
    if (key != 9)
    {
      switch (key - 273)
      {
        case 0:
          if (!Object.op_Inequality((Object) this.selectOnUp, (Object) null))
            break;
          UICamera.selectedObject = ((Component) this.selectOnUp).gameObject;
          break;
        case 1:
          if (!Object.op_Inequality((Object) this.selectOnDown, (Object) null))
            break;
          UICamera.selectedObject = ((Component) this.selectOnDown).gameObject;
          break;
        case 2:
          if (!Object.op_Inequality((Object) this.selectOnRight, (Object) null))
            break;
          UICamera.selectedObject = ((Component) this.selectOnRight).gameObject;
          break;
        case 3:
          if (!Object.op_Inequality((Object) this.selectOnLeft, (Object) null))
            break;
          UICamera.selectedObject = ((Component) this.selectOnLeft).gameObject;
          break;
      }
    }
    else if (Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303))
    {
      if (Object.op_Inequality((Object) this.selectOnLeft, (Object) null))
        UICamera.selectedObject = ((Component) this.selectOnLeft).gameObject;
      else if (Object.op_Inequality((Object) this.selectOnUp, (Object) null))
        UICamera.selectedObject = ((Component) this.selectOnUp).gameObject;
      else if (Object.op_Inequality((Object) this.selectOnDown, (Object) null))
      {
        UICamera.selectedObject = ((Component) this.selectOnDown).gameObject;
      }
      else
      {
        if (!Object.op_Inequality((Object) this.selectOnRight, (Object) null))
          return;
        UICamera.selectedObject = ((Component) this.selectOnRight).gameObject;
      }
    }
    else if (Object.op_Inequality((Object) this.selectOnRight, (Object) null))
      UICamera.selectedObject = ((Component) this.selectOnRight).gameObject;
    else if (Object.op_Inequality((Object) this.selectOnDown, (Object) null))
      UICamera.selectedObject = ((Component) this.selectOnDown).gameObject;
    else if (Object.op_Inequality((Object) this.selectOnUp, (Object) null))
    {
      UICamera.selectedObject = ((Component) this.selectOnUp).gameObject;
    }
    else
    {
      if (!Object.op_Inequality((Object) this.selectOnLeft, (Object) null))
        return;
      UICamera.selectedObject = ((Component) this.selectOnLeft).gameObject;
    }
  }

  private void Start()
  {
    if (!this.startsSelected || !Object.op_Equality((Object) UICamera.selectedObject, (Object) null) && NGUITools.GetActive(UICamera.selectedObject))
      return;
    UICamera.selectedObject = ((Component) this).gameObject;
  }
}
