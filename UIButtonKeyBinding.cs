// Decompiled with JetBrains decompiler
// Type: UIButtonKeyBinding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("Game/UI/Button Key Binding")]
public class UIButtonKeyBinding : MonoBehaviour
{
  public KeyCode keyCode;

  private void Update()
  {
    if (UICamera.inputHasFocus || this.keyCode == null)
      return;
    if (Input.GetKeyDown(this.keyCode))
      ((Component) this).SendMessage("OnPress", (object) true, (SendMessageOptions) 1);
    if (!Input.GetKeyUp(this.keyCode))
      return;
    ((Component) this).SendMessage("OnPress", (object) false, (SendMessageOptions) 1);
    ((Component) this).SendMessage("OnClick", (SendMessageOptions) 1);
  }
}
