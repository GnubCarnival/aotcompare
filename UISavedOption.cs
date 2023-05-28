// Decompiled with JetBrains decompiler
// Type: UISavedOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
  public string keyName;
  private UICheckbox mCheck;
  private UIPopupList mList;

  private void Awake()
  {
    this.mList = ((Component) this).GetComponent<UIPopupList>();
    this.mCheck = ((Component) this).GetComponent<UICheckbox>();
    if (Object.op_Inequality((Object) this.mList, (Object) null))
      this.mList.onSelectionChange += new UIPopupList.OnSelectionChange(this.SaveSelection);
    if (!Object.op_Inequality((Object) this.mCheck, (Object) null))
      return;
    this.mCheck.onStateChange += new UICheckbox.OnStateChange(this.SaveState);
  }

  private void OnDestroy()
  {
    if (Object.op_Inequality((Object) this.mCheck, (Object) null))
      this.mCheck.onStateChange -= new UICheckbox.OnStateChange(this.SaveState);
    if (!Object.op_Inequality((Object) this.mList, (Object) null))
      return;
    this.mList.onSelectionChange -= new UIPopupList.OnSelectionChange(this.SaveSelection);
  }

  private void OnDisable()
  {
    if (!Object.op_Equality((Object) this.mCheck, (Object) null) || !Object.op_Equality((Object) this.mList, (Object) null))
      return;
    UICheckbox[] componentsInChildren = ((Component) this).GetComponentsInChildren<UICheckbox>(true);
    int index = 0;
    for (int length = componentsInChildren.Length; index < length; ++index)
    {
      UICheckbox uiCheckbox = componentsInChildren[index];
      if (uiCheckbox.isChecked)
      {
        this.SaveSelection(((Object) uiCheckbox).name);
        break;
      }
    }
  }

  private void OnEnable()
  {
    if (Object.op_Inequality((Object) this.mList, (Object) null))
    {
      string str = PlayerPrefs.GetString(this.key);
      if (string.IsNullOrEmpty(str))
        return;
      this.mList.selection = str;
    }
    else if (Object.op_Inequality((Object) this.mCheck, (Object) null))
    {
      this.mCheck.isChecked = PlayerPrefs.GetInt(this.key, 1) != 0;
    }
    else
    {
      string str = PlayerPrefs.GetString(this.key);
      UICheckbox[] componentsInChildren = ((Component) this).GetComponentsInChildren<UICheckbox>(true);
      int index = 0;
      for (int length = componentsInChildren.Length; index < length; ++index)
      {
        UICheckbox uiCheckbox = componentsInChildren[index];
        uiCheckbox.isChecked = ((Object) uiCheckbox).name == str;
      }
    }
  }

  private void SaveSelection(string selection) => PlayerPrefs.SetString(this.key, selection);

  private void SaveState(bool state) => PlayerPrefs.SetInt(this.key, !state ? 0 : 1);

  private string key => string.IsNullOrEmpty(this.keyName) ? "NGUI State: " + ((Object) this).name : this.keyName;
}
