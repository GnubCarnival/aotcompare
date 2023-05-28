// Decompiled with JetBrains decompiler
// Type: UICheckboxControlledComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Component")]
public class UICheckboxControlledComponent : MonoBehaviour
{
  public bool inverse;
  private bool mUsingDelegates;
  public MonoBehaviour target;

  private void OnActivate(bool isActive)
  {
    if (this.mUsingDelegates)
      return;
    this.OnActivateDelegate(isActive);
  }

  private void OnActivateDelegate(bool isActive)
  {
    if (!((Behaviour) this).enabled || !Object.op_Inequality((Object) this.target, (Object) null))
      return;
    ((Behaviour) this.target).enabled = !this.inverse ? isActive : !isActive;
  }

  private void Start()
  {
    UICheckbox component = ((Component) this).GetComponent<UICheckbox>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    this.mUsingDelegates = true;
    component.onStateChange += new UICheckbox.OnStateChange(this.OnActivateDelegate);
  }
}
