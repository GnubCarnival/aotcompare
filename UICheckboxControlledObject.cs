// Decompiled with JetBrains decompiler
// Type: UICheckboxControlledObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Object")]
public class UICheckboxControlledObject : MonoBehaviour
{
  public bool inverse;
  public GameObject target;

  private void OnActivate(bool isActive)
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    NGUITools.SetActive(this.target, !this.inverse ? isActive : !isActive);
    UIPanel inParents = NGUITools.FindInParents<UIPanel>(this.target);
    if (!Object.op_Inequality((Object) inParents, (Object) null))
      return;
    inParents.Refresh();
  }

  private void OnEnable()
  {
    UICheckbox component = ((Component) this).GetComponent<UICheckbox>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    this.OnActivate(component.isChecked);
  }
}
