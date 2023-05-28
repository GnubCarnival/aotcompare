// Decompiled with JetBrains decompiler
// Type: MapNameChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class MapNameChange : MonoBehaviour
{
  private void OnSelectionChange()
  {
    LevelInfo info = LevelInfo.getInfo(((Component) this).GetComponent<UIPopupList>().selection);
    if (info != null)
      GameObject.Find("LabelLevelInfo").GetComponent<UILabel>().text = info.desc;
    if (!((Component) this).GetComponent<UIPopupList>().items.Contains("Custom"))
    {
      ((Component) this).GetComponent<UIPopupList>().items.Add("Custom");
      ((Component) this).GetComponent<UIPopupList>().textScale *= 0.8f;
    }
    if (((Component) this).GetComponent<UIPopupList>().items.Contains("Custom (No PT)"))
      return;
    ((Component) this).GetComponent<UIPopupList>().items.Add("Custom (No PT)");
  }
}
