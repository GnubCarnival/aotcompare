// Decompiled with JetBrains decompiler
// Type: CheckBoxShowSSInGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CheckBoxShowSSInGame : MonoBehaviour
{
  private bool init;

  private void OnActivate(bool yes)
  {
    if (!this.init)
      return;
    if (yes)
      PlayerPrefs.SetInt("showSSInGame", 1);
    else
      PlayerPrefs.SetInt("showSSInGame", 0);
  }

  private void Start()
  {
    this.init = true;
    if (PlayerPrefs.HasKey("showSSInGame"))
    {
      if (PlayerPrefs.GetInt("showSSInGame") == 1)
        ((Component) this).GetComponent<UICheckbox>().isChecked = true;
      else
        ((Component) this).GetComponent<UICheckbox>().isChecked = false;
    }
    else
    {
      ((Component) this).GetComponent<UICheckbox>().isChecked = true;
      PlayerPrefs.SetInt("showSSInGame", 1);
    }
  }
}
